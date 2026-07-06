using Microsoft.Data.SqlClient;
using LibraryExercise.Api.Models;
using LibraryExercise.Api.Data;

namespace LibraryExercise.Api.Repositories
{
    public class BooksRepository : IBooksRepository
    {
        private readonly SqlConnectionFactory _factory;

        public BooksRepository(SqlConnectionFactory factory)
        {
            _factory = factory;
        }

        public async Task<List<Book>> GetAllBooksAsync()
        {
            var books = new List<Book>();

            using SqlConnection connection =
                _factory.CreateConnection();

            await connection.OpenAsync();

            var query = @"
                SELECT book_id, book_title,
                       book_author, book_category,
                       no_of_available_copies, book_cover_image
                FROM Books";

            SqlCommand command =
                new SqlCommand(query, connection);

            using var reader =
                await command.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                books.Add(new Book
                {
                    Id = reader.GetInt32(0),
                    Title = reader.GetString(1),
                    Author = reader.GetString(2),
                    Category = reader.GetString(3),
                    NoOfAvailableCopies = reader.GetInt32(4),
                    ImageUrl = reader.IsDBNull(5)
                        ? null
                        : reader.GetString(5)
                });
            }

            return books;
        }

        public async Task<Book?> GetBookByIdAsync(int id)
        {
            using SqlConnection connection =
                _factory.CreateConnection();

            await connection.OpenAsync();

            var query = @"
                SELECT book_id, book_title,
                       book_author, book_category,
                       no_of_available_copies, book_cover_image
                FROM Books
                WHERE book_id = @Id";

            SqlCommand command =
                new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@Id", id);

            using var reader =
                await command.ExecuteReaderAsync();

            if (await reader.ReadAsync())
            {
                return new Book
                {
                    Id = reader.GetInt32(0),
                    Title = reader.GetString(1),
                    Author = reader.GetString(2),
                    Category = reader.GetString(3),
                    NoOfAvailableCopies = reader.GetInt32(4),
                    ImageUrl = reader.IsDBNull(5)
                        ? null
                        : reader.GetString(5)
                };
            }
            
            return null;
        }

    }
}