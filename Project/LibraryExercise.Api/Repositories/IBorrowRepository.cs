using LibraryExercise.Api.DTOs;

namespace LibraryExercise.Api.Repositories
{
    public interface IBorrowRepository
    {
        Task<int> GetNoOfAvailableCopies(int bookId);
        Task BorrowBook(int bookId, int userId);
        Task ReturnBook(int borrowId);
        Task<List<BorrowedBook>> GetAllBorrowedBooksByUser(int userId);
    }
}