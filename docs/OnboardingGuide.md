# APIForge Onboarding Guide

## Scenario

You joined the APIForge team as a full-stack developer. The project is already running. Your first responsibility is to understand the architecture, run it locally, and complete assigned tickets.

## Day 1 checklist

1. Read `README.md`.
2. Restore SQL database using `schema.sql` and `seed-small.sql`.
3. Run Portal API.
4. Run Gateway.
5. Run Product and Order services.
6. Run frontend portal.
7. Login as admin.
8. Generate a developer API key.
9. Call a downstream API through gateway.
10. Check request logs.

## How to trace a gateway request

1. Client sends request to Gateway.
2. `GatewayController.Forward` receives route prefix.
3. Controller validates `x-api-key`.
4. Controller fetches matching `RegisteredApi`.
5. Controller builds target URL.
6. Controller forwards using `HttpClient`.
7. Controller writes `GatewayRequestLog`.
8. Portal reads logs from `RequestLogsController`.

## How to add a new API

1. Login as Admin.
2. Open API Catalog.
3. Register API with route prefix and downstream URL.
4. Generate API key for that API.
5. Call `/gateway/{routePrefix}/...` with key.

## How to work on assigned tickets

For each ticket:

```text
git checkout -b feature/AF-201-short-title
understand existing flow
update backend/frontend/database
run affected services
test manually
commit with clear message
write PR notes
```

## First ticket recommendation

Start with `AF-FE-301: Add request logs filter UI` because it touches frontend + existing backend filters but is not too difficult.

Then move to `AF-BE-206: Add server-side pagination improvements` and `AF-DB-401: Add indexes for GatewayRequestLogs`.


## Opening the backend

Open `backend/APIForge.sln` in Visual Studio 2022. The solution includes Portal API, Gateway, Shared, Infrastructure, Product Service, and Order Service projects.
