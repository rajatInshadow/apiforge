namespace APIForge.Infrastructure.Entities;

public class ApiUsageDailyStat
{
    public long Id { get; set; }
    public int RegisteredApiId { get; set; }
    public DateOnly UsageDate { get; set; }
    public long TotalRequests { get; set; }
    public long FailedRequests { get; set; }
    public double AverageResponseTimeMs { get; set; }
    public RegisteredApi? RegisteredApi { get; set; }
}
