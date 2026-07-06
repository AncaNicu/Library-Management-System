using LibraryExercise.Api.Models;
using LibraryExercise.Api.DTOs;

namespace LibraryExercise.Api.Repositories
{
    public interface IAuthRepository
    {
        Task<bool> EmailExists(string email);

        Task CreateUser(string name, string email, string hashedPassword);

        Task<User?> GetUserByEmail(string email);
    }
}