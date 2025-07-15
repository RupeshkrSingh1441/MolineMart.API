using Microsoft.IdentityModel.Tokens;
using MolineMart.API.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace MolineMart.API.Helper
{
    public static class JwtHelper
    {
        public static string GenerateJwtToken(ApplicationUser user, IConfiguration config, IList<string> roles)
        {
            var claims = new List<Claim>
            {
                new(ClaimTypes.NameIdentifier, user.Id),
                new(ClaimTypes.Email, user.Email)
            };
            claims.AddRange(roles.Select(role => new Claim(ClaimTypes.NameIdentifier, role)));

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["Jwt:Key"]));
            var creds = new SigningCredentials(key,SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: config["Jwt:Issuer"],
                audience: config["Jwt:Audience"],
                claims: claims,
                expires:DateTime.Now.AddDays(7),
                signingCredentials:creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
