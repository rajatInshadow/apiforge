using APIForge.Infrastructure.Data;
using APIForge.Shared.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace APIForge.PortalApi.Controllers;

[ApiController]
[Authorize]
[Route("api/analytics")]
public class AnalyticsController : ControllerBase
{
    private readonly APIForgeDbContext _db;

    public AnalyticsController(APIForgeDbContext db)
    {
        _db = db;
    }

    [HttpGet("summary")]
    public async Task<ActionResult<DashboardSummaryResponse>> GetSummary()
    {
        var total = await _db.GatewayRequestLogs.AsNoTracking().LongCountAsync();
        var success = await _db.GatewayRequestLogs.AsNoTracking().LongCountAsync(x => x.StatusCode >= 200 && x.StatusCode < 400);
        var failed = await _db.GatewayRequestLogs.AsNoTracking().LongCountAsync(x => x.StatusCode >= 400);
        var average = total == 0 ? 0 : await _db.GatewayRequestLogs.AsNoTracking().AverageAsync(x => (double)x.ResponseTimeMs);

        var topApis = await _db.GatewayRequestLogs.AsNoTracking()
            .Where(x => x.RegisteredApiId != null)
            .GroupBy(x => new { x.RegisteredApiId, x.RegisteredApi!.Name })
            .OrderByDescending(g => g.Count())
            .Take(5)
            .Select(g => new ApiUsageResponse(g.Key.RegisteredApiId!.Value, g.Key.Name, g.LongCount(), g.Average(x => (double)x.ResponseTimeMs)))
            .ToListAsync();

        var recentFailures = await _db.GatewayRequestLogs.AsNoTracking()
            .Where(x => x.StatusCode >= 400)
            .OrderByDescending(x => x.CreatedAtUtc)
            .Take(5)
            .Select(x => new RecentFailureResponse(x.Id, x.RequestPath, x.StatusCode, x.ErrorMessage, x.CreatedAtUtc))
            .ToListAsync();

        return Ok(new DashboardSummaryResponse(total, success, failed, Math.Round(average, 2), topApis, recentFailures));
    }
}
