using LibraryExercise.Api.Models;

namespace LibraryExercise.Api.Repositories
{
    public interface IBooksRepository
    {
        Task<List<Book>> GetAllBooksAsync();

        Task<Book?> GetBookByIdAsync(int id);

        //void AddBookAsync(Book book);
    }
}