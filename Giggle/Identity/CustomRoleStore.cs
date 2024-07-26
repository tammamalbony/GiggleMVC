using Microsoft.AspNetCore.Identity;
using System;
using System.Threading;
using System.Threading.Tasks;
using Giggle.Models.DomainModels;
using Giggle.Repositories;
using Giggle.Models.DTOs;

namespace Giggle.Identity
{
    public class CustomRoleStore : IRoleStore<IdentityRole>
    {
        private readonly RoleRepository _roleRepository;

        public CustomRoleStore(RoleRepository roleRepository)
        {
            _roleRepository = roleRepository;
        }

        public async Task<IdentityResult> CreateAsync(IdentityRole role, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            var roleDto = new RoleDto
            {
                Name = role.Name
            };

            var result = await _roleRepository.CreateRoleAsync(roleDto);

            return result > 0 ? IdentityResult.Success : IdentityResult.Failed(new IdentityError { Description = "Could not insert role." });
        }

        public async Task<IdentityResult> DeleteAsync(IdentityRole role, CancellationToken cancellationToken)
        {
            // Implement delete role logic here using _roleRepository
            throw new NotImplementedException();
        }

        public async Task<IdentityRole> FindByIdAsync(string roleId, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            var role = await _roleRepository.GetRoleByIdAsync(roleId);

            return role == null ? null : new IdentityRole { Id = role.Id.ToString(), Name = role.Name };
        }

        public async Task<IdentityRole> FindByNameAsync(string normalizedRoleName, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            var role = await _roleRepository.GetRoleByNameAsync(normalizedRoleName);

            return role == null ? null : new IdentityRole { Id = role.Id.ToString(), Name = role.Name };
        }

        public Task<string> GetNormalizedRoleNameAsync(IdentityRole role, CancellationToken cancellationToken)
        {
            return Task.FromResult(role.Name.ToUpper());
        }

        public Task<string> GetRoleIdAsync(IdentityRole role, CancellationToken cancellationToken)
        {
            return Task.FromResult(role.Id);
        }

        public Task<string> GetRoleNameAsync(IdentityRole role, CancellationToken cancellationToken)
        {
            return Task.FromResult(role.Name);
        }

        public Task SetNormalizedRoleNameAsync(IdentityRole role, string normalizedName, CancellationToken cancellationToken)
        {
            role.Name = normalizedName;
            return Task.CompletedTask;
        }

        public Task SetRoleNameAsync(IdentityRole role, string roleName, CancellationToken cancellationToken)
        {
            role.Name = roleName;
            return Task.CompletedTask;
        }

        public Task<IdentityResult> UpdateAsync(IdentityRole role, CancellationToken cancellationToken)
        {
            // Implement update role logic here using _roleRepository
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            // Nothing to dispose
        }
    }
}
