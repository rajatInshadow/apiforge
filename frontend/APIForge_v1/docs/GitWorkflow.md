# Git Workflow

## Branch naming

```text
feature/AF-BE-201-refresh-token
feature/AF-FE-301-log-filters
bugfix/AF-BE-205-log-date-filter
performance/AF-DB-401-log-indexes
```

## Commit messages

```text
feat(auth): add refresh token entity
feat(gateway): add Redis rate limiting middleware
fix(logs): correct status-code filter
perf(sql): add composite index for logs query
refactor(frontend): extract reusable table component
```

## Pull request format

```md
## Ticket
AF-FE-301 Add request logs filter UI

## Changes
- Added API dropdown filter
- Added status-code filter
- Added date range inputs
- Connected filters with logs API

## Testing
- Verified default logs page loads
- Verified API filter works
- Verified status code filter works
- Verified pagination still works

## Notes
Backend currently supports apiId and statusCode only. Date filtering requires AF-BE-205.
```
