using Giggle.Models.DomainModels;
using Giggle.Models.DTOs;
using Giggle.Models.Results;

namespace Giggle.Services
{
    public interface IUserService
    {
        Task<ServiceResults> RegisterUserAsync(RegisterModel model);
        Task<bool> IsEmailUniqueAsync(string email);
        Task<bool> IsUsernameUniqueAsync(string username);
        //Task<ServiceResults> LoginUserAsync(LoginModel model);
        //Task<ServiceResults> LogoutUserAsync();
        Task<UserDto?> GetUserByUsernameAsync(string username);
    }
}
