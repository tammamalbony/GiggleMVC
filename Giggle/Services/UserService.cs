using System.Threading.Tasks;
using Giggle.Models.DomainModels;
using Giggle.Models.DTOs;
using Giggle.Models.Responses;
using Giggle.Repositories;
using Microsoft.AspNetCore.Identity;

namespace Giggle.Services
{
    public class UserService : IUserService
    {
        private readonly UserRepository _userRepository;
        private readonly IPasswordHasherServices _passwordHasher;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;

        public UserService(UserRepository userRepository, IPasswordHasherServices passwordHasher, UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager)
        {
            _userRepository = userRepository;
            _passwordHasher = passwordHasher;
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public async Task<ServiceResult> RegisterUserAsync(RegisterModel model)
        {
            if (!await IsEmailUniqueAsync(model.Email))
            {
                return new ServiceResult { Success = false, ErrorMessage = "Email is already taken." };
            }

            if (!await IsUsernameUniqueAsync(model.Username))
            {
                return new ServiceResult { Success = false, ErrorMessage = "Username is already taken." };
            }

            var user = new UserDto
            {
                FirstName = model.FirstName,
                LastName = model.LastName,
                Username = model.Username,
                Email = model.Email,
                Password = _passwordHasher.HashPassword(model.Password),
                Terms = model.Terms,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                IsVerified = false // The user is not verified initially
            };

            await _userRepository.AddUserAsync(user);

            var identityUser = new IdentityUser { UserName = model.Username, Email = model.Email };
            var identityResult = await _userManager.CreateAsync(identityUser, model.Password);
            if (!identityResult.Succeeded)
            {
                return new ServiceResult { Success = false, ErrorMessage = "Failed to create user in Identity." };
            }

            return new ServiceResult { Success = true };
        }

        public async Task<bool> IsEmailUniqueAsync(string email)
        {
            return await _userRepository.IsEmailUniqueAsync(email);
        }

        public async Task<bool> IsUsernameUniqueAsync(string username)
        {
            return await _userRepository.IsUsernameUniqueAsync(username);
        }

        public async Task<ServiceResult> LoginUserAsync(LoginModel model)
        {
            var user = await _userManager.FindByNameAsync(model.Username);
            if (user == null)
            {
                return new ServiceResult { Success = false, ErrorMessage = "Invalid login attempt." };
            }

            var result = await _signInManager.PasswordSignInAsync(model.Username, model.Password, false, false);
            if (result.Succeeded)
            {
                return new ServiceResult { Success = true };
            }

            return new ServiceResult { Success = false, ErrorMessage = "Invalid login attempt." };
        }

        public async Task<ServiceResult> LogoutUserAsync()
        {
            await _signInManager.SignOutAsync();
            return new ServiceResult { Success = true };
        }
    }
}
