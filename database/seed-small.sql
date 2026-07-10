USE APIForgeDb;
GO

INSERT INTO Users (Email, PasswordHash, FullName, Role, IsActive)
VALUES
('admin@apiforge.local', '100000.MDEyMzQ1Njc4OWFiY2RlZg==.t+/LKFaqsE++N+G0BuuCaechxCGF1PSFppgP56exer4=', 'APIForge Admin', 'Admin', 1),
('dev@apiforge.local', '100000.YWJjZGVmMDEyMzQ1Njc4OQ==.srZb1ueqJn6nKN6P/6ntWn52/xXHdILbktAwdwJZOv4=', 'Demo Developer', 'Developer', 1);
GO

INSERT INTO RegisteredApis (Name, Description, RoutePrefix, DownstreamBaseUrl, Status, CreatedByUserId)
VALUES
('Product API', 'Sample product catalog downstream service.', 'products', 'https://localhost:7301', 'Active', 1),
('Order API', 'Sample order management downstream service.', 'orders', 'https://localhost:7401', 'Active', 1);
GO

-- Seeded demo API key for Product API.
-- Use this key only for local practice: af_DEMO_PRODUCT_KEY_1234567890
INSERT INTO ApiKeys (UserId, RegisteredApiId, Name, KeyPrefix, KeyHash, IsActive, ExpiresAtUtc)
VALUES (2, 1, 'Demo Product Key', 'af_DEMO_PR', '9D573004898DB7E74B2E460472F3FC3D8CA690080CCA760A23E97291C785D6F1', 1, DATEADD(YEAR, 1, SYSUTCDATETIME()));
GO

;WITH n AS (
    SELECT TOP (100) ROW_NUMBER() OVER (ORDER BY (SELECT NULL)) AS i
    FROM sys.objects a CROSS JOIN sys.objects b
)
INSERT INTO Products (Sku, Name, Price, StockQuantity)
SELECT CONCAT('SKU-', FORMAT(i, '000000')), CONCAT('Demo Product ', i), CAST((10 + (i % 500)) AS DECIMAL(18,2)), 50 + (i % 100)
FROM n;
GO

;WITH n AS (
    SELECT TOP (50) ROW_NUMBER() OVER (ORDER BY (SELECT NULL)) AS i
    FROM sys.objects a CROSS JOIN sys.objects b
)
INSERT INTO Customers (FullName, Email)
SELECT CONCAT('Customer ', i), CONCAT('customer', i, '@example.com') FROM n;
GO

;WITH n AS (
    SELECT TOP (200) ROW_NUMBER() OVER (ORDER BY (SELECT NULL)) AS i
    FROM sys.objects a CROSS JOIN sys.objects b
)
INSERT INTO Orders (OrderNumber, CustomerId, TotalAmount, Status, CreatedAtUtc)
SELECT CONCAT('ORD-', FORMAT(i, '000000')), 1 + (i % 50), CAST((100 + (i % 2000)) AS DECIMAL(18,2)),
       CASE WHEN i % 5 = 0 THEN 'Cancelled' WHEN i % 3 = 0 THEN 'Shipped' ELSE 'Created' END,
       DATEADD(MINUTE, -i, SYSUTCDATETIME())
FROM n;
GO

;WITH n AS (
    SELECT TOP (1000) ROW_NUMBER() OVER (ORDER BY (SELECT NULL)) AS i
    FROM sys.objects a CROSS JOIN sys.objects b
)
INSERT INTO GatewayRequestLogs (RegisteredApiId, ApiKeyId, UserId, HttpMethod, RequestPath, StatusCode, ResponseTimeMs, ClientIp, ErrorMessage, CreatedAtUtc)
SELECT CASE WHEN i % 2 = 0 THEN 1 ELSE 2 END,
       1,
       2,
       'GET',
       CASE WHEN i % 2 = 0 THEN '/gateway/products/api/products' ELSE '/gateway/orders/api/orders' END,
       CASE WHEN i % 10 = 0 THEN 500 WHEN i % 7 = 0 THEN 404 ELSE 200 END,
       20 + (i % 900),
       '127.0.0.1',
       CASE WHEN i % 10 = 0 THEN 'Simulated downstream failure' ELSE NULL END,
       DATEADD(MINUTE, -i, SYSUTCDATETIME())
FROM n;
GO
