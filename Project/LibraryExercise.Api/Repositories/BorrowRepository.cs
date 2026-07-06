using Microsoft.Data.SqlClient;
using LibraryExercise.Api.DTOs;
using LibraryExercise.Api.Data;

namespace LibraryExercise.Api.Repositories
{
    public class BorrowRepository : IBorrowRepository
    {
        private readonly SqlConnectionFactory _factory;

        public BorrowRepository(SqlConnectionFactory factory)
        {
            _factory = factory;
        }

        public async Task<int> GetNoOfAvailableCopies(int bookId)
        {
            using SqlConnection connection =
                _factory.CreateConnection();

            await connection.OpenAsync();

            var query = @"
                SELECT no_of_available_copies
                FROM Books
                WHERE book_id = @BookId";

            SqlCommand command =
                new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@BookId", bookId);

            var result = await command.ExecuteScalarAsync();

            if(result == null)
            {
                throw new Exception("Book not found");
            }

            return Convert.ToInt32(result);        
        }

        public async Task BorrowBook(int bookId, int userId)
        {
            using SqlConnection connection =
                _factory.CreateConnection();

            await connection.OpenAsync();

            var transaction = connection.BeginTransaction();
            try
            {
                var insertBorrowQuery = @"
                INSERT INTO Borrow
                (
                    user_id, book_id, borrow_date, borrow_status  
                )
                VALUES
                (
                    @UserId, @BookId, GETDATE(), 'Borrowed'
                )";

                var insertCommand = new SqlCommand(insertBorrowQuery, connection, transaction);

                insertCommand.Parameters.AddWithValue("@BookId", bookId);

                insertCommand.Parameters.AddWithValue("@UserId", userId);

                await insertCommand.ExecuteNonQueryAsync();

                var updateBookQuery = @"
                    UPDATE Books
                    SET no_of_available_copies = no_of_available_copies - 1
                    WHERE book_id = @BookId
                ";

                var updateCommand = new SqlCommand(updateBookQuery, connection, transaction);

                updateCommand.Parameters.AddWithValue("@BookId", bookId);

                await updateCommand.ExecuteNonQueryAsync();

                transaction.Commit();
            }
            catch
            {
                transaction.Rollback();
                throw;
            }
        }

        public async Task ReturnBook(int borrowId)
        {
            using SqlConnection connection =
                _factory.CreateConnection();

            await connection.OpenAsync();

            var transaction = connection.BeginTransaction();

            try
            {
                //obtine id-ul cartii de returnat
                var getBookIdQuery = @"
                    SELECT book_id
                    FROM Borrow
                    WHERE borrow_id = @BorrowId
                    AND borrow_status = 'Borrowed'
                ";

                var getBookIdCommand = new SqlCommand(getBookIdQuery, connection, transaction);

                getBookIdCommand.Parameters.AddWithValue("@BorrowId", borrowId);

                var result = await getBookIdCommand.ExecuteScalarAsync();

                if(result == null)
                {
                    throw new Exception("Book already returned");
                }

                var bookId = Convert.ToInt32(result);

                //actualizeaza borrow_status la 'Returned' si return_date la data crt
                var updateBorrowQuery = @"
                    UPDATE Borrow 
                    SET borrow_status = 'Returned',
                    return_date = GETDATE()
                    WHERE borrow_id = @BorrowId
                ";

                var updateBorrowCommand = new SqlCommand(updateBorrowQuery, connection, transaction);

                updateBorrowCommand.Parameters.AddWithValue("@BorrowId", borrowId);

                await updateBorrowCommand.ExecuteNonQueryAsync();

                //actualizeaza nr de copii disponibile ale cartii
                var updateBookQuery = @"
                    UPDATE Books
                    SET no_of_available_copies = no_of_available_copies + 1
                    WHERE book_id = @BookId
                ";

                var updateBookCommand = new SqlCommand(updateBookQuery, connection, transaction);

                updateBookCommand.Parameters.AddWithValue("@BookId", bookId);

                await updateBookCommand.ExecuteNonQueryAsync();

                transaction.Commit();
            } 
            catch
            {
                transaction.Rollback();
                throw;
            }           
        }

        public async Task<List<BorrowedBook>> GetAllBorrowedBooksByUser(int userId)
        {
            var books = new List<BorrowedBook>();

            using SqlConnection connection =
                _factory.CreateConnection();

            await connection.OpenAsync();

            var query = @"
                SELECT b.borrow_id, bk.book_title, bk.book_author, b.borrow_date
                FROM Borrow b
                JOIN Books bk 
                ON b.book_id = bk.book_id
                WHERE b.borrow_status = 'Borrowed' AND
                b.user_id = @UserId
            ";     

            var command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@UserId", userId);

            using var reader = await command.ExecuteReaderAsync();

            while(await reader.ReadAsync())
            {
                books.Add(new BorrowedBook
                {
                    BorrowId = reader.GetInt32(0),
                    BookTitle = reader.GetString(1),
                    BookAuthor = reader.GetString(2),
                    BorrowDate = reader.GetDateTime(3)
                });
            }
            return books;     
        }
    }
}