using Microsoft.AspNetCore.Mvc;
using LibraryExercise.Api.Services;
using LibraryExercise.Api.Models;

namespace LibraryExercise.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BooksController : ControllerBase
    {
        private readonly IBooksService _booksService;

        public BooksController(IBooksService booksService)
        {
            _booksService = booksService;
        }

        [HttpGet]
        public async Task<IActionResult> GetBooks()
        {
            var books = await _booksService.GetAllBooksAsync();

            return Ok(books);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetBookById(int id)
        {
            var book = await _booksService.GetBookByIdAsync(id);

            if (book == null)
                return NotFound();

            return Ok(book);
        }
    }
}