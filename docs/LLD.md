# APIForge LLD — Low-Level Design

## 1. Backend project responsibilities

| Project | Responsibility |
|---|---|
| APIForge.Shared | DTOs, constants, security helpers |
| APIForge.Infrastructure | Entities and EF Core DbContext |
| APIForge.PortalApi | Auth, API registry, keys, logs, analytics |
| APIForge.Gateway | Gateway forwarding and request logging |
| APIForge.Sample.ProductService | Sample product API |
| APIForge.Sample.OrderService | Sample order API |

## 2. Important entities

### User

Fields: `Id`, `Email`, `PasswordHash`, `FullName`, `Role`, `IsActive`, `CreatedAtUtc`.

### RegisteredApi

Fields: `Id`, `Name`, `Description`, `RoutePrefix`, `DownstreamBaseUrl`, `Status`, `CreatedByUserId`, timestamps.

### ApiKey

Fields: `Id`, `UserId`, `RegisteredApiId`, `Name`, `KeyPrefix`, `KeyHash`, `IsActive`, expiry.

Plain API keys are shown only once during generation. The database stores only SHA-256 hash.

### GatewayRequestLog

Fields: `RegisteredApiId`, `ApiKeyId`, `UserId`, `HttpMethod`, `RequestPath`, `StatusCode`, `ResponseTimeMs`, `ClientIp`, `ErrorMessage`, `CreatedAtUtc`.

## 3. Controller design

| Controller | APIs |
|---|---|
| AuthController | Register, login, current user |
| RegisteredApisController | List, get, create, update APIs |
| ApiKeysController | List, create, deactivate keys |
| RequestLogsController | Paginated logs |
| AnalyticsController | Dashboard summary |
| GatewayController | Catch-all forwarding |

## 4. API key validation logic

```text
Read x-api-key header
If missing: return 401
Hash key using SHA-256
Find active ApiKey by hash
If expired: return 401
Find RegisteredApi by routePrefix
If inactive/missing: return 404
If key is scoped to different API: return 403
Forward request
```

## 5. Gateway forwarding logic

`GatewayController` uses `HttpClient` to create a new downstream request.

```text
Incoming: /gateway/products/api/products?page=1
Route prefix: products
Remaining path: api/products
Downstream base URL: https://localhost:7301
Target URL: https://localhost:7301/api/products?page=1
```

## 6. Error handling

Current v1 has simple controller-level error handling in Gateway. Your assigned ticket will improve global exception handling.

## 7. Pagination design

Logs endpoint uses:

```text
page
pageSize
apiId optional
statusCode optional
```

Your assigned task will add date filters, method filter, and UI filters.

## 8. Performance notes

The `GatewayRequestLogs` table is expected to become large. Indexes are added on:

- `CreatedAtUtc`
- `(RegisteredApiId, CreatedAtUtc)`
- `(StatusCode, CreatedAtUtc)`

Your performance tasks will compare query behavior before and after indexing.
