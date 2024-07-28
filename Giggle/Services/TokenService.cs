using Giggle.Configurations;
using Giggle.Identity;
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
            if (string.IsNullOrWhiteSpace(_configManager.JwtIssuer))
                throw new ArgumentException("JWT Issuer is not set in the configuration.");

            _privateRsa = _configManager.JwtPrivateKey;
            _publicRsa = _configManager.JwtPublicKey;
        }

        public string GenerateToken(string username, string role, bool isVerified)
        {
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Name, username),
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
            var resultToke   = new JwtSecurityTokenHandler().WriteToken(token);
            if (Environment.GetEnvironmentVariable("APP_DEBUG") == "TRUE")
            {
                Console.WriteLine(resultToke);
            }
            return resultToke;
        }
        public void SetJwtCookie(HttpResponse response, string token)
        {
            var cookieOptions = GetCookieOptions();
            response.Cookies.Append("jwt", token, cookieOptions);
        }
        public bool ValidateToken(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var validationParameters = GetTokenValidationParameters();

            try
            {
                tokenHandler.ValidateToken(token, validationParameters, out SecurityToken validatedToken);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        //generating RSA keys 
        public Task generatingRSAkeys()
        {
            using (var rsa = new RSACryptoServiceProvider(2048))
            {
                string publicKeyBase64 = RsaKeyConverter.ExportPublicKey(rsa);
                string privateKeyBase64 = RsaKeyConverter.ExportPrivateKey(rsa);

                Console.WriteLine("Public Key (Base64): " + publicKeyBase64);
                Console.WriteLine("Private Key (Base64): " + privateKeyBase64);
            }
            return Task.CompletedTask;
        }
        public TokenValidationParameters GetTokenValidationParameters()
        {
            return new TokenValidationParameters
            {
                ValidateIssuer = false,
                ValidateAudience = false,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = _configManager.JwtIssuer,
                ValidAudience = _configManager.JwtIssuer,
                IssuerSigningKey = new RsaSecurityKey(_publicRsa),
                NameClaimType = JwtRegisteredClaimNames.Name,
                RoleClaimType = ClaimTypes.Role,
            };
        }

        public CookieOptions GetCookieOptions()
        {
            return new CookieOptions
            {
                HttpOnly = true,
                Secure = true,  // Ensure this is true if you're using HTTPS, which you should be.
                SameSite = SameSiteMode.None,  // This might need to be Lax depending on your cross-site needs.
                Expires = DateTimeOffset.Now.AddMinutes(30)  // Match token expiration
            };
        }
        public string GenerateVerificationToken()
        {
            byte[] tokenBytes = new byte[32]; // Adjust the length as per requirement (256 bits is 32 bytes)
            RandomNumberGenerator.Fill(tokenBytes);
            return Convert.ToBase64String(tokenBytes);
        }

        public void TestToken()
        {
            string username = "testuser";
            string role = "admin";
            bool isVerified = true;

            string token = GenerateToken(username, role, isVerified);
            Console.WriteLine($"Generated Token: {token}");

            bool isValid = ValidateToken(token);
            Console.WriteLine($"Is Token Valid: {isValid}");
        }
    }
}
