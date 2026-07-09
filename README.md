<p align="center">
  <img src="Assets/banner.png" alt="Team Task Management Banner" width="100%">
</p>

# 🚀 Team Task Management System

A full-stack Team Task Management System built with **ASP.NET Core Web API** and **Angular**, following **Clean Architecture** principles.

The system helps organizations manage teams, projects, tasks, and collaboration between members through a secure role-based platform.

---

## ✨ Features

### 🔐 Authentication & Authorization

- JWT Authentication
- Secure Login & Registration
- Role-Based Authorization
- Roles:
  - Admin
  - Manager
  - Member

---

### 👥 Team Management

- Create Teams
- Update Teams
- Delete Teams
- View Teams
- Add Members to Teams

---

### 📁 Project Management

- Create Projects
- Update Projects
- Delete Projects
- View Project Details
- Assign Projects to Teams
- Project Status Management

---

### ✅ Task Management

- Create Tasks
- Update Tasks
- Delete Tasks
- Assign Tasks to Team Members
- Task Priorities
- Task Status
- Task Tracking

---

### 💬 Comment System

- Add Comments
- View Comments
- Delete Comments

---

### 📊 Dashboard

- Total Teams
- Total Projects
- Tasks In Progress
- Completed Tasks
- Recent Projects
- Recent Tasks

---

## 🛠 Tech Stack

### Backend

- ASP.NET Core Web API
- Entity Framework Core
- SQL Server
- ASP.NET Identity
- JWT Authentication
- AutoMapper
- FluentValidation
- Repository Pattern
- Unit Of Work
- Clean Architecture

### Frontend

- Angular
- TypeScript
- SCSS
- Angular Router
- HttpClient
- Route Guards
- HTTP Interceptors

---

## 🏛 Architecture

The project follows **Clean Architecture**:

```
Presentation (Angular)

↓

Web API

↓

Application Layer

↓

Domain Layer

↓

Infrastructure Layer

↓

SQL Server
```

---

## 📂 Project Structure

```
Team-Task-Management

├── Backend
│   ├── TeamTaskManager.API
│   ├── TeamTaskManager.Application
│   ├── TeamTaskManager.Domain
│   └── TeamTaskManager.Infrastructure
│
├── Frontend
│
└── README.md
```

---

## 🚀 Running the Project

### Backend

```bash
Update-Database

dotnet run
```

### Frontend

```bash
npm install

ng serve
```

---

## 📸 Screenshots

Screenshots will be added soon.

---

## 👨‍💻 Author

**Abdelfatah Ahmed**

Computer Science & Artificial Intelligence Graduate

Full Stack .NET Developer