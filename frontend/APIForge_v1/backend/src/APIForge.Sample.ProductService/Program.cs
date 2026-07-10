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

app.MapGet("/api/products", async (APIForgeDbContext db, int page = 1, int pageSize = 20, string? search = null) =>
{
    page = Math.Max(page, 1);
    pageSize = Math.Clamp(pageSize, 1, 100);
    var query = db.Products.AsNoTracking().AsQueryable();
    if (!string.IsNullOrWhiteSpace(search))
    {
        query = query.Where(x => x.Name.Contains(search) || x.Sku.Contains(search));
    }
    var items = await query.OrderBy(x => x.Id).Skip((page - 1) * pageSize).Take(pageSize)
        .Select(x => new ProductResponse(x.Id, x.Sku, x.Name, x.Price, x.StockQuantity))
        .ToListAsync();
    return Results.Ok(items);
});

app.MapGet("/api/products/{id:int}", async (APIForgeDbContext db, int id) =>
{
    var item = await db.Products.AsNoTracking()
        .Where(x => x.Id == id)
        .Select(x => new ProductResponse(x.Id, x.Sku, x.Name, x.Price, x.StockQuantity))
        .FirstOrDefaultAsync();
    return item is null ? Results.NotFound() : Results.Ok(item);
});

app.Run();
