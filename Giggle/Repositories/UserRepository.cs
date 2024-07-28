using Dapper;
using Giggle.Configurations;
using Giggle.Models.DTOs;
using Giggle.Services;
using MySql.Data.MySqlClient;
using System.Data;

namespace Giggle.Repositories
{
    public class UserRepository
    {

        private readonly DatabaseService _databaseService;

        public UserRepository(DatabaseService databaseService)
        {
            _databaseService = databaseService;
        }

        public async Task<int> CreateUserAsync(UserDto user)
        {
            const string sql = @"
                INSERT INTO `user` (email, password, first_name, last_name, username, is_verified, created_at, updated_at, terms)
                VALUES (@Email, @Password, @FirstName, @LastName, @Username, @IsVerified, @CreatedAt, @UpdatedAt, @Terms);
                SELECT LAST_INSERT_ID();";

            using (var connection = _databaseService.CreateConnection())
            {
                connection.Open();
                return await connection.QuerySingleAsync<int>(sql, user);
            }
        }
        public async Task<UserDto?> GetUserByIdAsync(string userId)
        {
            const string sql = "SELECT * FROM `user` WHERE id = @Id;";

            using (var connection = _databaseService.CreateConnection())
            {
                connection.Open();
                return await connection.QuerySingleOrDefaultAsync<UserDto>(sql, new { Id = userId });
            }
        }

        public async Task<UserDto?> GetUserByUsernameAsync(string username)
        {
            const string sql = "SELECT * FROM `user` WHERE username = @Username;";

            using (var connection = _databaseService.CreateConnection())
            {
                connection.Open();
                return await connection.QuerySingleOrDefaultAsync<UserDto>(sql, new { Username = username });
            }
        }

        public async Task<UserDto?> GetUserByEmailAsync(string email)
        {
            const string sql = "SELECT * FROM `user` WHERE email = @Email;";

            using (var connection = _databaseService.CreateConnection())
            {
                connection.Open();
                return await connection.QuerySingleOrDefaultAsync<UserDto>(sql, new { Email = email });
            }
        }

        public async Task<bool> IsUsernameUniqueAsync(string username)
        {
            const string sql = "SELECT COUNT(1) FROM `user` WHERE username = @Username;";
            using (var connection = _databaseService.CreateConnection())
            {
                connection.Open();
                var count = await connection.ExecuteScalarAsync<int>(sql, new { Username = username });
                return count == 0;
            }
        }

        public async Task<bool> IsEmailUniqueAsync(string email)
        {
            const string sql = "SELECT COUNT(1) FROM `user` WHERE email = @Email;";
            using (var connection = _databaseService.CreateConnection())
            {
                connection.Open();
                var count = await connection.ExecuteScalarAsync<int>(sql, new { Email = email });
                return count == 0;
            }
        }

        public async Task AddUserAsync(UserDto user)
        {
            const string sql = @"
                INSERT INTO `user` (email, password, first_name, last_name, username, is_verified, created_at, updated_at, terms,verification_token,role)
                VALUES (@Email, @Password, @FirstName, @LastName, @Username, @IsVerified, @CreatedAt, @UpdatedAt, @Terms,@VerificationToken,@Role);
                SELECT LAST_INSERT_ID();";

            using (var connection = _databaseService.CreateConnection())
            {
                connection.Open();
                await connection.ExecuteAsync(sql, user);
            }
        }

        internal async Task<bool> UpdateUserAsync(UserDto userDto)
        {
            const string sql = @"
        UPDATE `user` 
        SET email = @Email, 
            password = @Password, 
            first_name = @FirstName, 
            last_name = @LastName, 
            username = @Username, 
            is_verified = @IsVerified, 
            updated_at = @UpdatedAt, 
            terms = @Terms, 
            role = @Role,
            verification_token = @VerificationToken 
        WHERE id = @Id;";

            using (var connection = _databaseService.CreateConnection())
            {
                connection.Open();
                var result = await connection.ExecuteAsync(sql, userDto);
                return result > 0;
            }
        }


        internal async Task<bool> IsEmailConfirmedAsync(int userId)
        {
            const string sql = "SELECT is_verified FROM `user` WHERE id = @UserId;";
            using (var connection = _databaseService.CreateConnection())
            {
                connection.Open();
                var isVerified = await connection.QuerySingleOrDefaultAsync<bool>(sql, new { UserId = userId });
                return isVerified;
            }
        }

        internal async Task SetEmailConfirmedAsync(int userId, bool confirmed)
        {
            const string sql = "UPDATE `user` SET is_verified = @IsVerified WHERE id = @UserId;";
            using (var connection = _databaseService.CreateConnection())
            {
                connection.Open();
                await connection.ExecuteAsync(sql, new { UserId = userId, IsVerified = confirmed });
            }
        }
        internal async Task<bool> DeleteUserAsync(int userId)
        {
            const string sql = "DELETE FROM `user` WHERE id = @UserId;";
            using (var connection = _databaseService.CreateConnection())
            {
                connection.Open();
                var result = await connection.ExecuteAsync(sql, new { UserId = userId });
                return result > 0;
            }
        }
    }
}
