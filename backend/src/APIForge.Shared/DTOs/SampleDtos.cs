namespace APIForge.Shared.DTOs;

public record ProductResponse(int Id, string Sku, string Name, decimal Price, int StockQuantity);
public record OrderResponse(long Id, string OrderNumber, string CustomerName, decimal TotalAmount, string Status, DateTime CreatedAtUtc);
