using LibraryExercise.Api.Models;

namespace LibraryExercise.Api.Services
{
    public interface IBooksService
    {
        Task<List<Book>> GetAllBooksAsync();
        Task<Book?> GetBookByIdAsync(int id);
    }
}
