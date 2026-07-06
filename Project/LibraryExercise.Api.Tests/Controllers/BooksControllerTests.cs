using LibraryExercise.Api.Controllers;
using LibraryExercise.Api.DTOs;
using LibraryExercise.Api.Models;
using LibraryExercise.Api.Services;

using Microsoft.AspNetCore.Mvc;

using Moq;

namespace LibraryExercise.Api.Tests.Controllers;

public class BooksControllerTests
{
    private readonly Mock<IBooksService> _service;
    private readonly BooksController _controller;

    public BooksControllerTests()
    {
        _service = new Mock<IBooksService>();
        _controller = new BooksController(_service.Object);
    }

    //test1 -> obtinerea cartilor => ret Ok
    [Fact]
    public async Task GetBooks_ShouldReturnOk()
    {
        //arrange
        var books = new List<Book>
        {
            new Book
            {
                Id = 1,
                Title = "Book1",
                Author = "Author1",
                Category = "Crime",
                NoOfAvailableCopies = 3
            }
        };

        //pres ca serviciul returneaza books
        _service.Setup(x => x.GetAllBooksAsync()).ReturnsAsync(books);

        //act
        var result = await _controller.GetBooks();

        //assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(200, okResult.StatusCode);
    }

    //test2 -> GetBookById() ar trebui sa ret NotFound(id) daca nu gaseste cartea
    [Fact]
    public async Task GetBookById_ShouldReturnNotFound_WhenBookIsNotFound()
    {
        //arrange -> pres ca serviciul returneaza null 
        _service.Setup(x => x.GetBookByIdAsync(11111)).ReturnsAsync((Book?)null);

        //act
        var result = await _controller.GetBookById(11111);

        //assert
        Assert.IsType<NotFoundResult>(result);
    }

    //test3 -> GetBookById() ar trebui sa ret Ok(book) pt carte gasita
    [Fact]
    public async Task GetBookById_ShouldReturnOk_WhenBookFound()
    {
        //arrange
        var book = new Book
        {
            Id = 1,
            Title = "Book1",
            Author = "Author1",
            Category = "Crime",
            NoOfAvailableCopies = 3
        };

        _service.Setup(x => x.GetBookByIdAsync(1)).ReturnsAsync(book);

        //act
        var result = await _controller.GetBookById(1);

        //assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(200, okResult.StatusCode);
    }
}