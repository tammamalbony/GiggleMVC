using Giggle.Models.DomainModels;
using Giggle.Models.DTOs;
using Giggle.Repositories;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

namespace Giggle.Providers
{
    public static class ValidationExtensions
    {
        public static async Task<(string, string)> ValidateEmailAsync(this UserRepository userRepository, string email)
        {
            if (!new EmailAddressAttribute().IsValid(email))
            {
                return ("is-invalid", "Invalid email format.");
            }

            var isUnique = await userRepository.IsEmailUniqueAsync(email);
            if (!isUnique)
            {
                return ("is-invalid", "Email is already taken.");
            }

            return ("is-valid", string.Empty);
        }

        public static async Task<(string, string)> ValidateUsernameAsync(this UserRepository userRepository, string username)
        {
            if (username.Length < 3)
            {
                return ("is-invalid", "Username must be at least 3 characters long.");
            }

            var isUnique = await userRepository.IsUsernameUniqueAsync(username);
            if (!isUnique)
            {
                return ("is-invalid", "Username is already taken.");
            }

            return ("is-valid", string.Empty);
        }

        public static (string, string) ValidateFirstName(this string firstName)
        {
            return firstName.Length >= 2 ? ("is-valid", string.Empty) : ("is-invalid", "First name must be at least 2 characters long.");
        }

        public static (string, string) ValidateLastName(this string lastName)
        {
            return lastName.Length >= 2 ? ("is-valid", string.Empty) : ("is-invalid", "Last name must be at least 2 characters long.");
        }

        public static (string, string) ValidatePassword(this string password)
        {
            var regex = new Regex(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^a-zA-Z\d]).{8,}$");
            if (!regex.IsMatch(password))
            {
                return ("is-invalid", "Password must be at least 8 characters and include at least one uppercase letter, one lowercase letter, one number, and one special character.");
            }

            return ("is-valid", string.Empty);
        }

        public static (string, string) ValidatePasswordRepeat(this string password, string passwordRepeat)
        {
            return passwordRepeat == password && passwordRepeat.Length > 0
                ? ("is-valid", string.Empty)
                : ("is-invalid", "Passwords do not match.");
        }

        public static (string, string) ValidateTerms(this bool termsAccepted)
        {
            return termsAccepted ? ("is-valid", string.Empty) : ("is-invalid", "You must accept the terms and conditions.");
        }
    }
}
