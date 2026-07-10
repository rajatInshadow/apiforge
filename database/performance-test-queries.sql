USE APIForgeDb;
GO

SET STATISTICS IO ON;
SET STATISTICS TIME ON;
GO

-- 1. Recent logs page
SELECT TOP 50 Id, RegisteredApiId, StatusCode, ResponseTimeMs, RequestPath, CreatedAtUtc
FROM GatewayRequestLogs
ORDER BY CreatedAtUtc DESC;
GO

-- 2. Logs filtered by API and date range
DECLARE @ApiId INT = 1;
DECLARE @FromDate DATETIME2 = DATEADD(DAY, -7, SYSUTCDATETIME());
SELECT TOP 100 Id, RegisteredApiId, StatusCode, ResponseTimeMs, RequestPath, CreatedAtUtc
FROM GatewayRequestLogs
WHERE RegisteredApiId = @ApiId AND CreatedAtUtc >= @FromDate
ORDER BY CreatedAtUtc DESC;
GO

-- 3. Failure logs
SELECT TOP 100 Id, RegisteredApiId, StatusCode, ResponseTimeMs, ErrorMessage, CreatedAtUtc
FROM GatewayRequestLogs
WHERE StatusCode >= 400
ORDER BY CreatedAtUtc DESC;
GO

-- 4. Usage summary aggregation
SELECT RegisteredApiId,
       COUNT_BIG(*) AS TotalRequests,
       SUM(CASE WHEN StatusCode >= 400 THEN 1 ELSE 0 END) AS FailedRequests,
       AVG(CAST(ResponseTimeMs AS FLOAT)) AS AverageResponseTimeMs
FROM GatewayRequestLogs
GROUP BY RegisteredApiId
ORDER BY TotalRequests DESC;
GO

SET STATISTICS IO OFF;
SET STATISTICS TIME OFF;
GO
