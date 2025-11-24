# Architecture Overview

This document describes the Clean Architecture structure of CleanTaskBoard.

```mermaid
flowchart LR
    A[Domain Layer] --> B[Application Layer]
    B --> C[Infrastructure Layer]
    C --> D[API Layer]
```

- **Domain**: Entities, enums, core logic
- **Application**: CQRS, MediatR handlers, interfaces
- **Infrastructure**: EF Core, PostgreSQL, repositories
- **API**: Minimal API endpoints, DI wiring
