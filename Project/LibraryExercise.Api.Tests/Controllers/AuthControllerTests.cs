using LibraryExercise.Api.Controllers;
using LibraryExercise.Api.DTOs;
using LibraryExercise.Api.Services;

using Microsoft.AspNetCore.Mvc;

using Moq;

namespace LibraryExercise.Api.Tests.Controllers;

public class AuthControllerTests
{
    //mock la serviciu si referinta la controller
    private readonly Mock<IAuthService> _service;

    private readonly AuthController _controller;

    public AuthControllerTests()
    {
        _service = new Mock<IAuthService>();

        _controller = new AuthController(_service.Object);
    }

    //=========================REGISTER TESTS
    //test1 -> register realizat cu succes => ret OK()
    [Fact]
    public async Task Register_ShouldReturnOk_WhenRegisterSucceeds()
    {
        //arrange -> construieste cererea valida
        var request = new RegisterRequest
        {
            Name = "John",
            Email = "jogn@gmail.com",
            Password = "123",
            ConfirmPassword = "123"
        };

        //act -> apeleaza controller-ul
        var result = await _controller.Register(request);

        //arrange -> se asteapta ca rezultatul sa fie HTTP 200 OK
        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(200, okResult.StatusCode);
    }

    //test2 -> register esueaza si se genereaza BadRequest
    [Fact]
    public async Task Register_ShouldReturnBadRequest_WhenRegisterThrowsException()
    {
        //arrange -> pregateste datele
        var request = new RegisterRequest
        {
            Name = "John",
            Email = "jogn@gmail.com",
            Password = "123",
            ConfirmPassword = "123"
        };

        //pres ca la apelarea serviciului se genereaza eroare
        _service.Setup(x => x.Register(request)).ThrowsAsync(
            new Exception("Email already exists")
        );

        //act -> apeleaza controllerul
        var result = await _controller.Register(request);

        //assert -> se asteapta sa HTTP 400 BadRequest
        var badResult = Assert.IsType<BadRequestObjectResult>(result);
        Assert.Equal(400, badResult.StatusCode);
        Assert.Equal("Email already exists", badResult.Value);        
    }

    //=========================LOGIN TESTS
    //test3 -> login realizat cu succes => ret Ok()
    [Fact]
    public async Task Login_ShouldReturnOk_WhenLoginSucceeds()
    {
        //arrange -> pregateste datele
        var request = new LoginRequest
        {
            Email = "john@gmail.com",
            Password = "123"
        };

        //pres ca serviciul a trimis success = true
        _service.Setup(x => x.Login(request)).ReturnsAsync
        (
            new LoginResult
            {
                Success = true,
                Message = "Login successful",
                UserId = 1,
                UserName = "John",
                Token = "fake-token"               
            }
        );

        //act -> apeleaza controller.Login()
        var result = await _controller.Login(request);

        //assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(200, okResult.StatusCode);
    }

    //test4 -> login esueaza => ret Unauthorized()
    [Fact]
    public async Task Login_ShouldReturnUnauthorized_WhenLoginFails()
    {
        //arrange -> pregateste datele
        var request = new LoginRequest
        {
            Email = "john@gmail.com",
            Password = "123"
        };

        //pres ca serviciul a trimis success = false
        _service.Setup(x => x.Login(request)).ReturnsAsync
        (
            new LoginResult
            {
                Success = false,
                Message = "Invalid email or password",              
            }
        );

        //act -> apeleaza controller-ul
        var result = await _controller.Login(request);

        //assert -> ne asteptam la Unauthorized(Invalid email or password)
        var unauthorized = Assert.IsType<UnauthorizedObjectResult>(result);
        Assert.Equal(401, unauthorized.StatusCode);                
    }
}