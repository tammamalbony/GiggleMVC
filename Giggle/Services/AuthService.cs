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
        private readonly IUserService _userService;
        private readonly IPasswordHasherServices _passwordHasher;

        public AuthService(IUserService userService, IPasswordHasherServices passwordHasher)
        {
            _userService = userService;
            _passwordHasher = passwordHasher;
        }

        public async Task<IdentityResult> RegisterUserAsync(RegisterModel model)
        {
            if (await _userService.IsUsernameUniqueAsync(model.Username) && await _userService.IsEmailUniqueAsync(model.Email))
            {

                await _userService.RegisterUserAsync(model);
                // Send verification email logic here
                return IdentityResult.Success;
            }
            return IdentityResult.Failed(new IdentityError { Description = "Username or Email already exists." });
        }

        public async Task<UserDto> ValidateUserAsync(LoginModel model)
        {
            // Check if the user exists
            var user = await _userService.GetUserByUsernameAsync(model.Username);
            if (user != null && _passwordHasher.VerifyPassword( user.Password, model.Password) )
            {
                return user;
            }
            return null;
        }
    }
}
