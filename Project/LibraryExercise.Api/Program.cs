using System.Text;
using LibraryExercise.Api.Services;
using LibraryExercise.Api.Repositories;
using LibraryExercise.Api.Data;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSingleton<SqlConnectionFactory>();

builder.Services.AddScoped<IBooksService, BooksService>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IBorrowService, BorrowService>();

builder.Services.AddScoped<IBooksRepository, BooksRepository>();
builder.Services.AddScoped<IAuthRepository, AuthRepository>();
builder.Services.AddScoped<IBorrowRepository, BorrowRepository>();

var jwtSettings = builder.Configuration.GetSection("Jwt");
var keyString = jwtSettings["Key"] ?? throw new Exception("JWT Key missing");
var key = Encoding.ASCII.GetBytes(keyString);

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateIssuerSigningKey = true,
        ValidateLifetime = true,
        ValidIssuer = jwtSettings["Issuer"],
        ValidAudience = jwtSettings["Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(key)
    };
});

// CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngular",
        policy =>
        {
            policy.AllowAnyOrigin()
                  .AllowAnyHeader()
                  .AllowAnyMethod();
        });
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();

    app.UseSwaggerUI();
}

app.UseCors("AllowAngular");
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
