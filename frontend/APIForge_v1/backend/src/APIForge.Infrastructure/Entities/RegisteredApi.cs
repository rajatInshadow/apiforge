namespace APIForge.Infrastructure.Entities;

public class RegisteredApi
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string RoutePrefix { get; set; } = string.Empty;
    public string DownstreamBaseUrl { get; set; } = string.Empty;
    public string Status { get; set; } = "Active";
    public int CreatedByUserId { get; set; }
    public DateTime CreatedAtUtc { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAtUtc { get; set; }
    public ICollection<ApiKey> ApiKeys { get; set; } = new List<ApiKey>();
}
