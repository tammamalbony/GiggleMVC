using Giggle.Configurations;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;

namespace Giggle.Services
{
    public class TokenService
    {
        private readonly CustomConfigurationManager _configManager;
        private readonly RSA _privateRsa;
        private readonly RSA _publicRsa;

        public TokenService(CustomConfigurationManager configManager)
        {
            _configManager = configManager;

            _privateRsa = RSA.Create();
            _privateRsa.ImportFromPem(_configManager.JwtPrivateKey);

            _publicRsa = RSA.Create();
            _publicRsa.ImportFromPem(_configManager.JwtPublicKey);
        }

        public string GenerateToken(string username, string role, bool isVerified)
        {
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, username),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(ClaimTypes.Role, role),
                new Claim("IsVerified", isVerified.ToString())
            };

            var creds = new SigningCredentials(new RsaSecurityKey(_privateRsa), SecurityAlgorithms.RsaSha256);

            var token = new JwtSecurityToken(
                issuer: _configManager.JwtIssuer,
                audience: _configManager.JwtIssuer,
                claims: claims,
                expires: DateTime.Now.AddMinutes(30),
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public TokenValidationParameters GetTokenValidationParameters()
        {
            return new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = _configManager.JwtIssuer,
                ValidAudience = _configManager.JwtIssuer,
                IssuerSigningKey = new RsaSecurityKey(_publicRsa)
            };
        }
    }
}
