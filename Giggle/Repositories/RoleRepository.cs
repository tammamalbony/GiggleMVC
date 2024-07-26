using Dapper;
using Giggle.Configurations;
using Giggle.Models.DTOs;
using Giggle.Services;
using MySql.Data.MySqlClient;
using System.Data;
using System.Threading.Tasks;

namespace Giggle.Repositories
{
    public class RoleRepository
    {
        private readonly DatabaseService _databaseService;

        public RoleRepository(DatabaseService databaseService)
        {
            _databaseService = databaseService;
        }

        public async Task<int> CreateRoleAsync(RoleDto role)
        {
            const string sql = @"
                INSERT INTO `role` (name)
                VALUES (@Name);
                SELECT LAST_INSERT_ID();";

            using (var connection = _databaseService.CreateConnection())
            {
                connection.Open();
                return await connection.QuerySingleAsync<int>(sql, role);
            }
        }

        public async Task<RoleDto> GetRoleByIdAsync(string roleId)
        {
            const string sql = "SELECT * FROM `role` WHERE id = @Id;";

            using (var connection = _databaseService.CreateConnection())
            {
                connection.Open();
                return await connection.QuerySingleOrDefaultAsync<RoleDto>(sql, new { Id = roleId });
            }
        }

        public async Task<RoleDto> GetRoleByNameAsync(string roleName)
        {
            const string sql = "SELECT * FROM `role` WHERE name = @Name;";

            using (var connection = _databaseService.CreateConnection())
            {
                connection.Open();
                return await connection.QuerySingleOrDefaultAsync<RoleDto>(sql, new { Name = roleName });
            }
        }

        // Additional methods for updating and deleting roles can be added here
    }
}
