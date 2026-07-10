# APIForge Performance Guide

## Goal

Learn how large database tables affect API performance and how to improve slow APIs.

## Main table for performance practice

`GatewayRequestLogs` is the main high-volume table. It can grow from 1,000 rows to 500,000+ rows using `seed-large.sql`.

## Exercise 1: Slow logs page

API:

```text
GET /api/request-logs?page=1&pageSize=50
```

What to check:

- Does SQL scan the whole table?
- Is `ORDER BY CreatedAtUtc DESC` using an index?
- Is API returning only required fields?

Optimization techniques:

- Server-side pagination
- Index on `CreatedAtUtc DESC`
- DTO projection
- `AsNoTracking()`

## Exercise 2: Logs filtered by API

API:

```text
GET /api/request-logs?apiId=1&page=1&pageSize=50
```

Useful index:

```sql
CREATE INDEX IX_GatewayRequestLogs_ApiId_CreatedAtUtc
ON GatewayRequestLogs (RegisteredApiId, CreatedAtUtc DESC)
INCLUDE (StatusCode, ResponseTimeMs, RequestPath);
```

## Exercise 3: Failed request dashboard

Query:

```sql
SELECT TOP 100 *
FROM GatewayRequestLogs
WHERE StatusCode >= 400
ORDER BY CreatedAtUtc DESC;
```

Challenge:

A direct condition `StatusCode >= 400` may not always use the exact index optimally depending on data distribution.

## Exercise 4: Dashboard aggregation

Problem:

```sql
SELECT RegisteredApiId, COUNT_BIG(*), AVG(ResponseTimeMs)
FROM GatewayRequestLogs
GROUP BY RegisteredApiId;
```

This becomes expensive with millions of rows.

Better approach:

- Create background aggregation job.
- Store daily summaries in `ApiUsageDailyStats`.
- Dashboard reads summary table instead of raw logs.

## Exercise 5: Measure before and after

Use:

```sql
SET STATISTICS IO ON;
SET STATISTICS TIME ON;
```

Record:

- logical reads
- CPU time
- elapsed time
- API response time from browser/Postman

## Backend performance checklist

- Use `AsNoTracking()` for read-only queries.
- Use `.Select()` projection to DTO.
- Avoid loading navigation properties unless needed.
- Paginate high-volume endpoints.
- Add indexes based on actual query patterns.
- Avoid live aggregation over huge tables for every dashboard request.
- Cache frequently accessed metadata like API routes and key details.

## Your assigned performance outcome

You should be able to explain:

> The logs endpoint was slow with 500k rows because it sorted and filtered a large table. I improved it by adding server-side pagination, indexing query columns, using DTO projection, and comparing SQL statistics before and after optimization.
