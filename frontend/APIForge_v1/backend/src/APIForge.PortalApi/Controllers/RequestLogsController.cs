using APIForge.Infrastructure.Data;
using APIForge.Shared.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace APIForge.PortalApi.Controllers;

[ApiController]
[Authorize]
[Route("api/request-logs")]
public class RequestLogsController : ControllerBase
{
    private readonly APIForgeDbContext _db;

    public RequestLogsController(APIForgeDbContext db)
    {
        _db = db;
    }

    [HttpGet]
    public async Task<ActionResult<PagedResponse<RequestLogResponse>>> GetLogs(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 50,
        [FromQuery] int? apiId = null,
        [FromQuery] int? statusCode = null)
    {
        page = Math.Max(page, 1);
        pageSize = Math.Clamp(pageSize, 1, 200);

        var query = _db.GatewayRequestLogs.AsNoTracking().AsQueryable();
        if (apiId.HasValue) query = query.Where(x => x.RegisteredApiId == apiId.Value);
        if (statusCode.HasValue) query = query.Where(x => x.StatusCode == statusCode.Value);

        var total = await query.CountAsync();
        var items = await query
            .OrderByDescending(x => x.CreatedAtUtc)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(x => new RequestLogResponse(
                x.Id,
                x.RegisteredApiId,
                x.RegisteredApi != null ? x.RegisteredApi.Name : null,
                x.ApiKeyId,
                x.HttpMethod,
                x.RequestPath,
                x.StatusCode,
                x.ResponseTimeMs,
                x.ClientIp,
                x.ErrorMessage,
                x.CreatedAtUtc))
            .ToListAsync();

        return Ok(new PagedResponse<RequestLogResponse>(items, page, pageSize, total));
    }
}
