using Microsoft.AspNetCore.Identity;
using System;
using System.Threading;
using System.Threading.Tasks;
using Giggle.Models.DomainModels;
using Giggle.Repositories;
using Giggle.Models.DTOs;

namespace Giggle.Identity
{
    public class CustomUserStore : IUserStore<IdentityUser>, IUserPasswordStore<IdentityUser>
    {
        private readonly UserRepository _userRepository;

        public CustomUserStore(UserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<IdentityResult> CreateAsync(IdentityUser user, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            var userDto = new UserDto
            {
                Username = user.UserName,
                Email = user.Email,
                Password = user.PasswordHash
            };

            var result = await _userRepository.CreateUserAsync(userDto);

            return result > 0 ? IdentityResult.Success : IdentityResult.Failed(new IdentityError { Description = "Could not insert user." });
        }

        public async Task<IdentityUser> FindByIdAsync(string userId, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            var user = await _userRepository.GetUserByIdAsync(userId);

            return user == null ? null : new IdentityUser { Id = user.Id.ToString(), UserName = user.Username, Email = user.Email, PasswordHash = user.Password };
        }

        public async Task<IdentityUser> FindByNameAsync(string normalizedUserName, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            var user = await _userRepository.GetUserByUsernameAsync(normalizedUserName);

            return user == null ? null : new IdentityUser { Id = user.Id.ToString(), UserName = user.Username, Email = user.Email, PasswordHash = user.Password };
        }

        public async Task<IdentityResult> UpdateAsync(IdentityUser user, CancellationToken cancellationToken)
        {
            // Implement update user logic here using _userRepository
            throw new NotImplementedException();
        }

        public async Task<IdentityResult> DeleteAsync(IdentityUser user, CancellationToken cancellationToken)
        {
            // Implement delete user logic here using _userRepository
            throw new NotImplementedException();
        }

        public Task<string> GetUserIdAsync(IdentityUser user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.Id);
        }

        public Task<string> GetUserNameAsync(IdentityUser user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.UserName);
        }

        public Task SetUserNameAsync(IdentityUser user, string userName, CancellationToken cancellationToken)
        {
            user.UserName = userName;
            return Task.CompletedTask;
        }

        public Task<string> GetNormalizedUserNameAsync(IdentityUser user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.UserName.ToUpper());
        }

        public Task SetNormalizedUserNameAsync(IdentityUser user, string normalizedName, CancellationToken cancellationToken)
        {
            user.UserName = normalizedName;
            return Task.CompletedTask;
        }

        public Task SetPasswordHashAsync(IdentityUser user, string passwordHash, CancellationToken cancellationToken)
        {
            user.PasswordHash = passwordHash;
            return Task.CompletedTask;
        }

        public Task<string> GetPasswordHashAsync(IdentityUser user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.PasswordHash);
        }

        public Task<bool> HasPasswordAsync(IdentityUser user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.PasswordHash != null);
        }

        public void Dispose()
        {
            // Nothing to dispose
        }
    }
}
