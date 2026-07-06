using LibraryExercise.Api.Models;
using LibraryExercise.Api.DTOs;

namespace LibraryExercise.Api.Services
{
    public interface IAuthService
    {
        void ValidateRegisterRequest(RegisterRequest request);

        void ValidateLoginRequest(LoginRequest request);

        Task Register(RegisterRequest request);

        Task<LoginResult> Login(LoginRequest request);
    }
}