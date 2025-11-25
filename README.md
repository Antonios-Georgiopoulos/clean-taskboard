![Build](https://github.com/Antonios-Georgiopoulos/clean-taskboard/actions/workflows/build.yml/badge.svg)
![.NET](https://img.shields.io/badge/.NET-10.0-blueviolet?logo=dotnet)
![Docker](https://img.shields.io/badge/Docker-Ready-2496ED?logo=docker&logoColor=white)
![PostgreSQL](https://img.shields.io/badge/PostgreSQL-16-336791?logo=postgresql&logoColor=white)
![Architecture](https://img.shields.io/badge/Architecture-Clean-informational)
![License](https://img.shields.io/badge/License-MIT-green)

# CleanTaskBoard

A Clean Architecture Kanban backend built with .NET, CQRS, MediatR, EF Core, and PostgreSQL.

## Overview
CleanTaskBoard is a production-ready backend inspired by Trello/Jira-style Kanban systems.  
It demonstrates real enterprise engineering practices:
- Clean Architecture (Domain → Application → Infrastructure → API)
- CQRS + MediatR
- PostgreSQL persistence via EF Core
- Dockerized development environment
- Full Kanban feature set (Boards, Columns, Tasks, Subtasks)
- Task movement, reordering, completion logic
- **JWT-based authentication (Users, Register/Login, secure boards)**

---

## Features

### ✔ Authentication & Users
- User registration (`/auth/register`)
- User login (`/auth/login`)
- Secure JWT token generation
- Password hashing (PBKDF2-based)
- Ready to secure boards/tasks per authenticated user

### ✔ Boards
- Create board  
- Get boards  
- Get board by ID  

### ✔ Columns
- Create column  
- Get columns per board  
- Delete column  

### ✔ Tasks
- CRUD  
- Move between columns  
- Reorder inside a column  
- Toggle completion  

### ✔ Subtasks
- CRUD  
- Toggle completion  
- Reorder  

---

## Authentication

### Register

**Endpoint:** `POST /auth/register`  

**Request body:**
```json
{
  "username": "testuser",
  "email": "test@example.com",
  "password": "P@ssw0rd!"
}
```

**Response:**
```json
{
  "userId": "GUID",
  "username": "testuser",
  "email": "test@example.com",
  "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9..."
}
```

### Login

**Endpoint:** `POST /auth/login`  

**Request body:**
```json
{
  "email": "test@example.com",
  "password": "P@ssw0rd!"
}
```

**Response:**
```json
{
  "userId": "GUID",
  "username": "testuser",
  "email": "test@example.com",
  "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9..."
}
```

### Using the JWT token

When accessing protected endpoints (e.g. boards, columns, tasks, subtasks), send the token in the HTTP header:

```http
Authorization: Bearer {{token}}
```

In Postman, you can set a collection variable `token` and use `Bearer {{token}}` across requests.

> Note: Currently, authentication is wired and functional. Board ownership and per-user data scoping will be introduced in the next milestone.

---

## Architecture

### Clean Architecture Layers

```
Domain → Enterprise business rules  
Application → Use-cases (CQRS / MediatR)  
Infrastructure → EF Core & PostgreSQL, Repositories, Security (PasswordHasher, JwtTokenGenerator)  
API → Minimal API endpoints, DI wiring, JWT Authentication & Authorization  
```

See `docs/architecture.md` for full diagrams.

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

```
https://localhost:5001
```

---

## Postman Collection
A full Postman collection is available under `/docs/postman/CleanTaskBoard.postman_collection.json`.

The collection includes:
- Auth endpoints (`/auth/register`, `/auth/login`)
- Boards / Columns / Tasks / Subtasks
- Support for a `token` collection variable used in the `Authorization` header.

---

## Project Structure

```
CleanTaskBoard/
├─ src/
│  ├─ CleanTaskBoard.Api/
│  ├─ CleanTaskBoard.Application/
│  ├─ CleanTaskBoard.Domain/
│  └─ CleanTaskBoard.Infrastructure/
├─ docs/
│  ├─ architecture.md
│  └─ diagrams/
│     ├─ entities.md
│     ├─ cqrs-flow.md
│     └─ layers.md
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
- Minimal API  
- JWT Authentication  

---

## License
MIT License.
