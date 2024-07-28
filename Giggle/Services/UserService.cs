using System.Threading.Tasks;
using Giggle.Models.DomainModels;
using Giggle.Models.DTOs;
using Giggle.Models.Results;
using Giggle.Repositories;
using Microsoft.AspNetCore.Identity;

namespace Giggle.Services
{
    public class UserService : IUserService
    {
        private readonly UserRepository _userRepository;
        private readonly IPasswordHasherServices _passwordHasher;
        //private readonly UserManager<IdentityUser> _userManager;
        //private readonly SignInManager<IdentityUser> _signInManager;
        public readonly TokenService _tokenService;

        //public UserService(UserRepository userRepository, IPasswordHasherServices passwordHasher, UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager,TokenService tokenService)
        //{
        //    _userRepository = userRepository;
        //    _passwordHasher = passwordHasher;
        //    _userManager = userManager;
        //    _signInManager = signInManager;
        //    _tokenService = tokenService;
        //}
        public UserService(UserRepository userRepository, IPasswordHasherServices passwordHasher, TokenService tokenService)
        {
            _userRepository = userRepository;
            _passwordHasher = passwordHasher;
            _tokenService = tokenService;
        }
        public async Task<ServiceResults> RegisterUserAsync(RegisterModel model)
        {
            if (!await IsEmailUniqueAsync(model.Email))
            {
                return new ServiceResults { Success = false, Message = "Email is already taken." };
            }

            if (!await IsUsernameUniqueAsync(model.Username))
            {
                return new ServiceResults { Success = false, Message = "Username is already taken." };
            }

            var user = new UserDto
            {
                FirstName = model.FirstName,
                LastName = model.LastName,
                Username = model.Username,
                Email = model.Email,
                Password = _passwordHasher.HashPassword(model.Password),
                Terms = model.Terms,
                Role = "Admin",
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                VerificationToken = _tokenService.GenerateVerificationToken(),
                IsVerified = false
            };

            await _userRepository.AddUserAsync(user);

            //var identityUser = new IdentityUser { UserName = model.Username, Email = model.Email };
            //var identityResult = await _userManager.CreateAsync(identityUser, model.Password);
            //if (!identityResult.Succeeded)
            //{
            //    return new ServiceResults { Success = false, Message = "Failed to create user in Identity." };
            //}

            return new ServiceResults { Success = true, Message = "Registration successful. Please verify your email to complete the process." };
        }

        public async Task<bool> IsEmailUniqueAsync(string email)
        {
            var result =  await _userRepository.IsEmailUniqueAsync(email);
            if (result == false)
            {
                throw new Exceptions.ValidationException("Email is already Taken.");
            }
            return result;
        }

        public async Task<bool> IsUsernameUniqueAsync(string username)
        {
            var result = await _userRepository.IsUsernameUniqueAsync(username);
            if (result == false)
            {
                throw new Exceptions.ValidationException("User Name is already Taken.");
            }
            return result;
        }

        //public async Task<ServiceResults> LoginUserAsync(LoginModel model)
        //{
        //    var user = await _userManager.FindByNameAsync(model.Username);
        //    if (user == null)
        //    {
        //        return new ServiceResults { Success = false, Message = "Invalid login attempt. User not found." };
        //    }

        //    var result = await _signInManager.PasswordSignInAsync(model.Username, model.Password, false, false);
        //    if (result.Succeeded)
        //    {
        //        return new ServiceResults { Success = true, Message = "Login successful. Welcome back!" };
        //    }

        //    return new ServiceResults { Success = false, Message = "Invalid login attempt. Please check your credentials and try again." };
        //}

        //public async Task<ServiceResults> LogoutUserAsync()
        //{
        //    await _signInManager.SignOutAsync();
        //    return new ServiceResults { Success = true, Message = "You have been successfully logged out." };
        //}

        public async Task<UserDto?> GetUserByUsernameAsync(string username)
        {
            return await _userRepository.GetUserByUsernameAsync(username); 
        }
    }
}
