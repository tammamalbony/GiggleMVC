using Giggle.Models.DomainModels;
using Giggle.Models.Responses;

namespace Giggle.Services
{
    public interface IUserService
    {
        Task<ServiceResult> RegisterUserAsync(RegisterModel model);
        Task<bool> IsEmailUniqueAsync(string email);
        Task<bool> IsUsernameUniqueAsync(string username);
        Task<ServiceResult> LoginUserAsync(LoginModel model);
        Task<ServiceResult> LogoutUserAsync();
    }
}
