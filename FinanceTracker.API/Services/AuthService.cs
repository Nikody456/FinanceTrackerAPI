using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using FinanceTracker.API.Data;
using FinanceTracker.API.DTOs;
using FinanceTracker.API.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace FinanceTracker.API.Services;

public class AuthService(FinanceTrackerDbContext context, IConfiguration config) : IAuthService
{
    public async Task<string?> RegisterAsync(RegisterUserDto dto)
    {
        if (await context.Users.AnyAsync(u => u.Email == dto.Email))
            return null;

        var user = new User
        {
            Email = dto.Email,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password)
        };

        context.Users.Add(user);
        await context.SaveChangesAsync();

        return GenerateJwtToken(user);
    }

    public async Task<string?> LoginAsync(LoginUserDto dto)
    {
        var user = await context.Users.FirstOrDefaultAsync(u => u.Email == dto.Email);
        if (user == null) return null;
        
        Console.WriteLine($"Input password: {dto.Password}");
        Console.WriteLine($"Hash from db: {user.PasswordHash}");

        var isValid = BCrypt.Net.BCrypt.Verify(dto.Password, user.PasswordHash);
        Console.WriteLine($"Password valid: {isValid}");
        
        if (!isValid) return null;

        return GenerateJwtToken(user);
    }

    private string GenerateJwtToken(User user)
    {
        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Email, user.Email)
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["Jwt:Key"]!));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: config["Jwt:Issuer"],
            audience: config["Jwt:Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddHours(12),
            signingCredentials: creds);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
