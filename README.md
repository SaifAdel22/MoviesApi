# 🎬 Movies API

![.NET](https://img.shields.io/badge/.NET-8-blue)
![API](https://img.shields.io/badge/REST-API-green)
![EF Core](https://img.shields.io/badge/EntityFramework-Core-purple)
![Swagger](https://img.shields.io/badge/API-Documentation-orange)

A **RESTful API built with ASP.NET Core** for managing movie data with authentication, filtering, pagination, and secure endpoints.

This project demonstrates **modern backend development practices** including API design, authentication, and database optimization.

---

# 🚀 Features

✅ RESTful API architecture  
✅ JWT authentication  
✅ CRUD operations for movie resources  
✅ Advanced filtering and search  
✅ Pagination for large datasets  
✅ Input validation and error handling  
✅ Interactive API documentation with Swagger

---

# 🏗 API Architecture


Client (Frontend / Postman)
│
▼
ASP.NET Core Web API
│
▼
Service Layer
│
▼
Repository Pattern
│
▼
Entity Framework Core
│
▼
SQL Server Database


---

# 🛠 Tech Stack

## Backend
- ASP.NET Core 8
- C#

## Database
- SQL Server
- Entity Framework Core
- LINQ

## API Tools
- Swagger / OpenAPI
- Postman

---

# 📂 Project Structure


MoviesAPI
│
├── Controllers
├── DTOs
├── Services
├── Repositories
├── Data
└── Models
---

# 🔑 Authentication

The API uses **JWT Bearer Tokens**.




---

# 📚 API Endpoints

### Get all movies


GET /api/movies


### Get movie by id


GET /api/movies/{id}


### Create movie


POST /api/movies


### Update movie


PUT /api/movies/{id}


### Delete movie


DELETE /api/movies/{id}


---


# 👨‍💻 Author

**Saif Younis**

Backend Developer  
ASP.NET Core | REST APIs | AI Systems
