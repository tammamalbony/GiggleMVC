using Giggle.Services;
using Microsoft.AspNetCore.Identity;
using System;
using System.Security.Cryptography;
namespace Giggle.Services
{
    public class PasswordHasherServices : IPasswordHasherServices
    {
        public string HashPassword(string password)
        {
            using (var sha256 = SHA256.Create())
            {
                var hashedBytes = sha256.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                return BitConverter.ToString(hashedBytes).Replace("-", "").ToLower();
            }
        }

        public bool VerifyPassword(string hashedPassword, string password)
        {
            var hashOfInput = HashPassword(password);
            return string.Equals(hashedPassword, hashOfInput, StringComparison.OrdinalIgnoreCase);
        }
    }
}
