using LibraryExercise.Api.Controllers;
using LibraryExercise.Api.DTOs;
using LibraryExercise.Api.Services;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using Moq;

using System.Security.Claims;

namespace LibraryExercise.Api.Tests.Controllers;

public class BorrowControllerTests
{
    private readonly Mock<IBorrowService> _service;
    private readonly BorrowController _controller;

    public BorrowControllerTests()
    {
        _service = new Mock<IBorrowService>();
        _controller = new BorrowController(_service.Object);
    }

    //creeaza un utilizator fals logat
    private void SetAuthenticatedUser(int userId)
    {
        var claims = new[]
        {
            new Claim(
                ClaimTypes.NameIdentifier,
                userId.ToString()
            )
        };

        var identity =
            new ClaimsIdentity(
                claims,
                "TestAuthentication"
            );

        var principal =
            new ClaimsPrincipal(
                identity
            );

        _controller.ControllerContext =
            new ControllerContext
            {
                HttpContext =
                    new DefaultHttpContext
                    {
                        User = principal
                    }
            };
    }

    //creeaza un context pt utilizatorii nelogati
    private void SetUnauthenticatedUser()
    {
        _controller.ControllerContext =
            new ControllerContext
            {
                HttpContext = new DefaultHttpContext()
            };
    }

    //test1 -> BorrowBook() ret Unauthorized cand nu se gaseste userId
    [Fact]
    public async Task BorrowBook_ShouldReturnUnauthorized_WhenUserIdMissing()
    {
        //arrange
        SetUnauthenticatedUser();
        var request = new BorrowBookRequest {BookId = 1};

        //act
        var result = await _controller.BorrowBook(request);

        //assert
        Assert.IsType<UnauthorizedResult>(result);
    }
    
    //test2 -> BorrowBook() reuseste, cand se gaseste userId
    [Fact]
    public async Task BorrowBook_ShouldReturnOk_WhenBorrowSucceeds()
    {
        //arrange
        SetAuthenticatedUser(1);
        var request = new BorrowBookRequest{BookId = 10};

        //act
        var result = await _controller.BorrowBook(request);

        //assert
        var ok = Assert.IsType<OkObjectResult>(result);

        Assert.Equal(200, ok.StatusCode);

        _service.Verify(x => x.BorrowBook(10, 1), Times.Once);
    }
    
    //test3 -> ReturnBook() ret Ok()
    [Fact]
    public async Task ReturnBook_ShouldReturnOk()
    {
        //act
        var result = await _controller.ReturnBook(5);

        //assert
        var ok = Assert.IsType<OkObjectResult>(result);

        Assert.Equal(200, ok.StatusCode);

        _service.Verify(x => x.ReturnBook(5), Times.Once);
    }
    
    //test4 -> GetBorrowedBooks() ret Unauthorized cand nu se gaseste userId
    [Fact]
    public async Task GetBorrowedBooks_ShouldReturnUnauthorized_WhenUserIdMissing()
    {
        //act
        SetUnauthenticatedUser();
        var result = await _controller.GetBorrowedBooks();

        //assert
        Assert.IsType<UnauthorizedResult>(result);
    }
    
    //test5 -> GetBorrowedBooks() reuseste, cand se gaseste userId
    [Fact]
    public async Task GetBorrowedBooks_ShouldReturnOk_WhenUserExists()
    {
        //arrange
        SetAuthenticatedUser(1);
        var books = new List<BorrowedBook>
        {
            new BorrowedBook
            {
                BorrowId = 1,
                BookTitle = "Harry Potter",
                BookAuthor = "J.K. Rowling",
                BorrowDate = DateTime.Now
            }
        };

        _service
            .Setup(x =>
            x.GetAllBorrowedBooksByUser(1))
            .ReturnsAsync(books);

        //act
        var result = await _controller.GetBorrowedBooks();

        //assert
        var ok = Assert.IsType<OkObjectResult>(result);

        Assert.Equal(200, ok.StatusCode);

        _service.Verify(
            x =>
            x.GetAllBorrowedBooksByUser(1),
            Times.Once
        );
    }
}