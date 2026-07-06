using LibraryExercise.Api.DTOs;
using LibraryExercise.Api.Repositories;

namespace LibraryExercise.Api.Services
{
    public class BorrowService : IBorrowService
    {
        private readonly IBorrowRepository _borrowRepository;

        public BorrowService(IBorrowRepository borrowRepository)
        {
            _borrowRepository = borrowRepository;
        }

        public async Task BorrowBook(int bookId, int userId)
        {
            //verif daca sunt exemplare disponibile
            //daca da => o imprumuta
            //daca nu => eroare
            int availableCopies = await _borrowRepository.GetNoOfAvailableCopies(bookId);

            if(availableCopies <= 0)
            {
                throw new Exception("Book is not available");
            }
            
            await _borrowRepository.BorrowBook(bookId, userId);
        }

        public async Task ReturnBook(int borrowId)
        {
            await _borrowRepository.ReturnBook(borrowId);
        }

        public Task<List<BorrowedBook>> GetAllBorrowedBooksByUser(int userId)
        {
            return _borrowRepository.GetAllBorrowedBooksByUser(userId);
        }
    }
}