using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using LibraryExercise.Api.Services;
using LibraryExercise.Api.Models;
using LibraryExercise.Api.DTOs;

namespace LibraryExercise.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BorrowController : ControllerBase
    {
        private readonly IBorrowService _borrowService;

        public BorrowController(IBorrowService borrowService)
        {
            _borrowService = borrowService;
        }

        [Authorize]
        [HttpPost("borrow")]
        public async Task<IActionResult> BorrowBook(BorrowBookRequest borrowBookRequest)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if(userIdClaim == null)
            {
                return Unauthorized();
            }
            
            var userId = int.Parse(userIdClaim.Value);

            await _borrowService.BorrowBook(borrowBookRequest.BookId, userId);

            return Ok(new {message = "Book borrowed successfully"});
        }

        [Authorize]
        [HttpPost("return/{borrowId}")]
        public async Task<IActionResult> ReturnBook(int borrowId)
        {
            await _borrowService.ReturnBook(borrowId);

            return Ok(new {message = "Book returned successfully"});
        }

        [Authorize]
        [HttpGet("my-borrowed-books")]
        public async Task<IActionResult> GetBorrowedBooks()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if(userIdClaim == null)
            {
                return Unauthorized();
            }
            
            var userId = int.Parse(userIdClaim.Value);

            var books = await _borrowService.GetAllBorrowedBooksByUser(userId);

            return Ok(books);
        }
    }
}