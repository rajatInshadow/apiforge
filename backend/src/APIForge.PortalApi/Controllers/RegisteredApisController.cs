using System.Security.Claims;
using APIForge.Infrastructure.Data;
using APIForge.Infrastructure.Entities;
using APIForge.Shared.Constants;
using APIForge.Shared.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace APIForge.PortalApi.Controllers;

[ApiController]
[Authorize]
[Route("api/apis")]
public class RegisteredApisController : ControllerBase
{
    private readonly APIForgeDbContext _db;

    public RegisteredApisController(APIForgeDbContext db)
    {
        _db = db;
    }

    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<RegisteredApiResponse>>> GetAll()
    {
        var apis = await _db.RegisteredApis.AsNoTracking()
            .OrderBy(x => x.Name)
            .Select(x => new RegisteredApiResponse(x.Id, x.Name, x.Description, x.RoutePrefix, x.DownstreamBaseUrl, x.Status, x.CreatedAtUtc, x.UpdatedAtUtc))
            .ToListAsync();
        return Ok(apis);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<RegisteredApiResponse>> GetById(int id)
    {
        var api = await _db.RegisteredApis.AsNoTracking()
            .Where(x => x.Id == id)
            .Select(x => new RegisteredApiResponse(x.Id, x.Name, x.Description, x.RoutePrefix, x.DownstreamBaseUrl, x.Status, x.CreatedAtUtc, x.UpdatedAtUtc))
            .FirstOrDefaultAsync();

        return api is null ? NotFound() : Ok(api);
    }

    [Authorize(Roles = AppRoles.Admin)]
    [HttpPost]
    public async Task<ActionResult<RegisteredApiResponse>> Create(RegisteredApiRequest request)
    {
        var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var routePrefix = NormalizeRoutePrefix(request.RoutePrefix);

        var exists = await _db.RegisteredApis.AnyAsync(x => x.RoutePrefix == routePrefix);
        if (exists) return Conflict("Route prefix already exists.");

        var api = new RegisteredApi
        {
            Name = request.Name.Trim(),
            Description = request.Description.Trim(),
            RoutePrefix = routePrefix,
            DownstreamBaseUrl = request.DownstreamBaseUrl.Trim().TrimEnd('/'),
            Status = request.Status == ApiStatus.Inactive ? ApiStatus.Inactive : ApiStatus.Active,
            CreatedByUserId = userId,
            CreatedAtUtc = DateTime.UtcNow
        };

        _db.RegisteredApis.Add(api);
        await _db.SaveChangesAsync();

        return CreatedAtAction(nameof(GetById), new { id = api.Id }, new RegisteredApiResponse(api.Id, api.Name, api.Description, api.RoutePrefix, api.DownstreamBaseUrl, api.Status, api.CreatedAtUtc, api.UpdatedAtUtc));
    }

    [Authorize(Roles = AppRoles.Admin)]
    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, RegisteredApiRequest request)
    {
        var api = await _db.RegisteredApis.FindAsync(id);
        if (api is null) return NotFound();

        api.Name = request.Name.Trim();
        api.Description = request.Description.Trim();
        api.RoutePrefix = NormalizeRoutePrefix(request.RoutePrefix);
        api.DownstreamBaseUrl = request.DownstreamBaseUrl.Trim().TrimEnd('/');
        api.Status = request.Status == ApiStatus.Inactive ? ApiStatus.Inactive : ApiStatus.Active;
        api.UpdatedAtUtc = DateTime.UtcNow;

        await _db.SaveChangesAsync();
        return NoContent();
    }

    private static string NormalizeRoutePrefix(string routePrefix)
    {
        var value = routePrefix.Trim().Trim('/');
        return value.ToLowerInvariant();
    }
}
