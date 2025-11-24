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

---

## Features

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

## Architecture

### Clean Architecture Layers

```
Domain → Enterprise business rules  
Application → Use-cases (CQRS / MediatR)  
Infrastructure → EF Core & PostgreSQL  
API → Minimal API endpoints  
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
- FluentValidation  
- Minimal API  

---

## License
MIT License.
