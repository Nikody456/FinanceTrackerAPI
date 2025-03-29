using FinanceTracker.API.DTOs;
using FinanceTracker.API.Services;
using Microsoft.AspNetCore.Mvc;

namespace FinanceTracker.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController(IAuthService service) : ControllerBase
{
    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterUserDto dto)
    {
        var token = await service.RegisterAsync(dto);
        if (token == null) return BadRequest("Пользователь с таким email уже есть");

        return Ok(new { token });
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginUserDto dto)
    {
        var token = await service.LoginAsync(dto);
        if (token == null) return Unauthorized("Неверный логин или пароль");

        return Ok(new { token });
    }
}