namespace APIForge.Infrastructure.Entities;

public class ApiKey
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public int? RegisteredApiId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string KeyPrefix { get; set; } = string.Empty;
    public string KeyHash { get; set; } = string.Empty;
    public bool IsActive { get; set; } = true;
    public DateTime CreatedAtUtc { get; set; } = DateTime.UtcNow;
    public DateTime? ExpiresAtUtc { get; set; }
    public User? User { get; set; }
    public RegisteredApi? RegisteredApi { get; set; }
}
