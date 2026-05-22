# 🚀 Project & Task Management API

A scalable, robust, and production-ready **Task Management RESTful API** built with **.NET 9**, designed following **Clean Architecture** principles and implementing the **CQRS** pattern.

This system is designed as a backend solution for users to securely manage projects and tasks with Role-Based Access Control (RBAC).

---

## 🏗️ Architecture & Technical Stack

### **Architecture Overview**
The project strictly adheres to **Clean Architecture** to ensure separation of concerns, testability, and maintainability:
1. **Domain Layer:** Contains core business entities (`User`, `Project`, `ProjectTask`) and enums.
2. **Application Layer:** Contains business logic, MediatR Commands/Queries (CQRS), DTOs, and FluentValidation pipelines.
3. **Infrastructure Layer:** Implements data access using **Entity Framework Core**, Repository pattern, and External services (JWT Auth, Redis Caching).
4. **Presentation Layer:** The ASP.NET Core Web API layer consisting of API Controllers and Middleware (Global Exception Handling).

### **Technology Stack**
- **Framework:** .NET 9.0 (ASP.NET Core Web API)
- **Database:** Microsoft SQL Server (Dockerized)
- **ORM:** Entity Framework Core (Code-First)
- **Caching:** Redis (Distributed Caching)
- **Architecture Patterns:** Clean Architecture, CQRS (MediatR), Repository Pattern
- **Security:** JWT Authentication, Role-Based Authorization
- **API Versioning:** Asp.Versioning.Mvc (`v1` by default)
- **Validation:** FluentValidation
- **Containerization:** Docker & Docker Compose

---

## 🐳 Docker & Infrastructure Setup (`compose.yaml`)

The easiest way to run the entire infrastructure and API is using **Docker Compose**. 
The `compose.yaml` file configures three interconnected containers:
1. **SQL Server (`sqlserver`):** Hosts the main relational database on port `1433`.
2. **Redis (`redis`):** In-memory data store for extremely fast caching of project and task data on port `6379`.
3. **API (`api`):** The .NET 9 Web API application itself, exposing port `5265`.

### How to Run:
Make sure Docker Desktop is installed and running, then execute:
```bash
docker compose up -d --build
```
> **Note:** The API will automatically apply EF Core Migrations and create the database schema on startup.

---

## 🔐 Authentication & Roles

The system uses **Stateless JWT Authentication**. There are two roles: `Admin` and `User`.

**Default Seeded Admin User:**
- **Email:** `admin@system.com`
- **Password:** `Admin123!`

**Role Permissions:**
- **Admin:** Can Create, Update, and Delete both Projects and Tasks.
- **User:** Can View Projects/Tasks, and Update the Status of a Task. Cannot create or delete.

---

## 🧪 Testing the API (Postman & Swagger)

You can explore and test the endpoints using either the included **Postman Collection** or the built-in **Swagger UI**.

### 1️⃣ Swagger Documentation
Once the application is running, navigate to:
**`http://localhost:5265/swagger`**

Swagger provides an interactive UI to test all endpoints. 
**How to authenticate in Swagger:**
1. Call `POST /api/Auth/login` with your credentials to receive the `accessToken`.
2. Click the **Authorize** button at the top of the Swagger page.
3. ⚠️ **IMPORTANT:** Simply paste the raw token into the input field! **Do NOT write the word "Bearer" before it.** The system will handle the Bearer scheme automatically. If you type "Bearer " manually, the authentication will fail.

### 2️⃣ Postman Collection
For your convenience, a ready-to-use Postman collection is included in the repository:
📄 `Project & Task Management API.postman_collection.json`

**How to use:**
1. Open Postman.
2. Click **Import** and select the `.json` file from the repository root.
3. The collection contains all endpoints pre-configured with the correct JSON bodies.
4. Just use the Login request, copy the token, and set it in the Authorization tab as a Bearer Token for the other requests.

---

## 📁 Database Migrations

Entity Framework Core is used for managing the database schema. The migration files are located in:
`Infrastructure.Persistence/Migrations/`

**Migrations applied:**
1. `InitialCreate`: Sets up the base entities (Projects, Tasks).
2. `AddAuthentication`: Adds the User entity and Roles.
3. `SeedAdminUser`: Seeds the default admin account on database creation.
4. `AddSoftDelete`: Implements Global Query Filters for logical deletion without losing data.

If you are running the project locally without Docker, you can update your database using the .NET CLI:
```bash
dotnet ef database update --project Infrastructure.Persistence --startup-project Presentation.WebApi
```

---

## 🎯 API Endpoints Summary

**Auth**
- `POST /api/v1/Auth/register`
- `POST /api/v1/Auth/login`

**Projects**
- `GET /api/v1/Projects` (Public/User)
- `GET /api/v1/Projects/{id}` (Public/User)
- `POST /api/v1/Projects` (Admin Only)
- `PUT /api/v1/Projects/{id}` (Admin Only)
- `DELETE /api/v1/Projects/{id}` (Admin Only)

**Tasks**
- `GET /api/v1/Tasks/project/{projectId}` (Public/User)
- `POST /api/v1/Tasks` (Admin Only)
- `PATCH /api/v1/Tasks/{id}/status` (User & Admin)
- `DELETE /api/v1/Tasks/{id}` (Admin Only)
