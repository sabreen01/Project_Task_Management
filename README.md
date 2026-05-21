# Project Task Management API 🚀

A modern, robust, and scalable Task Management RESTful API built with **.NET 10**, following **Clean Architecture** principles and the **CQRS** pattern.

## 🌟 Key Features

- **Clean Architecture:** Strict separation of concerns (Domain, Application, Infrastructure, Presentation).
- **CQRS Pattern:** Segregation of Commands and Queries using **MediatR**.
- **Robust Validation:** Input validation using **FluentValidation** pipeline behaviors.
- **Custom Authentication & Authorization:** 
  - Stateless JWT (JSON Web Tokens).
  - Passwords hashed securely using **BCrypt**.
  - Role-Based Access Control (Admin vs. Standard User).
- **Dockerized Environment:** One command to spin up the API and a SQL Server database via `docker-compose`.
- **Entity Framework Core:** Code-first migrations with automatic database seeding.
- **Pagination:** Built-in pagination for fetching Projects and Tasks.

## 🛠️ Technology Stack

- **Framework:** .NET 10 (ASP.NET Core Web API)
- **Database:** Microsoft SQL Server (Dockerized)
- **ORM:** Entity Framework Core
- **Libraries:** MediatR, FluentValidation, BCrypt.Net-Next, Mapster/AutoMapper
- **Containerization:** Docker & Docker Compose

## 🚀 Getting Started

The easiest way to run the application is using Docker.

### Prerequisites
- [Docker](https://www.docker.com/products/docker-desktop) and Docker Compose installed on your machine.

### Run via Docker Compose

1. Clone or download the repository.
2. Navigate to the root directory where `compose.yaml` is located.
3. Run the following command:
   ```bash
   docker compose up -d --build
   ```
4. The API will be available at: `http://localhost:5265`

*Note: On first startup, the API container will automatically apply EF Core Migrations and create the database schemas.*

## 🔐 Authentication & Roles

The system uses a Custom Identity implementation with two roles: `Admin` (1) and `User` (2).

### Default Seeded Admin
When the database is created, a default **Admin** user is seeded into the database:
- **Email:** `admin@system.com`
- **Password:** `Admin123!`

*Use these credentials in the `/api/Auth/login` endpoint to obtain an Admin JWT Token.*

### Access Control Rules
- **Admin:** Can Create, Update, and Delete Projects and Tasks.
- **User:** Can view Projects and Tasks, and update the Status of a Task. Cannot create or delete projects/tasks.
- **Registration:** New users who sign up via `/api/Auth/register` are assigned the `User` role by default to prevent privilege escalation.

## 📝 API Endpoints Overview

You can test the APIs using tools like Postman. 

**Auth:**
- `POST /api/Auth/register` - Register a new standard user.
- `POST /api/Auth/login` - Authenticate and get a JWT token.

**Projects (Admin Only for Mutations):**
- `GET /api/Projects` - Get all projects (Paginated).
- `GET /api/Projects/{id}` - Get project by ID.
- `POST /api/Projects` - Create a new project (Admin).
- `PUT /api/Projects/{id}` - Update a project (Admin).
- `DELETE /api/Projects/{id}` - Delete a project (Admin).

**Tasks (Admin Only for Mutations, Users can Update Status):**
- `GET /api/Tasks/project/{projectId}` - Get tasks for a specific project (Paginated).
- `POST /api/Tasks` - Create a task in a project (Admin).
- `PATCH /api/Tasks/{id}/status` - Update the status of a task (User & Admin).
- `DELETE /api/Tasks/{id}` - Delete a task (Admin).

*(Note: Pass the JWT token as a `Bearer Token` in the Authorization header for all endpoints except register/login).*
