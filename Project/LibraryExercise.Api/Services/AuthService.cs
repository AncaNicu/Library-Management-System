using LibraryExercise.Api.Models;
using LibraryExercise.Api.DTOs;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using LibraryExercise.Api.Repositories;

namespace LibraryExercise.Api.Services
{
    public class AuthService : IAuthService
    {
        private readonly IAuthRepository _authRepository;
        private readonly IConfiguration _config;

        public AuthService(IAuthRepository authRepository, IConfiguration config)
        {
            _authRepository = authRepository;
            _config = config;
        }

        public void ValidateRegisterRequest(RegisterRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.Name))
                throw new Exception("Name is required");

            if (string.IsNullOrWhiteSpace(request.Email))
                throw new Exception("Email is required");

            if (string.IsNullOrWhiteSpace(request.Password))
                throw new Exception("Password is required");

            if (request.Password != request.ConfirmPassword)
                throw new Exception("Passwords do not match");
        }

        public void ValidateLoginRequest(LoginRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.Email))
                throw new Exception("Email is required");

            if (string.IsNullOrWhiteSpace(request.Password))
                throw new Exception("Password is required");
        }

        public async Task Register(RegisterRequest request)
        {
            ValidateRegisterRequest(request);

            bool emailExists = await _authRepository.EmailExists(request.Email);

            if (emailExists)
                throw new Exception("Email already exists");

            string hashedPassword = BCrypt.Net.BCrypt.HashPassword(request.Password);

            await _authRepository.CreateUser(
                request.Name,
                request.Email,
                hashedPassword
            );
        }

        public async Task<LoginResult> Login(LoginRequest request)
        {
            ValidateLoginRequest(request);

            var user = await _authRepository.GetUserByEmail(request.Email);

            if (user == null)
            {
                return new LoginResult
                {
                    Success = false,
                    Message = "User not found"
                };
            }

            bool isValid = BCrypt.Net.BCrypt.Verify(
                request.Password,
                user.HashedPassword
            );

            if (!isValid)
            {
                return new LoginResult
                {
                    Success = false,
                    Message = "Invalid email or password"
                };
            }

            var token = GenerateJwtToken(user.Id, user.Name);

            return new LoginResult
            {
                Success = true,
                Message = "Login successful",
                UserId = user.Id,
                UserName = user.Name,
                Token = token
            };
        }

        private string GenerateJwtToken(int userId, string name)
        {
            var jwtSettings = _config.GetSection("Jwt");
            var keyString = jwtSettings["Key"] ?? throw new Exception("JWT Key missing");
            var key = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(keyString));

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, userId.ToString()),
                new Claim(ClaimTypes.Name, name)
            };

            var token = new JwtSecurityToken(
                issuer: jwtSettings["Issuer"],
                audience: jwtSettings["Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddHours(2),
                signingCredentials: new SigningCredentials(key, SecurityAlgorithms.HmacSha256)
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

    }
}