# Developer Tasks Backlog

The baseline project is intentionally not the final product. These are your assigned tasks as a newly onboarded developer.

## Backend Tasks

| ID | Title | Priority | Description |
|---|---|---|---|
| AF-BE-201 | Add refresh token flow | P1 | Add refresh tokens so users do not need to log in again after JWT expiry. |
| AF-BE-202 | Add Redis-based rate limiting | P1 | Limit API calls per API key using Redis counters. |
| AF-BE-203 | Add API subscription approval workflow | P1 | Developers should request access to APIs; admins approve/reject. |
| AF-BE-204 | Add audit log for API changes | P1 | Store create/update/deactivate actions for RegisteredApis and ApiKeys. |
| AF-BE-205 | Add request log filtering by API/date/status/method | P1 | Extend logs API filters. |
| AF-BE-206 | Improve request log pagination | P1 | Add metadata like total pages and next/previous flags. |
| AF-BE-207 | Optimize slow logs query | P1 | Analyze SQL execution and add/adjust indexes. |
| AF-BE-208 | Add API key expiry handling UI support API | P2 | Add APIs to update expiry date. |
| AF-BE-209 | Add daily usage limit per API key | P2 | Reject calls after daily quota is consumed. |
| AF-BE-210 | Add regenerate API key endpoint | P2 | Allow user to rotate a key. |
| AF-BE-211 | Add API versioning support | P2 | Allow route prefixes like products/v1 and products/v2. |
| AF-BE-212 | Add route enable/disable feature | P2 | Admin can disable a route without deleting it. |
| AF-BE-213 | Add global exception middleware | P2 | Standardize error responses. |
| AF-BE-214 | Add latency percentile calculation | P2 | Calculate p50, p95, and p99 latency. |
| AF-BE-215 | Optimize dashboard summary API | P1 | Use pre-aggregated stats or better queries. |
| AF-BE-216 | Add background daily aggregation job | P2 | Populate ApiUsageDailyStats from GatewayRequestLogs. |
| AF-BE-217 | Add FluentValidation | P2 | Validate requests cleanly. |
| AF-BE-218 | Add gateway integration tests | P1 | Test valid key, invalid key, route missing, downstream failure. |
| AF-BE-219 | Improve role-based permission design | P2 | Add permission constants and policy-based authorization. |
| AF-BE-220 | Add API health check endpoint | P2 | Check downstream API health from admin portal. |

## Frontend Tasks

| ID | Title | Priority | Description |
|---|---|---|---|
| AF-FE-301 | Add request logs filter UI | P1 | Filter by API, status code, HTTP method, date range. |
| AF-FE-302 | Add improved pagination component | P1 | Reusable pagination with page size selector. |
| AF-FE-303 | Add API usage chart by date | P1 | Use chart library and analytics endpoint. |
| AF-FE-304 | Add failed request chart | P2 | Show status-code breakdown. |
| AF-FE-305 | Add API key expiry display | P2 | Show expiry date and expired status. |
| AF-FE-306 | Add regenerate API key button | P2 | Call key rotation API after backend ticket. |
| AF-FE-307 | Add subscription approval screen | P1 | Admin can approve/reject developer access. |
| AF-FE-308 | Add audit log page | P2 | Display admin/system audit events. |
| AF-FE-309 | Improve loading and error states globally | P1 | Add reusable Loading, Error, EmptyState components. |
| AF-FE-310 | Improve protected routes | P2 | Redirect based on role and auth state. |
| AF-FE-311 | Add role-based sidebar menu | P1 | Hide admin-only screens from developers. |
| AF-FE-312 | Improve API detail page | P2 | Show docs, keys, sample curl, usage summary. |
| AF-FE-313 | Add reusable table component | P2 | Shared sortable table. |
| AF-FE-314 | Add confirmation modal | P2 | Use before destructive actions. |
| AF-FE-315 | Add form validation | P1 | Validate API registration and API key forms. |
| AF-FE-316 | Add empty-state UI | P3 | Improve UX when no data exists. |

## Database / Performance Tasks

| ID | Title | Priority | Description |
|---|---|---|---|
| AF-DB-401 | Benchmark GatewayRequestLogs indexes | P1 | Compare IO/time before and after index changes. |
| AF-DB-402 | Optimize analytics aggregation query | P1 | Improve dashboard performance with large data. |
| AF-DB-403 | Add stored procedure for daily usage summary | P2 | Create summarized data for dashboards. |
| AF-DB-404 | Add large seed data controls | P2 | Parameterize seed-large.sql target sizes. |
| AF-DB-405 | Add archive strategy document | P3 | Propose old log archival strategy. |
| AF-DB-406 | Add query execution notes | P2 | Record observations in PerformanceGuide.md. |

## Testing / Documentation Tasks

| ID | Title | Priority | Description |
|---|---|---|---|
| AF-TD-501 | Add Postman collection | P1 | Include auth, API registration, key creation, gateway call. |
| AF-TD-502 | Add API key validation test cases | P1 | Test missing, invalid, expired, inactive, scoped key. |
| AF-TD-503 | Update onboarding after refresh token | P2 | Keep docs in sync after feature work. |
| AF-TD-504 | Add troubleshooting section | P2 | Document common startup/database/CORS issues. |
| AF-TD-505 | Add PR template | P3 | Standardize changes like a team project. |
