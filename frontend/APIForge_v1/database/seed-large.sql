USE APIForgeDb;
GO

/*
Large-data generator for performance practice.
Default target volumes:
- 5,000 users
- 200 registered APIs
- 10,000 API keys
- 50,000 products
- 25,000 customers
- 200,000 orders
- 500,000 gateway logs

Increase @TargetLogs to 1000000 after your machine handles 500k comfortably.
*/

SET NOCOUNT ON;

DECLARE @TargetUsers INT = 5000;
DECLARE @TargetApis INT = 200;
DECLARE @TargetKeys INT = 10000;
DECLARE @TargetProducts INT = 50000;
DECLARE @TargetCustomers INT = 25000;
DECLARE @TargetOrders INT = 200000;
DECLARE @TargetLogs INT = 500000;

DECLARE @i INT;

SET @i = 3;
WHILE @i <= @TargetUsers
BEGIN
    INSERT INTO Users (Email, PasswordHash, FullName, Role, IsActive, CreatedAtUtc)
    VALUES (
        CONCAT('user', @i, '@apiforge.local'),
        '100000.ZHVtbXktc2FsdC0xMjM0NQ==.dummy-hash-for-performance-seed-only',
        CONCAT('Performance User ', @i),
        CASE WHEN @i % 10 = 0 THEN 'Admin' ELSE 'Developer' END,
        1,
        DATEADD(DAY, -(@i % 365), SYSUTCDATETIME())
    );
    SET @i += 1;
END
GO

DECLARE @i INT = 3;
WHILE @i <= 200
BEGIN
    INSERT INTO RegisteredApis (Name, Description, RoutePrefix, DownstreamBaseUrl, Status, CreatedByUserId, CreatedAtUtc)
    VALUES (
        CONCAT('Partner API ', @i),
        CONCAT('Generated API for performance testing ', @i),
        CONCAT('partner-', @i),
        CASE WHEN @i % 2 = 0 THEN 'https://localhost:7301' ELSE 'https://localhost:7401' END,
        CASE WHEN @i % 20 = 0 THEN 'Inactive' ELSE 'Active' END,
        1,
        DATEADD(DAY, -(@i % 90), SYSUTCDATETIME())
    );
    SET @i += 1;
END
GO

DECLARE @i INT = 2;
WHILE @i <= 10000
BEGIN
    INSERT INTO ApiKeys (UserId, RegisteredApiId, Name, KeyPrefix, KeyHash, IsActive, CreatedAtUtc, ExpiresAtUtc)
    VALUES (
        2 + (@i % 4998),
        1 + (@i % 200),
        CONCAT('Generated Key ', @i),
        CONCAT('af_seed_', RIGHT(CONCAT('000000', @i), 6)),
        CONVERT(VARCHAR(64), HASHBYTES('SHA2_256', CONCAT('seed-key-', @i)), 2),
        CASE WHEN @i % 50 = 0 THEN 0 ELSE 1 END,
        DATEADD(DAY, -(@i % 180), SYSUTCDATETIME()),
        DATEADD(DAY, 365 - (@i % 180), SYSUTCDATETIME())
    );
    SET @i += 1;
END
GO

DECLARE @i INT = 101;
WHILE @i <= 50000
BEGIN
    INSERT INTO Products (Sku, Name, Price, StockQuantity, CreatedAtUtc)
    VALUES (
        CONCAT('SKU-', FORMAT(@i, '000000')),
        CONCAT('Performance Product ', @i),
        CAST(10 + (@i % 5000) AS DECIMAL(18,2)),
        1 + (@i % 1000),
        DATEADD(DAY, -(@i % 365), SYSUTCDATETIME())
    );
    SET @i += 1;
END
GO

DECLARE @i INT = 51;
WHILE @i <= 25000
BEGIN
    INSERT INTO Customers (FullName, Email, CreatedAtUtc)
    VALUES (
        CONCAT('Performance Customer ', @i),
        CONCAT('perf.customer', @i, '@example.com'),
        DATEADD(DAY, -(@i % 365), SYSUTCDATETIME())
    );
    SET @i += 1;
END
GO

DECLARE @i BIGINT = 201;
WHILE @i <= 200000
BEGIN
    INSERT INTO Orders (OrderNumber, CustomerId, TotalAmount, Status, CreatedAtUtc)
    VALUES (
        CONCAT('ORD-', FORMAT(@i, '000000000')),
        1 + (@i % 25000),
        CAST(100 + (@i % 10000) AS DECIMAL(18,2)),
        CASE WHEN @i % 13 = 0 THEN 'Cancelled' WHEN @i % 5 = 0 THEN 'Delivered' WHEN @i % 3 = 0 THEN 'Shipped' ELSE 'Created' END,
        DATEADD(MINUTE, -@i, SYSUTCDATETIME())
    );
    SET @i += 1;
END
GO

DECLARE @i BIGINT = 1001;
WHILE @i <= 500000
BEGIN
    INSERT INTO GatewayRequestLogs (RegisteredApiId, ApiKeyId, UserId, HttpMethod, RequestPath, StatusCode, ResponseTimeMs, ClientIp, ErrorMessage, CreatedAtUtc)
    VALUES (
        1 + (@i % 200),
        1 + (@i % 10000),
        1 + (@i % 5000),
        CASE WHEN @i % 12 = 0 THEN 'POST' WHEN @i % 7 = 0 THEN 'PUT' ELSE 'GET' END,
        CONCAT('/gateway/', CASE WHEN @i % 2 = 0 THEN 'products/api/products' ELSE 'orders/api/orders' END, '?page=', (@i % 100)),
        CASE WHEN @i % 100 = 0 THEN 500 WHEN @i % 37 = 0 THEN 429 WHEN @i % 23 = 0 THEN 404 ELSE 200 END,
        10 + (@i % 1500),
        CONCAT('10.0.', (@i % 255), '.', ((@i / 255) % 255)),
        CASE WHEN @i % 100 = 0 THEN 'Simulated internal server error' WHEN @i % 37 = 0 THEN 'Simulated rate limit' ELSE NULL END,
        DATEADD(SECOND, -@i, SYSUTCDATETIME())
    );
    SET @i += 1;
END
GO
