using System.Diagnostics;
using APIForge.Infrastructure.Data;
using APIForge.Infrastructure.Entities;
using APIForge.Shared.Security;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace APIForge.Gateway.Controllers;

[ApiController]
[Route("gateway/{routePrefix}/{**remainingPath}")]
public class GatewayController : ControllerBase
{
    private readonly APIForgeDbContext _db;
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly ILogger<GatewayController> _logger;

    public GatewayController(APIForgeDbContext db, IHttpClientFactory httpClientFactory, ILogger<GatewayController> logger)
    {
        _db = db;
        _httpClientFactory = httpClientFactory;
        _logger = logger;
    }

    [HttpGet, HttpPost, HttpPut, HttpPatch, HttpDelete]
    public async Task<IActionResult> Forward(string routePrefix, string? remainingPath)
    {
        var stopwatch = Stopwatch.StartNew();
        int statusCode = 500;
        string? error = null;
        RegisteredApi? registeredApi = null;
        ApiKey? apiKey = null;

        try
        {
            var rawApiKey = Request.Headers["x-api-key"].FirstOrDefault();
            if (string.IsNullOrWhiteSpace(rawApiKey))
            {
                statusCode = StatusCodes.Status401Unauthorized;
                return Unauthorized("Missing x-api-key header.");
            }

            var keyHash = ApiKeyHasher.Hash(rawApiKey);
            apiKey = await _db.ApiKeys.AsNoTracking()
                .FirstOrDefaultAsync(x => x.KeyHash == keyHash && x.IsActive);

            if (apiKey is null || (apiKey.ExpiresAtUtc.HasValue && apiKey.ExpiresAtUtc < DateTime.UtcNow))
            {
                statusCode = StatusCodes.Status401Unauthorized;
                return Unauthorized("Invalid or expired API key.");
            }

            registeredApi = await _db.RegisteredApis.AsNoTracking()
                .FirstOrDefaultAsync(x => x.RoutePrefix == routePrefix.ToLowerInvariant() && x.Status == "Active");

            if (registeredApi is null)
            {
                statusCode = StatusCodes.Status404NotFound;
                return NotFound("No active API route found for this prefix.");
            }

            if (apiKey.RegisteredApiId.HasValue && apiKey.RegisteredApiId.Value != registeredApi.Id)
            {
                statusCode = StatusCodes.Status403Forbidden;
                return Forbid();
            }

            var targetUri = BuildTargetUri(registeredApi.DownstreamBaseUrl, remainingPath, Request.QueryString.Value);
            using var forwardRequest = await CreateForwardRequest(targetUri);
            var client = _httpClientFactory.CreateClient("gateway-forwarder");
            using var response = await client.SendAsync(forwardRequest, HttpCompletionOption.ResponseHeadersRead, HttpContext.RequestAborted);

            statusCode = (int)response.StatusCode;
            Response.StatusCode = statusCode;
            var contentBytes = await response.Content.ReadAsByteArrayAsync(HttpContext.RequestAborted);
            var contentType = response.Content.Headers.ContentType?.ToString() ?? "application/json";

            return File(contentBytes, contentType, enableRangeProcessing: false);
        }
        catch (Exception ex)
        {
            error = ex.Message;
            _logger.LogError(ex, "Gateway forwarding failed");
            statusCode = StatusCodes.Status502BadGateway;
            return StatusCode(statusCode, "Gateway forwarding failed.");
        }
        finally
        {
            stopwatch.Stop();
            await LogRequest(registeredApi?.Id, apiKey?.Id, apiKey?.UserId, statusCode, stopwatch.ElapsedMilliseconds, error);
        }
    }

    private async Task<HttpRequestMessage> CreateForwardRequest(string targetUri)
    {
        var request = new HttpRequestMessage(new HttpMethod(Request.Method), targetUri);

        if (Request.ContentLength > 0)
        {
            request.Content = new StreamContent(Request.Body);
            if (!string.IsNullOrWhiteSpace(Request.ContentType))
                request.Content.Headers.TryAddWithoutValidation("Content-Type", Request.ContentType);
        }

        foreach (var header in Request.Headers)
        {
            if (header.Key.Equals("Host", StringComparison.OrdinalIgnoreCase) ||
                header.Key.Equals("x-api-key", StringComparison.OrdinalIgnoreCase))
                continue;

            request.Headers.TryAddWithoutValidation(header.Key, header.Value.ToArray());
        }

        return await Task.FromResult(request);
    }

    private static string BuildTargetUri(string baseUrl, string? remainingPath, string? queryString)
    {
        var path = string.IsNullOrWhiteSpace(remainingPath) ? string.Empty : "/" + remainingPath.TrimStart('/');
        return baseUrl.TrimEnd('/') + path + queryString;
    }

    private async Task LogRequest(int? apiId, int? apiKeyId, int? userId, int statusCode, long responseTimeMs, string? error)
    {
        _db.GatewayRequestLogs.Add(new GatewayRequestLog
        {
            RegisteredApiId = apiId,
            ApiKeyId = apiKeyId,
            UserId = userId,
            HttpMethod = Request.Method,
            RequestPath = Request.Path + Request.QueryString,
            StatusCode = statusCode,
            ResponseTimeMs = responseTimeMs,
            ClientIp = HttpContext.Connection.RemoteIpAddress?.ToString(),
            ErrorMessage = error,
            CreatedAtUtc = DateTime.UtcNow
        });
        await _db.SaveChangesAsync();
    }
}
