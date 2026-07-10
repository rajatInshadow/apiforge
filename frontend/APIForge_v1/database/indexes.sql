USE APIForgeDb;
GO

CREATE INDEX IX_GatewayRequestLogs_CreatedAtUtc
ON GatewayRequestLogs (CreatedAtUtc DESC)
INCLUDE (RegisteredApiId, ApiKeyId, StatusCode, ResponseTimeMs);
GO

CREATE INDEX IX_GatewayRequestLogs_ApiId_CreatedAtUtc
ON GatewayRequestLogs (RegisteredApiId, CreatedAtUtc DESC)
INCLUDE (StatusCode, ResponseTimeMs, RequestPath);
GO

CREATE INDEX IX_GatewayRequestLogs_StatusCode_CreatedAtUtc
ON GatewayRequestLogs (StatusCode, CreatedAtUtc DESC)
INCLUDE (RegisteredApiId, ResponseTimeMs, RequestPath);
GO

CREATE INDEX IX_ApiKeys_UserId_RegisteredApiId
ON ApiKeys (UserId, RegisteredApiId)
INCLUDE (IsActive, CreatedAtUtc, ExpiresAtUtc);
GO

CREATE INDEX IX_Orders_CreatedAtUtc
ON Orders (CreatedAtUtc DESC)
INCLUDE (CustomerId, TotalAmount, Status);
GO

CREATE INDEX IX_Products_Name_Sku
ON Products (Name, Sku)
INCLUDE (Price, StockQuantity);
GO
