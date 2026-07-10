using System.Security.Claims;
using APIForge.Infrastructure.Data;
using APIForge.Infrastructure.Entities;
using APIForge.Shared.DTOs;
using APIForge.Shared.Security;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace APIForge.PortalApi.Controllers;

[ApiController]
[Authorize]
[Route("api/api-keys")]
public class ApiKeysController : ControllerBase
{
    private readonly APIForgeDbContext _db;

    public ApiKeysController(APIForgeDbContext db)
    {
        _db = db;
    }

    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<ApiKeyResponse>>> GetMyKeys()
    {
        var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var keys = await _db.ApiKeys.AsNoTracking()
            .Where(x => x.UserId == userId)
            .OrderByDescending(x => x.CreatedAtUtc)
            .Select(x => new ApiKeyResponse(x.Id, x.Name, x.KeyPrefix, x.RegisteredApiId, x.IsActive, x.CreatedAtUtc, x.ExpiresAtUtc))
            .ToListAsync();
        return Ok(keys);
    }

    [HttpPost]
    public async Task<ActionResult<ApiKeyCreateResponse>> Create(ApiKeyCreateRequest request)
    {
        var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        if (request.RegisteredApiId.HasValue)
        {
            var apiExists = await _db.RegisteredApis.AnyAsync(x => x.Id == request.RegisteredApiId.Value && x.Status == "Active");
            if (!apiExists) return BadRequest("Registered API is invalid or inactive.");
        }

        var plainKey = ApiKeyHasher.GeneratePlainTextKey();
        var apiKey = new ApiKey
        {
            UserId = userId,
            RegisteredApiId = request.RegisteredApiId,
            Name = request.Name.Trim(),
            KeyPrefix = ApiKeyHasher.Prefix(plainKey),
            KeyHash = ApiKeyHasher.Hash(plainKey),
            CreatedAtUtc = DateTime.UtcNow,
            ExpiresAtUtc = request.ExpiresAtUtc
        };

        _db.ApiKeys.Add(apiKey);
        await _db.SaveChangesAsync();

        return Ok(new ApiKeyCreateResponse(apiKey.Id, apiKey.Name, plainKey, apiKey.KeyPrefix, apiKey.RegisteredApiId, apiKey.ExpiresAtUtc));
    }

    [HttpPatch("{id:int}/deactivate")]
    public async Task<IActionResult> Deactivate(int id)
    {
        var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var key = await _db.ApiKeys.FirstOrDefaultAsync(x => x.Id == id && x.UserId == userId);
        if (key is null) return NotFound();

        key.IsActive = false;
        await _db.SaveChangesAsync();
        return NoContent();
    }
}
