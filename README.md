# Task Manager API (APBD Test)

REST API for managing team tasks and projects using **ASP.NET Core** and **ADO.NET** with parameterized SQL queries.

## 🚀 Overview

This project demonstrates:
- Fetching a team member’s assigned and created tasks
- Deleting a project along with all related tasks in a safe transaction

## 🔧 Tech Stack

- ASP.NET Core 8
- ADO.NET (SqlConnection, SqlCommand)
- SQL Server
- Swagger UI for testing endpoints

## 📁 Key Features

- `GET /api/tasks/{id}`  
  → Returns team member info and their tasks (assigned + created)

- `DELETE /api/tasks/{id}`  
  → Deletes a project and all its tasks within a transaction

## 📂 Folder Structure

- `Controllers/` – API endpoints
- `Services/` – Business logic using ADO.NET
- `Contracts/` – DTOs for requests and responses
- `appsettings.Development.json` – DB config

---

Built as part of an academic assignment to demonstrate low-level data access and clean architecture.
