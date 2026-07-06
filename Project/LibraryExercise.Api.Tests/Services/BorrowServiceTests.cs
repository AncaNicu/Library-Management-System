using LibraryExercise.Api.DTOs;
using LibraryExercise.Api.Repositories;
using LibraryExercise.Api.Services;
using Moq;

namespace LibraryExercise.Api.Tests.Services;

public class BorrowServiceTests
{
    private readonly Mock<IBorrowRepository> _repo;

    private readonly BorrowService _service;

    public BorrowServiceTests()
    {
        _repo = new Mock<IBorrowRepository>();

        _service = new BorrowService(_repo.Object);
    }

    //=========================BORROW TESTS
    //test1 -> nu sunt suficiente exemplare => borrow esueaza
    [Fact]
    public async Task BorrowBook_ShouldThrow_WhenNoCopiesAvailable()
    {
        //arrange ->pres ca repository ret 0 ca nr de exemplare
        _repo.Setup(x => x.GetNoOfAvailableCopies(1)).ReturnsAsync(0);

        //act -> apeleaza serviciul
        Func<Task> action = () => _service.BorrowBook(1, 1);

        //assert -> se asteapta la o exceptie
        var exception = await Assert.ThrowsAsync<Exception>(action);
        Assert.Equal("Book is not available", exception.Message);     
    }

    //test2 -> borrow realizata cu succes
    [Fact]
    public async Task BorrowBook_ShouldBorrowBook_WhenCopiesExist()
    {
        //arrange -> pres ca sunt suficiente exemplare
        _repo.Setup(x => x.GetNoOfAvailableCopies(1)).ReturnsAsync(5);

        //act -> apeleaza serviciul.Borrow()
        await _service.BorrowBook(1, 1);

        //assert -> se asteapta sa se efectueze imprumutul cu succes
        //adica sa fie apelat repository.BorrowBook() o data
        _repo.Verify(x => x.BorrowBook(1, 1), Times.Once);
    }

    //=========================RETURN TESTS
    //test3 -> return efectuata cu succes
    [Fact]
    public async Task ReturnBook_ShouldCallRepository()
    {
        //assert -> nu e cazul aici
        //act -> apeleaza serviciul.Return()
        await _service.ReturnBook(1);

        //assert -> verif daca a fost apelat repository.Return
        _repo.Verify(x => x.ReturnBook(1), Times.Once); 
    }

    //=========================GET ALL BORROWED BOOKS TESTS
    //test4 -> verif daca se returneaza toate cartile imprumutate de un utilizator
    [Fact]
    public async Task GetBorrowedBooks_ShouldReturnBooks()
    {
        //arrange -> creeaza o lista de carti imprumutate de user
        var books = new List<BorrowedBook>
        {
            new BorrowedBook
            {
                BorrowId = 1,
                BookTitle = "Book1",
                BookAuthor = "Author1",
                BorrowDate = DateTime.Now
            },
            new BorrowedBook
            {
                BorrowId = 2,
                BookTitle = "Book2",
                BookAuthor = "Author2",
                BorrowDate = DateTime.Now
            }
        };

        //pres ca apeland repository, se ret cele 2 carti
        _repo.Setup(x => x.GetAllBorrowedBooksByUser(1)).ReturnsAsync(books);

        //act -> apeleaza serviciul
        var result = await _service.GetAllBorrowedBooksByUser(1);

        //assert -> sunt 2 carti imprumutate si titlurile sunt Book1 si Book2
        Assert.Equal(2, result.Count);
        Assert.Equal(result[0].BookTitle, books[0].BookTitle);
        Assert.Equal(result[1].BookTitle, books[1].BookTitle);
    }
}