using System.ComponentModel.DataAnnotations;

namespace APIForge.Shared.DTOs;

public record RegisterRequest(
    [Required, EmailAddress] string Email,
    [Required, MinLength(6)] string Password,
    [Required] string FullName,
    [Required] string Role);

public record LoginRequest(
    [Required, EmailAddress] string Email,
    [Required] string Password);

public record AuthResponse(
    int UserId,
    string Email,
    string FullName,
    string Role,
    string Token);

public record CurrentUserResponse(int UserId, string Email, string FullName, string Role);
