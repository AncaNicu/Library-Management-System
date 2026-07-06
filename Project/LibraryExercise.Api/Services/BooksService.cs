using LibraryExercise.Api.Models;
using LibraryExercise.Api.Repositories;

namespace LibraryExercise.Api.Services
{
    public class BooksService : IBooksService
    {
        private readonly IBooksRepository _booksRepository;

        public BooksService(IBooksRepository booksRepository)
        {
            _booksRepository = booksRepository;
        }

        public Task<List<Book>> GetAllBooksAsync()
        {
            return _booksRepository.GetAllBooksAsync();
        }

        public Task<Book?> GetBookByIdAsync(int id)
        {
            return _booksRepository.GetBookByIdAsync(id);
        }
    }
}