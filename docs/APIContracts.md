# API Contracts

## Auth APIs

### POST `/api/auth/register`

Creates a user.

Request:

```json
{
  "email": "dev@apiforge.local",
  "password": "Dev@123",
  "fullName": "Demo Developer",
  "role": "Developer"
}
```

Response: `AuthResponse` with JWT.

### POST `/api/auth/login`

Authenticates user.

Request:

```json
{
  "email": "admin@apiforge.local",
  "password": "Admin@123"
}
```

### GET `/api/auth/me`

Requires JWT.

## API Registry APIs

### GET `/api/apis`

Requires JWT. Returns all registered APIs.

### GET `/api/apis/{id}`

Requires JWT. Returns one API.

### POST `/api/apis`

Requires Admin role.

```json
{
  "name": "Product API",
  "description": "Product catalog service",
  "routePrefix": "products",
  "downstreamBaseUrl": "https://localhost:7301",
  "status": "Active"
}
```

### PUT `/api/apis/{id}`

Requires Admin role.

## API Key APIs

### GET `/api/api-keys`

Requires JWT. Returns current user's keys.

### POST `/api/api-keys`

Requires JWT.

```json
{
  "name": "Product Key",
  "registeredApiId": 1,
  "expiresAtUtc": null
}
```

Response includes `plainTextKey`. It is shown once only.

### PATCH `/api/api-keys/{id}/deactivate`

Deactivates current user's key.

## Request Log APIs

### GET `/api/request-logs?page=1&pageSize=50&apiId=1&statusCode=500`

Requires JWT. Returns paginated logs.

## Analytics APIs

### GET `/api/analytics/summary`

Requires JWT. Returns total requests, failures, average latency, top APIs, and recent failures.

## Gateway APIs

### ANY `/gateway/{routePrefix}/{**remainingPath}`

Requires `x-api-key` header.

Example:

```bash
curl -k -H "x-api-key: af_DEMO_PRODUCT_KEY_1234567890" https://localhost:7201/gateway/products/api/products
```
