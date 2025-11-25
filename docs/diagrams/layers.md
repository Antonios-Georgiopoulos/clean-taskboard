# Layer Diagram

```mermaid
flowchart TD
    A[Domain] --> B[Application]
    B --> C[Infrastructure]
    C --> D[API]

    style A fill:#6a0dad,stroke:#6a0dad
    style B fill:#005f93,stroke:#005f93
    style C fill:#aa0000,stroke:#aa0000
    style D fill:#008000,stroke:#008000
```

- **Domain**: core entities, value objects, enums (Boards, Columns, Tasks, Subtasks, Users).
- **Application**: CQRS handlers, use-cases, interfaces, authentication commands.
- **Infrastructure**: persistence (EF Core, PostgreSQL), repositories, security (hashing/JWT).
- **API**: minimal API endpoints, dependency injection wiring, OpenAPI, JWT middleware.
