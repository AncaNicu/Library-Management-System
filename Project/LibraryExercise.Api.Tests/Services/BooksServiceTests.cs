using LibraryExercise.Api.Models;
using LibraryExercise.Api.Repositories;
using LibraryExercise.Api.Services;

using Moq;

namespace LibraryExercise.Api.Tests.Services;

public class BooksServiceTests
{
    //mock pt repository
    private readonly Mock<IBooksRepository> _repo;

    private readonly BooksService _service;

    public BooksServiceTests()
    {
        _repo = new Mock<IBooksRepository>();

        _service = new BooksService(
            _repo.Object
        );
    }

    //test1 => verif daca returneaza toate cartile 
    [Fact] 
    public async Task GetAllBooks_ShouldReturnAllBooks()
    {
        //arrange -> creeaza o lista de carti
        var books = new List<Book>
        {
            new Book
            {
                Id = 1,
                Title = "Book1",
                Author = "Author1",
                Category = "Crime",
                NoOfAvailableCopies = 2
            },
            new Book
            {
                Id = 2,
                Title = "Book2",
                Author = "Author2",
                Category = "Thriller",
                NoOfAvailableCopies = 5
            }
        };
        //pres ca repository-ul retrneaza books
        _repo.Setup(x => x.GetAllBooksAsync()).ReturnsAsync(books);

        //act -> apeleaza serviciul.GetAllBooksAsync()
        var result = await _service.GetAllBooksAsync();

        //assert
        //se asteapta returnarea a 2 carti
        Assert.Equal(2, result.Count);

        //se asteapta cele 2 carti (Book1, Book2)
        Assert.Equal("Book1", result[0].Title);
        Assert.Equal("Book2", result[1].Title);
    }

    //test2 => verif daca returneaza cartea cu un anumit id
    [Fact]
    public async Task GetBookById_ShouldReturnBook()
    {
        //assert -> pres ca avem o carte 
        var book = new Book
        {
            Id = 1,
            Title = "Book1",
            Author = "Author1",
            Category = "Crime",
            NoOfAvailableCopies = 23
        };

        _repo.Setup(x => x.GetBookByIdAsync(1)).ReturnsAsync(book);

        //act -> apeleaza serviciul.GetBookByIdAsync(1)
        var result = await _service.GetBookByIdAsync(1);

        //assert
        Assert.NotNull(result);
        Assert.Equal(book.Id, result!.Id);
        Assert.Equal(book.Title, result.Title);
        Assert.Equal(book.Author, result.Author);
    }  
}