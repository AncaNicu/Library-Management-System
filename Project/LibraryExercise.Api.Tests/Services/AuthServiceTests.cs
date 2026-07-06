using LibraryExercise.Api.DTOs;
using LibraryExercise.Api.Repositories;
using LibraryExercise.Api.Services;
using LibraryExercise.Api.Models;
using Microsoft.Extensions.Configuration;
using Moq;

namespace LibraryExercise.Api.Tests.Services;

public class AuthServiceTests
{
    //mock pt repository
    private readonly Mock<IAuthRepository> _repo;

    private readonly IConfiguration _config;
    //referinta la serviciul testat
    private readonly AuthService _service;

    //constructorul
    public AuthServiceTests()
    {
        //creeaza un repository fals
        _repo = new Mock<IAuthRepository>();

        //pt JWT, lucruri pe care le-ar lua in mod normal
        //din appsettings.json, dar la teste appsettings nu e accesat
        var settings = new Dictionary<string, string?>
        {
            {"Jwt:Key","abcdefghijklmnopqrstuvwxyz123456"},
            {"Jwt:Issuer","LibraryExercise.Api"},
            {"Jwt:Audience","LibraryClient"}
        };

        _config = new ConfigurationBuilder()
            .AddInMemoryCollection(settings)
            .Build();

        _service = new AuthService(
            _repo.Object,//obiectul fals 
            _config
        );
    }

    //===========================REGISTER TESTS

    //test1 => register esueaza daca deja exista email ul
    [Fact]
    public async Task Register_ShouldThrow_WhenEmailExists()
    {
        //arrange -> pregateste datele
        var request = new RegisterRequest
        {
            Name = "John",
            Email = "john@test.com",
            Password = "123",
            ConfirmPassword = "123"
        };

        _repo.Setup(x => x.EmailExists(request.Email)).ReturnsAsync(true);

        //act -> apeleaza fct ce trebuie apelata (aici Register())
        Func<Task> action = () => _service.Register(request);

        //assert -> verifica rezultatele
        //aici ar trebui sa se genereze o eroare, pt ca email-ul deja exista
        var exception = await Assert.ThrowsAsync<Exception>(action);
        Assert.Equal("Email already exists", exception.Message);
    }

    //test2 => register esueaza cand numele e empty/null
    [Fact]
    public async Task Register_ShouldThrow_WhenNameIsEmpty()
    {
        //arrange
        var request = new RegisterRequest
        {
            Name = "",//nume gol
            Email = "john@test.com",
            Password = "123",
            ConfirmPassword = "123"
        };  

        //act -> apeleaza Register()
        Func<Task> action = () => _service.Register(request);

        //assert -> se asteapta generarea unei exceptii
        var exception = await Assert.ThrowsAsync<Exception>(action);
        Assert.Equal("Name is required", exception.Message);      
    }

    //test3 => register esueaza cand email-ul e empty/null
    [Fact]
    public async Task Register_ShouldThrow_WhenEmailIsEmpty()
    {
        //arrange
        var request = new RegisterRequest
        {
            Name = "John",
            Email = "",//email gol
            Password = "123",
            ConfirmPassword = "123"
        };  

        //act -> apeleaza Register()
        Func<Task> action = () => _service.Register(request);

        //assert -> se asteapta generarea unei exceptii
        var exception = await Assert.ThrowsAsync<Exception>(action);
        Assert.Equal("Email is required", exception.Message);      
    }

    //test4 => register esueaza cand parola e empty/null
    [Fact]
    public async Task Register_ShouldThrow_WhenPasswordIsEmpty()
    {
        //arrange
        var request = new RegisterRequest
        {
            Name = "John",
            Email = "john@gmail.com",
            Password = "",//parola goala
            ConfirmPassword = ""
        };  

        //act -> apeleaza Register()
        Func<Task> action = () => _service.Register(request);

        //assert -> se asteapta generarea unei exceptii
        var exception = await Assert.ThrowsAsync<Exception>(action);
        Assert.Equal("Password is required", exception.Message);      
    }

    //test5 => register esueaza cand password != confirmPassword
    [Fact]
    public async Task Register_ShouldThrow_WhenPasswordsDoNotMatch()
    {
        //arrange
        var request = new RegisterRequest
        {
            Name = "John",
            Email = "john@gmail.com",
            Password = "123",
            ConfirmPassword = "123qq"
        };  

        //act -> apeleaza Register()
        Func<Task> action = () => _service.Register(request);

        //assert -> se asteapta generarea unei exceptii
        var exception = await Assert.ThrowsAsync<Exception>(action);
        Assert.Equal("Passwords do not match", exception.Message);      
    }

    //test6 => datele corecte => register ar trebui sa creeze user nou
    [Fact]
    public async Task Register_ShouldCreateUser_WhenDataIsValid()
    {
        //arrange
        var request = new RegisterRequest
        {
            Name = "John",
            Email = "john@gmail.com",
            Password = "123",
            ConfirmPassword = "123"
        };

        //pres ca email-ul nu exista deja
        _repo.Setup(x => x.EmailExists(request.Email)).ReturnsAsync(false);

        //act -> apeleaza Register()
        await _service.Register(request);

        //assert -> verif daca a fost creat user nou
        _repo.Verify(x => x.CreateUser(
            request.Name,
            request.Email,
            It.IsAny<string>()//pt ca nu apelam BCrypt, deci orice parola e ok
        ), Times.Once);
    }

    //===========================LOGIN TESTS

    //test7 => login esueaza daca user-ul nu exista
    [Fact]
    public async Task Login_ShouldReturnFailure_WhenUserDoesNotExist()
    {
        //arrange
        var request = new LoginRequest
        {
            Email = "john@gmail.com",
            Password = "123"
        };

        _repo.Setup(x => x.GetUserByEmail(request.Email)).ReturnsAsync((User?)null);

        //act
        var result = await _service.Login(request);

        //assert
        Assert.False(result.Success);
        Assert.Equal("User not found", result.Message);
    }

    //test8 => login esueaza daca parola e gresita
    [Fact]
    public async Task Login_ShouldReturnFailure_WhenPasswordIsWrong()
    {
        //arrange
        var request = new LoginRequest
        {
            Email = "john@gmail.com",
            Password = "wrong"
        };

        var user = new User
        {
            Id = 1,
            Name = "John",
            Email = "john@gmail.com",
            HashedPassword = BCrypt.Net.BCrypt.HashPassword("correct")
        };

        _repo.Setup(x => x.GetUserByEmail(request.Email)).ReturnsAsync(user);

        //act
        var result = await _service.Login(request);

        //assert
        Assert.False(result.Success);
        Assert.Equal("Invalid email or password", result.Message);
    }

    //test9 => date corecte => utilizator logat
    [Fact]
    public async Task Login_ShouldReturnSuccess_WhenCredentialsAreCorrect()
    {
        //arrange
        var request = new LoginRequest
        {
            Email = "john@gmail.com",
            Password = "123"
        };

        var user = new User
        {
            Id = 1,
            Name = "John",
            Email = "john@gmail.com",
            HashedPassword = BCrypt.Net.BCrypt.HashPassword("123")
        };

        _repo.Setup(x => x.GetUserByEmail(request.Email)).ReturnsAsync(user); 

        //act
        var result = await _service.Login(request);

        //assert
        Assert.True(result.Success); 
        Assert.Equal("Login successful", result.Message);
        Assert.Equal(user.Id, result.UserId);   
        Assert.Equal(user.Name, result.UserName);  
        Assert.False(string.IsNullOrEmpty(result.Token));            
    }
}