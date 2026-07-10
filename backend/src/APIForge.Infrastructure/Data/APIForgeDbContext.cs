using APIForge.Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;

namespace APIForge.Infrastructure.Data;

public class APIForgeDbContext : DbContext
{
    public APIForgeDbContext(DbContextOptions<APIForgeDbContext> options) : base(options) { }

    public DbSet<User> Users => Set<User>();
    public DbSet<RegisteredApi> RegisteredApis => Set<RegisteredApi>();
    public DbSet<ApiKey> ApiKeys => Set<ApiKey>();
    public DbSet<GatewayRequestLog> GatewayRequestLogs => Set<GatewayRequestLog>();
    public DbSet<ApiUsageDailyStat> ApiUsageDailyStats => Set<ApiUsageDailyStat>();
    public DbSet<AuditLog> AuditLogs => Set<AuditLog>();
    public DbSet<Product> Products => Set<Product>();
    public DbSet<Customer> Customers => Set<Customer>();
    public DbSet<Order> Orders => Set<Order>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>()
            .HasIndex(x => x.Email)
            .IsUnique();

        modelBuilder.Entity<RegisteredApi>()
            .HasIndex(x => x.RoutePrefix)
            .IsUnique();

        modelBuilder.Entity<ApiKey>()
            .HasIndex(x => x.KeyHash)
            .IsUnique();

        modelBuilder.Entity<ApiKey>()
            .HasIndex(x => new { x.UserId, x.RegisteredApiId });

        modelBuilder.Entity<GatewayRequestLog>()
            .HasIndex(x => x.CreatedAtUtc);

        modelBuilder.Entity<GatewayRequestLog>()
            .HasIndex(x => new { x.RegisteredApiId, x.CreatedAtUtc });

        modelBuilder.Entity<GatewayRequestLog>()
            .HasIndex(x => new { x.StatusCode, x.CreatedAtUtc });

        modelBuilder.Entity<Product>()
            .Property(x => x.Price)
            .HasPrecision(18, 2);

        modelBuilder.Entity<Order>()
            .Property(x => x.TotalAmount)
            .HasPrecision(18, 2);
    }
}
