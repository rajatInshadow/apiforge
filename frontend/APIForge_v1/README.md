# APIForge — API Gateway + Developer Portal

APIForge is a company-style full-stack training project for learning API gateways, developer portals, authentication, API key management, request logging, analytics, SQL performance, and real project onboarding.

This is **v1 runnable baseline**. It is intentionally complete enough to run locally, but it also leaves realistic tickets for you to implement like a newly onboarded developer.

## Tech stack

- Backend: ASP.NET Core Web API (.NET 8 style)
- Frontend: React + TypeScript + Vite
- Database: SQL Server
- ORM: Entity Framework Core
- Auth: JWT + role-based authorization
- Gateway: ASP.NET Core catch-all reverse proxy controller using `HttpClient`
- Docs: HLD, LLD, architecture, API contracts, onboarding, developer tickets, performance guide

## Folder structure

```text
APIForge_v1/
  backend/
    APIForge.sln.instructions.md
    src/
      APIForge.Shared/
      APIForge.Infrastructure/
      APIForge.PortalApi/
      APIForge.Gateway/
      APIForge.Sample.ProductService/
      APIForge.Sample.OrderService/
  frontend/
    apiforge-portal/
  database/
    schema.sql
    seed-small.sql
    seed-large.sql
    indexes.sql
    performance-test-queries.sql
  docs/
    HLD.md
    LLD.md
    Architecture.md
    APIContracts.md
    OnboardingGuide.md
    DeveloperTasks.md
    PerformanceGuide.md
    GitWorkflow.md
```

## Prerequisites

Install these locally:

- .NET SDK 8+
- Node.js 20+
- SQL Server Developer Edition or SQL Server Express
- SQL Server Management Studio
- Optional: Postman

## Quick start

### 1. Restore database

Open SSMS and run these files in order:

```text
database/schema.sql
database/seed-small.sql
database/indexes.sql
```

For performance practice, run `database/seed-large.sql` later. It generates large volumes using T-SQL loops and may take time depending on your machine.

### 2. Create backend solution locally

Because this ZIP is generated without running the .NET CLI in this environment, use the commands below from `backend/` to create a `.sln` file on your machine:

```bash
dotnet new sln -n APIForge

dotnet sln add src/APIForge.Shared/APIForge.Shared.csproj
dotnet sln add src/APIForge.Infrastructure/APIForge.Infrastructure.csproj
dotnet sln add src/APIForge.PortalApi/APIForge.PortalApi.csproj
dotnet sln add src/APIForge.Gateway/APIForge.Gateway.csproj
dotnet sln add src/APIForge.Sample.ProductService/APIForge.Sample.ProductService.csproj
dotnet sln add src/APIForge.Sample.OrderService/APIForge.Sample.OrderService.csproj
```

### 3. Update connection strings

Update `appsettings.Development.json` in:

```text
APIForge.PortalApi
APIForge.Gateway
APIForge.Sample.ProductService
APIForge.Sample.OrderService
```

Default connection string:

```json
"Server=localhost;Database=APIForgeDb;Trusted_Connection=True;TrustServerCertificate=True;"
```

### 4. Run backend services

Open separate terminals:

```bash
cd backend/src/APIForge.PortalApi
dotnet run
```

```bash
cd backend/src/APIForge.Gateway
dotnet run
```

```bash
cd backend/src/APIForge.Sample.ProductService
dotnet run
```

```bash
cd backend/src/APIForge.Sample.OrderService
dotnet run
```

Default local ports:

```text
Portal API:      https://localhost:7101
Gateway:         https://localhost:7201
Product Service: https://localhost:7301
Order Service:   https://localhost:7401
Frontend:        http://localhost:5173
```

### 5. Run frontend

```bash
cd frontend/apiforge-portal
npm install
npm run dev
```

### 6. Demo login

```text
Admin:
Email: admin@apiforge.local
Password: Admin@123

Developer:
Email: dev@apiforge.local
Password: Dev@123
```

### 7. Test gateway

After logging in as developer, generate an API key from the portal. Or use the seeded key note from `seed-small.sql`.

Example request:

```bash
curl -k -H "x-api-key: <your-api-key>" https://localhost:7201/gateway/products/api/products
```

## What is intentionally left for you

See `docs/DeveloperTasks.md`. Important assigned work includes:

- Redis-based rate limiting
- Refresh token flow
- Advanced request log filters
- Analytics charts
- Swagger/OpenAPI import
- Subscription approval workflow
- Audit log improvements
- Gateway integration tests
- SQL performance optimization

## Learning goal

Treat this project like this scenario:

> You joined a company that already has APIForge running. Your job is to understand the architecture, run it locally, debug the gateway flow, and implement assigned tickets across backend, frontend, and database performance.


## Backend solution

The backend solution file is included at `backend/APIForge.sln`. Open this file in Visual Studio 2022 or run `dotnet restore` from the `backend` folder.
