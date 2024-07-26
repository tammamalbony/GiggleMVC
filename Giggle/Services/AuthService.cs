using Giggle.Models.DomainModels;
using Giggle.Models.DTOs;
using Giggle.Repositories;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Identity;
using Microsoft.JSInterop;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace Giggle.Services
{
    public class AuthService
    {
        private readonly UserRepository _userRepository;
        private readonly IPasswordHasherServices _passwordHasher;

        public AuthService(UserRepository userRepository, IPasswordHasherServices passwordHasher)
        {
            _userRepository = userRepository;
            _passwordHasher = passwordHasher;
        }

        public async Task<IdentityResult> RegisterUserAsync(RegisterModel model)
        {
            if (await _userRepository.IsUsernameUniqueAsync(model.Username) && await _userRepository.IsEmailUniqueAsync(model.Email))
            {
                var user = new UserDto
                {
                    Email = model.Email,
                    Password = _passwordHasher.HashPassword(model.Password),
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    Username = model.Username,
                    IsVerified = false,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow,
                    Terms = model.Terms
                };

                await _userRepository.CreateUserAsync(user);
                // Send verification email logic here
                return IdentityResult.Success;
            }
            return IdentityResult.Failed(new IdentityError { Description = "Username or Email already exists." });
        }

        public async Task<UserDto?> ValidateUserAsync(LoginModel model)
        {
            var user = await _userRepository.GetUserByUsernameAsync(model.Username);
            if (user != null && _passwordHasher.VerifyPassword(user.Password, model.Password))
            {
                return user;
            }
            return null;
        }
    }
}
