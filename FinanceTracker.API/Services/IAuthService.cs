using FinanceTracker.API.DTOs;

namespace FinanceTracker.API.Services;

public interface IAuthService
{
    Task<string?> RegisterAsync(RegisterUserDto dto);
    Task<string?> LoginAsync(LoginUserDto dto);
}