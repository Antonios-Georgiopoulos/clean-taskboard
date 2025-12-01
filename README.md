![Build](https://github.com/Antonios-Georgiopoulos/clean-taskboard/actions/workflows/build.yml/badge.svg)
![.NET](https://img.shields.io/badge/.NET-10.0-blueviolet?logo=dotnet)
![Docker](https://img.shields.io/badge/Docker-Ready-2496ED?logo=docker&logoColor=white)
![PostgreSQL](https://img.shields.io/badge/PostgreSQL-16-336791?logo=postgresql&logoColor=white)
![Architecture](https://img.shields.io/badge/Architecture-Clean-informational)
![License](https://img.shields.io/badge/License-MIT-green)

# CleanTaskBoard

A Clean Architecture Kanban backend built with .NET, CQRS, MediatR, EF Core, PostgreSQL, and JWT-based authentication.

## Overview

CleanTaskBoard is a production-ready backend inspired by Trello/Jira-style Kanban systems.  
It demonstrates real enterprise engineering practices:

- Clean Architecture (Domain → Application → Infrastructure → API)
- CQRS + MediatR
- PostgreSQL persistence via EF Core
- Dockerized development environment
- Full Kanban feature set (Boards, Columns, Tasks, Subtasks)
- Task movement, reordering, completion logic
- **Authentication & Authorization with JWT**
- **Board membership & role-based access control (Owner / Member / Viewer)**

---

## Features

### ✔ Authentication & Users

- Register new user
- Login with email/password
- Issue JWT access tokens
- Password hashing with PBKDF2
- Secured endpoints via `Authorization: Bearer <token>`

### ✔ Boards

- Create board (authenticated)
- Get boards for current user (owner)
- Get board by ID (Owner / Member / Viewer)
- Board membership model (Owner / Member / Viewer)

### ✔ Board Membership

- Add member to board (Owner only)
- Remove member from board (Owner only)
- Update member role (Owner only)
- List board members (Owner only)
- Roles:
  - **Owner** → full control, manage membership, manage columns/tasks/subtasks
  - **Member** → can create/update/delete/move tasks & subtasks
  - **Viewer** → read-only

### ✔ Columns

- Create column (Owner only)
- Get columns per board (Owner / Member / Viewer)
- Delete column (Owner only)

### ✔ Tasks

- CRUD (Owner / Member)
- Move between columns (Owner / Member)
- Reorder inside a column (Owner / Member)
- Toggle completion (Owner / Member)
- Read-only access for Viewer

### ✔ Subtasks

- CRUD (Owner / Member)
- Toggle completion (Owner / Member)
- Reorder (Owner / Member)
- Read-only access for Viewer

---

## Architecture

### Clean Architecture Layers

```text
Domain       → Enterprise business rules
Application  → Use-cases (CQRS / MediatR, validation, authorization)
Infrastructure → EF Core & PostgreSQL, repositories, security (JWT, hashing)
API          → Minimal API endpoints, DI wiring, exception handling
```

See `docs/architecture.md` and `docs/diagrams` for full diagrams.

### Authentication & Authorization

- **Domain**
  - `User`
  - `Board`
  - `BoardMembership`
  - `BoardRole (Owner, Member, Viewer)`

- **Application**
  - Auth use-cases: Register / Login
  - `IJwtTokenGenerator`, `IPasswordHasher`
  - `IBoardAccessService` for role-based access:
    - `EnsureCanReadBoard`, `EnsureCanEditBoard`, `EnsureCanManageMembership`
    - `EnsureCanRead/CanEditColumn`
    - `EnsureCanRead/CanEditTask`, `EnsureCanEditTasksForColumn`
    - `EnsureCanRead/CanEditSubtask`, `EnsureCanEditSubtasksForTask`
  - Handlers call access service before repositories.

- **Infrastructure**
  - `UserRepository`, `BoardRepository`, `BoardMembershipRepository`
  - `JwtTokenGenerator` using `System.IdentityModel.Tokens.Jwt`
  - `PasswordHasher` using PBKDF2
  - EF Core `AppDbContext` with all entities

- **API**
  - Minimal API endpoints for:
    - `/auth/register`
    - `/auth/login`
    - `/boards`, `/boards/{boardId}`
    - `/boards/{boardId}/members`
    - `/columns`, `/tasks`, `/subtasks`
  - JWT authentication with `JwtBearer` middleware
  - Global exception handling returning problem details JSON

---

## Run Locally (Development)

### 1. Start PostgreSQL via Docker

```bash
docker compose up -d
```

### 2. Apply EF Core Migrations

```bash
dotnet ef database update --project src/CleanTaskBoard.Infrastructure --startup-project src/CleanTaskBoard.Api
```

### 3. Run the API

```bash
dotnet run --project src/CleanTaskBoard.Api
```

The API runs on:

```text
https://localhost:5001
```

---

## Authentication Flow

### 1. Register

`POST /auth/register`

```json
{
  "username": "owner",
  "email": "owner@example.com",
  "password": "StrongPassword123!"
}
```

### 2. Login

`POST /auth/login`

```json
{
  "email": "owner@example.com",
  "password": "StrongPassword123!"
}

Copy the token from the response and use it as:
Authorization: Bearer <token>
```

Response:

```json
{
  "accessToken": "<JWT token>",
  "expiresIn": 3600
}
```

Copy the `accessToken` and use it as:

```http
Authorization: Bearer <accessToken>
```

in all subsequent requests.

---

## RBAC Model (Boards)

| Role    | Boards | Columns        | Tasks/Subtasks        | Membership |
|--------|--------|----------------|------------------------|-----------|
| Owner  | Full   | Create/Delete  | Full (CRUD + move)     | Full      |
| Member | Read   | Read           | Create/Update/Delete   | None      |
| Viewer | Read   | Read           | Read-only              | None      |

Non-members receive `404`/`403` via the access service, without leaking board existence.

---

## Postman Collection

A full Postman collection is available under `/docs/postman/CleanTaskBoard.postman_collection.json`.

It includes:

- Auth folder (Register/Login)
- Boards / Columns / Tasks / Subtasks
- Board Members (membership management)
- Shared `{{baseUrl}}` and `{{accessToken}}` variables

---

## Project Structure

```text
CleanTaskBoard/
├─ src/
│  ├─ CleanTaskBoard.Api/
│  ├─ CleanTaskBoard.Application/
│  ├─ CleanTaskBoard.Domain/
│  └─ CleanTaskBoard.Infrastructure/
├─ docs/
│  ├─ architecture.md
│  ├─ diagrams/
│  │  ├─ entities.md
│  │  ├─ cqrs-flow.md
│  │  └─ layers.md
│  └─ postman/
│     └─ CleanTaskBoard.postman_collection.json
├─ docker-compose.yml
└─ README.md
```

---

## Tech Stack

- .NET 9 / 10 preview  
- EF Core 9  
- PostgreSQL  
- Docker  
- MediatR  
- FluentValidation  
- Minimal API  
- JWT Authentication (Bearer tokens)

---

## License

MIT License.
