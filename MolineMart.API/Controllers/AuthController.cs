using Azure.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Microsoft.EntityFrameworkCore;
using MolineMart.API.Data;
using MolineMart.API.Dto;
using MolineMart.API.Helper;
using MolineMart.API.Models;
using MolineMart.API.Services;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;

namespace MolineMart.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IConfiguration _configuration;
        private readonly IEmailSender _emailSender;
        private readonly ApplicationDbContext _context;
        public AuthController(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager,
            IConfiguration configuration, IEmailSender emailSender,ApplicationDbContext context)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _configuration = configuration;
            _emailSender = emailSender;
            _context = context;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterModel model)
        {
            var user = new ApplicationUser 
            { 
                UserName = model.Email, 
                Email = model.Email, 
                FullName = model.FullName,

                AddressLine1 = model.AddressLine1,
                AddressLine2 = model.AddressLine2,
                City = model.City,
                State = model.State,
                Country = model.Country,
                ZipCode = model.ZipCode
            };
            var result = await _userManager.CreateAsync(user, model.Password);
            try
            {
                if (result.Succeeded)
                {

                    if (!await _roleManager.RoleExistsAsync("User"))
                        await _roleManager.CreateAsync(new IdentityRole("User"));

                    await _userManager.AddToRoleAsync(user, "User");

                    var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    var confirmationLink = Url.Action(nameof(ConfirmEmail), "Auth",
                        new { userId = user.Id, token = WebUtility.UrlEncode(token) }, Request.Scheme);

                    await _emailSender.SendEmailAsync(user.Email, "Confirm your email",
                        $"Please <a href='{confirmationLink}'>click here</a> to confirm your email.");

                    return Ok("Registration successful! Please check your email to confirm your account.");
                }

                return BadRequest(result.Errors);
            }
            catch (Exception ex)
            {
                var message = ex.StackTrace;
                return BadRequest(result.Errors);
            }
        }

        [HttpGet("confirm-email")]
        public async Task<IActionResult> ConfirmEmail(string userId, string token)
        {
            var result = new IdentityResult();
            try
            {
                var user = await _userManager.FindByIdAsync(userId);
                if (user == null) return BadRequest("User not found");

                var decodeToken = WebUtility.UrlDecode(token);
                result = await _userManager.ConfirmEmailAsync(user, decodeToken);
                return result.Succeeded ? Ok("Email confirmed") : BadRequest("Invalid token");
            }
            catch (Exception ex)
            {
                var message = ex.StackTrace;
                return BadRequest(result.Errors);
            }

        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginModel model)
        {
            try
            {
                var user = await _userManager.FindByEmailAsync(model.Email);


                if (user == null || !await _userManager.CheckPasswordAsync(user, model.Password))
                    return Unauthorized("Invalid credentials");


                if (!await _userManager.IsEmailConfirmedAsync(user))
                    return Unauthorized("Email not confirmed");

                var role = await _userManager.GetRolesAsync(user);
                var accessToken = JwtHelper.GenerateJwtToken(user, _configuration, role);
                var refreshToken = JwtHelper.GenerateRefreshToken();

                // Save refresh token
                var refreshEntity = new RefreshToken
                {
                    Token = refreshToken,
                    UserId = user.Id,
                    Expires = DateTime.UtcNow.AddDays(7)
                };

                _context.RefreshTokens.Add(refreshEntity);
                await _context.SaveChangesAsync();

                return Ok(new
                {
                    accessToken,
                    refreshToken,
                    user = new { user.FullName, user.Email }
                });
            }
            catch (Exception ex)
            {
                var message = ex.StackTrace;
                return BadRequest();
            }
        }
        
        [HttpGet("profile")]
        [Authorize]
        public async Task<IActionResult> GetProfile()
        {
            // NameIdentifier is the user id stored in the JWT (usually)
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId)) return Unauthorized();

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null) return NotFound();

            var roles = await _userManager.GetRolesAsync(user);

            return Ok(new
            {
                id = user.Id,
                fullName = user.FullName,
                email = user.Email,
                phonNumber=user.PhoneNumber,
                AddressLine1=user.AddressLine1,
                AddressLine2 =user.AddressLine2,
                City =user.City,
                State =user.State,
                Country =user.Country,
                ZipCode = user.ZipCode,
                role = roles
            });
        }

        [Authorize]
        [HttpPut("update-profile")]
        public async Task<IActionResult> UpdateProfile(UpdateProfileDto dto)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var user = await _userManager.FindByIdAsync(userId);

            if (user == null)
                return NotFound();

            user.FullName = dto.FullName;
            user.AddressLine1 = dto.AddressLine1;
            user.City = dto.City;
            user.State = dto.State;
            user.Country = dto.Country;
            user.ZipCode = dto.ZipCode;

            await _userManager.UpdateAsync(user);

            return Ok(new { message = "Profile updated successfully" });
        }

        [HttpPost("refresh-token")]
        public async Task<IActionResult> RefreshToken([FromBody] RefreshRequestDto dto)
        {
            var refresh = await _context.RefreshTokens
                .FirstOrDefaultAsync(x => x.Token == dto.RefreshToken);

            if (refresh == null || !refresh.IsActive)
                return Unauthorized(new { message = "Invalid refresh token" });

            var user = await _userManager.FindByIdAsync(refresh.UserId);
            var roles = await _userManager.GetRolesAsync(user);

            // rotate token
            refresh.Revoked = DateTime.UtcNow;

            var newRefreshToken = new RefreshToken
            {
                Token = JwtHelper.GenerateRefreshToken(),
                UserId = user.Id,
                Expires = DateTime.UtcNow.AddDays(7)
            };

            _context.RefreshTokens.Add(newRefreshToken);
            await _context.SaveChangesAsync();

            var newAccessToken = JwtHelper.GenerateJwtToken(user, _configuration, roles);

            return Ok(new
            {
                accessToken = newAccessToken,
                refreshToken = newRefreshToken.Token
            });
        }
    }
}
