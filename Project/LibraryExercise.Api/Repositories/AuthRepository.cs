using Microsoft.Data.SqlClient;
using LibraryExercise.Api.Models;
using LibraryExercise.Api.DTOs;
using LibraryExercise.Api.Data;

namespace LibraryExercise.Api.Repositories
{
    public class AuthRepository : IAuthRepository
    {
        private readonly SqlConnectionFactory _factory;

        public AuthRepository(SqlConnectionFactory factory)
        {
            _factory = factory;
        }

        public async Task<bool> EmailExists(string email)
        {
            using SqlConnection connection = _factory.CreateConnection();

            await connection.OpenAsync();

            var query = "SELECT COUNT(*) FROM Users WHERE user_email = @Email";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@Email", email);

            var result = await command.ExecuteScalarAsync();

            int exists = result == null ? 0 : Convert.ToInt32(result);

            return exists > 0;
        }

        public async Task CreateUser(string name, string email, string hashedPassword)
        {
            using SqlConnection connection = _factory.CreateConnection();

            await connection.OpenAsync();

            var query = @"
                INSERT INTO Users 
                (user_name, user_email, user_password)
                VALUES
                (@Name, @Email, @Password)";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@Name", name);
            command.Parameters.AddWithValue("@Email", email);
            command.Parameters.AddWithValue("@Password", hashedPassword);

            await command.ExecuteNonQueryAsync();
        }

        public async Task<User?> GetUserByEmail(string email)
        {
            using SqlConnection connection = _factory.CreateConnection();

            await connection.OpenAsync();

            var query = @"
                SELECT user_id, user_password, user_name
                FROM Users
                WHERE user_email = @Email";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@Email", email);

            using var reader = await command.ExecuteReaderAsync();

            if (await reader.ReadAsync())
            {
                return new User
                {
                    Id = reader.GetInt32(0),
                    HashedPassword = reader.GetString(1),
                    Name = reader.GetString(2),
                    Email = email
                };
            }

            return null;
        }

    }
}