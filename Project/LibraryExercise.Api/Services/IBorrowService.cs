using LibraryExercise.Api.DTOs;

namespace LibraryExercise.Api.Services
{
    public interface IBorrowService
    {
        Task BorrowBook(int bookId, int userId);
        Task ReturnBook(int borrowId);
        Task<List<BorrowedBook>> GetAllBorrowedBooksByUser(int userId);
    }
}