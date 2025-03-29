﻿using System.ComponentModel.DataAnnotations;

namespace FinanceTracker.API.DTOs;

public class LoginUserDto
{
    [Required]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;

    [Required]
    public string Password { get; set; } = string.Empty;
}