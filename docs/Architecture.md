# APIForge Architecture Notes

## Why Gateway is separate

The gateway is the entry point for external API calls. It should be separated from the portal backend because gateway traffic and portal traffic have different purposes.

- Gateway handles high-volume API traffic.
- Portal API handles management screens.
- Gateway validates API keys.
- Portal API validates JWT tokens.

## Why Portal API is separate

The portal backend manages business/admin operations:

- Register APIs
- Generate keys
- View logs
- View analytics

This is different from forwarding client traffic.

## Why sample services exist

Product Service and Order Service simulate downstream APIs. In real companies, these would be independent services owned by different teams.

## Communication style

### Synchronous HTTP

Gateway forwards requests to downstream services using HTTP.

```text
Client → Gateway → Product Service
```

This is synchronous because the client waits for a response.

### Future asynchronous communication

In a future version, request logs can be pushed to RabbitMQ instead of written directly by the Gateway.

```text
Gateway → RabbitMQ → Analytics Worker → SQL Server
```

This improves gateway latency and reliability.

## Where SQL Server fits

SQL Server stores:

- Users
- API registrations
- API keys
- Request logs
- Analytics aggregates
- Sample service data

## Where Redis fits later

Redis can be added for:

- API rate limiting
- Caching frequently used route/API key data
- Temporary counters

## Where Docker fits later

Docker Compose can run:

- Portal API
- Gateway
- Product Service
- Order Service
- SQL Server
- Redis

## Architecture style

APIForge v1 is a multi-project backend with separate services. It is not a fully production-hardened microservice system yet, but it gives you realistic experience with distributed service boundaries.
