using System.ComponentModel.DataAnnotations;

namespace APIForge.Shared.DTOs;

public record RegisteredApiRequest(
    [Required] string Name,
    [Required] string Description,
    [Required] string RoutePrefix,
    [Required] string DownstreamBaseUrl,
    string Status = "Active");

public record RegisteredApiResponse(
    int Id,
    string Name,
    string Description,
    string RoutePrefix,
    string DownstreamBaseUrl,
    string Status,
    DateTime CreatedAtUtc,
    DateTime? UpdatedAtUtc);

public record ApiKeyCreateRequest([Required] string Name, int? RegisteredApiId, DateTime? ExpiresAtUtc);

public record ApiKeyCreateResponse(int Id, string Name, string PlainTextKey, string KeyPrefix, int? RegisteredApiId, DateTime? ExpiresAtUtc);

public record ApiKeyResponse(int Id, string Name, string KeyPrefix, int? RegisteredApiId, bool IsActive, DateTime CreatedAtUtc, DateTime? ExpiresAtUtc);

public record RequestLogResponse(
    long Id,
    int? RegisteredApiId,
    string? ApiName,
    int? ApiKeyId,
    string HttpMethod,
    string RequestPath,
    int StatusCode,
    long ResponseTimeMs,
    string? ClientIp,
    string? ErrorMessage,
    DateTime CreatedAtUtc);

public record PagedResponse<T>(IReadOnlyList<T> Items, int Page, int PageSize, long TotalCount);

public record DashboardSummaryResponse(
    long TotalRequests,
    long SuccessfulRequests,
    long FailedRequests,
    double AverageResponseTimeMs,
    IReadOnlyList<ApiUsageResponse> TopApis,
    IReadOnlyList<RecentFailureResponse> RecentFailures);

public record ApiUsageResponse(int RegisteredApiId, string ApiName, long RequestCount, double AverageResponseTimeMs);
public record RecentFailureResponse(long LogId, string RequestPath, int StatusCode, string? ErrorMessage, DateTime CreatedAtUtc);
