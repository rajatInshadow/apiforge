# Known Limitations in APIForge v1

This baseline is designed for learning and onboarding simulation. It is not a production-ready API management platform.

## Intentional limitations

- Refresh token flow is not implemented yet. See `AF-BE-201`.
- Redis-based rate limiting is not implemented yet. See `AF-BE-202`.
- Gateway route/key metadata is read from SQL on each request. A later version should cache this.
- Request logs are written synchronously by the gateway. A later version should use a queue.
- Analytics are basic and may become slow with large data. A later version should use daily aggregates.
- Frontend charts are intentionally left for assigned tasks.
- Integration tests and Postman collection are left for assigned tasks.

## Why these are left open

These gaps are realistic onboarding tasks. They help you practice reading an existing codebase, changing both backend and frontend, improving performance, and documenting your work.
