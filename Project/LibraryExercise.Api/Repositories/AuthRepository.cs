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
            //using -> cand metoda se termina, se inchide conexiunea la baza de date
            using SqlConnection connection = _factory.CreateConnection();

            //deschiderea efectiva a conexiunii cu bd
            await connection.OpenAsync();

            var query = @"
                INSERT INTO Users 
                (user_name, user_email, user_password)
                VALUES
                (@Name, @Email, @Password)";

            //crearea unui obiect SqlCommand care contine query-ul si conexiunea la baza de date
            //comanda nu se executa imediat, pt ca inca nu am adaugat parametrii ("@Name", "@Email", "@Password")
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
                    Id = reader.GetInt32(0),//user_id -> 0 in query
                    HashedPassword = reader.GetString(1),//user_password -> 1 in query
                    Name = reader.GetString(2),
                    Email = email
                };
            }

            return null;
        }

    }
}