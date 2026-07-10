using System.Security.Claims;
using APIForge.Infrastructure.Data;
using APIForge.Infrastructure.Entities;
using APIForge.PortalApi.Services;
using APIForge.Shared.Constants;
using APIForge.Shared.DTOs;
using APIForge.Shared.Security;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace APIForge.PortalApi.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    private readonly APIForgeDbContext _db;
    private readonly JwtTokenService _jwtTokenService;

    public AuthController(APIForgeDbContext db, JwtTokenService jwtTokenService)
    {
        _db = db;
        _jwtTokenService = jwtTokenService;
    }

    [HttpPost("register")]
    public async Task<ActionResult<AuthResponse>> Register(RegisterRequest request)
    {
        var role = request.Role == AppRoles.Admin ? AppRoles.Admin : AppRoles.Developer;
        var exists = await _db.Users.AnyAsync(x => x.Email == request.Email);
        if (exists) return Conflict("Email already exists.");

        var user = new User
        {
            Email = request.Email.Trim().ToLowerInvariant(),
            FullName = request.FullName.Trim(),
            Role = role,
            PasswordHash = PasswordHasher.Hash(request.Password),
            CreatedAtUtc = DateTime.UtcNow
        };

        _db.Users.Add(user);
        await _db.SaveChangesAsync();
        var token = _jwtTokenService.GenerateToken(user);
        return Ok(new AuthResponse(user.Id, user.Email, user.FullName, user.Role, token));
    }

    [HttpPost("login")]
    public async Task<ActionResult<AuthResponse>> Login(LoginRequest request)
    {
        var email = request.Email.Trim().ToLowerInvariant();
        var user = await _db.Users.FirstOrDefaultAsync(x => x.Email == email && x.IsActive);
        if (user is null || !PasswordHasher.Verify(request.Password, user.PasswordHash))
            return Unauthorized("Invalid email or password.");

        var token = _jwtTokenService.GenerateToken(user);
        return Ok(new AuthResponse(user.Id, user.Email, user.FullName, user.Role, token));
    }

    [Authorize]
    [HttpGet("me")]
    public ActionResult<CurrentUserResponse> Me()
    {
        var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var email = User.FindFirstValue(ClaimTypes.Email)!;
        var fullName = User.FindFirstValue(ClaimTypes.Name)!;
        var role = User.FindFirstValue(ClaimTypes.Role)!;
        return Ok(new CurrentUserResponse(userId, email, fullName, role));
    }
}
