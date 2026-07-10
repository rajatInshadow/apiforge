using APIForge.Infrastructure.Data;
using APIForge.Shared.DTOs;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<APIForgeDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

var app = builder.Build();
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseHttpsRedirection();

app.MapGet("/api/orders", async (APIForgeDbContext db, int page = 1, int pageSize = 20, string? status = null) =>
{
    page = Math.Max(page, 1);
    pageSize = Math.Clamp(pageSize, 1, 100);
    var query = db.Orders.AsNoTracking().Include(x => x.Customer).AsQueryable();
    if (!string.IsNullOrWhiteSpace(status)) query = query.Where(x => x.Status == status);
    var items = await query.OrderByDescending(x => x.CreatedAtUtc).Skip((page - 1) * pageSize).Take(pageSize)
        .Select(x => new OrderResponse(x.Id, x.OrderNumber, x.Customer!.FullName, x.TotalAmount, x.Status, x.CreatedAtUtc))
        .ToListAsync();
    return Results.Ok(items);
});

app.MapGet("/api/orders/{id:long}", async (APIForgeDbContext db, long id) =>
{
    var item = await db.Orders.AsNoTracking().Include(x => x.Customer)
        .Where(x => x.Id == id)
        .Select(x => new OrderResponse(x.Id, x.OrderNumber, x.Customer!.FullName, x.TotalAmount, x.Status, x.CreatedAtUtc))
        .FirstOrDefaultAsync();
    return item is null ? Results.NotFound() : Results.Ok(item);
});

app.Run();
