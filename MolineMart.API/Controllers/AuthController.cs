using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApplicationParts;
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
        public AuthController(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager,
            IConfiguration configuration, IEmailSender emailSender)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _configuration = configuration;
            _emailSender = emailSender;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterModel model)
        {
            var user = new ApplicationUser { UserName = model.Email, Email = model.Email, FullName = model.FullName };
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
                var token = JwtHelper.GenerateJwtToken(user, _configuration, role);

                return Ok(new { token });
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
                ZipCode = user.ZipCode
            });
        }
    }
}
