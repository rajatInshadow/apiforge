namespace APIForge.Infrastructure.Entities;

public class GatewayRequestLog
{
    public long Id { get; set; }
    public int? RegisteredApiId { get; set; }
    public int? ApiKeyId { get; set; }
    public int? UserId { get; set; }
    public string HttpMethod { get; set; } = string.Empty;
    public string RequestPath { get; set; } = string.Empty;
    public int StatusCode { get; set; }
    public long ResponseTimeMs { get; set; }
    public string? ClientIp { get; set; }
    public string? ErrorMessage { get; set; }
    public DateTime CreatedAtUtc { get; set; } = DateTime.UtcNow;
    public RegisteredApi? RegisteredApi { get; set; }
    public ApiKey? ApiKey { get; set; }
}
