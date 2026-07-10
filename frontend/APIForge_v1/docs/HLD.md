# APIForge HLD — High-Level Design

## 1. Project overview

APIForge is an API Gateway and Developer Portal platform. It allows administrators to register backend APIs, developers to generate API keys, and external clients to access downstream APIs through a controlled gateway.

## 2. Business goal

In enterprise systems, teams expose APIs to internal or external developers. They need a common layer for authentication, route management, request logging, usage analytics, and access control. APIForge simulates that platform.

## 3. Major users

| User | Responsibility |
|---|---|
| Admin | Registers APIs, manages routes, monitors usage |
| Developer | Generates API keys and consumes APIs |
| External Client | Calls gateway endpoints using API keys |

## 4. High-level architecture

```text
React Developer Portal
        ↓ JWT
Portal API
        ↓ EF Core
SQL Server

External Client
        ↓ x-api-key
APIForge Gateway
        ↓ HTTP forwarding
Product Service / Order Service
        ↓
GatewayRequestLogs in SQL Server
```

## 5. Core modules

| Module | Purpose |
|---|---|
| Auth | Login, registration, JWT, roles |
| API Registry | Stores registered APIs and downstream URLs |
| API Key Management | Generates and validates developer keys |
| Gateway | Validates key, resolves route, forwards request |
| Request Logs | Stores every gateway call |
| Analytics | Aggregates request counts, failures, latency |
| Sample Services | Product and Order services for gateway testing |

## 6. Gateway flow

```text
Client sends request to /gateway/{routePrefix}/{path}
Gateway reads x-api-key
Gateway hashes and validates key
Gateway loads route by routePrefix
Gateway checks API key access
Gateway forwards request to downstream service
Gateway captures status code and response time
Gateway writes GatewayRequestLog
Gateway returns downstream response
```

## 7. Deployment view

For local development, each service runs separately:

| Service | Port |
|---|---:|
| Portal API | 7101 |
| Gateway | 7201 |
| Product Service | 7301 |
| Order Service | 7401 |
| React Portal | 5173 |

## 8. Scalability considerations

- Request logs will become large quickly.
- Logs API must use pagination.
- Dashboard should avoid expensive live aggregation on every request at high scale.
- Future version should aggregate logs in a background worker.
- Redis can be used for rate limiting and caching.
- RabbitMQ can be used for async log ingestion.

## 9. Security considerations

- API keys are stored as hashes, not plain text.
- JWT protects portal APIs.
- Admin-only APIs are role-protected.
- API keys can be scoped to a specific registered API.
- HTTPS should be used in real deployments.

## 10. Future architecture upgrades

- Redis rate limiting
- RabbitMQ log pipeline
- OpenTelemetry tracing
- API versioning
- Tenant isolation
- API subscription approval workflow
