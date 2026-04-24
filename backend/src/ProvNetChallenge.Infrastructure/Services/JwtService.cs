using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using ProvNetChallenge.Application.Interfaces;
using ProvNetChallenge.Domain.Entities;

namespace ProvNetChallenge.Infrastructure.Services
{
    public class JwtService : IJwtService
    {
        private readonly IConfiguration _config;

        public JwtService(IConfiguration config)
        {
            _config = config;
        }

        public string GenerateJwtToken(User user)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["JwtSecretKey"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            // Define the claims (the assertions or data that travel INSIDE the token)
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Name, user.UserName),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()), // Unique identifier (Id) for the token (to prevent replay attacks)
                //new Claim("Rol", "Administrador") // Add custom claim for role (if needed)
            };

            // Configure the token properties (expiration, issuer, etc.)
            var token = new JwtSecurityToken(
                issuer: _config["Authentication:Schemes:Bearer:ValidIssuer"],
                audience: _config["Authentication:Schemes:Bearer:ValidAudiences:0"],
                claims: claims,
                expires: DateTime.UtcNow.AddHours(2), // The token will expire in 2 hours
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
        
    }
}
