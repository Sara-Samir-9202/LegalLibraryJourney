# 📚 Legal Library- Web API

The backend solution for the Legal Library Management System, built with **.NET 9**. This project provides a robust, layered Web API to serve Web (React/Angular) and Mobile (iOS/Android) client applications.

---

## 🏗️ System Architecture Design

The project is structured following **Clean Architecture** principles to ensure separation of concerns, maintainability, and scalability.

[Architecture Design]

<img width="1024" height="569" alt="image" src="https://github.com/user-attachments/assets/42f4f1d9-1a21-405b-a89f-997f1ad18462" />

> *The diagram above illustrates the complete data flow from the Client Layer to the Microsoft SQL Server database.*

---

## 🛠️ Project Structure & Solution Organization

The solution is neatly organized into three primary layers, reflecting the architectural diagram:

### 1. LegalLibrary.API (Presentation Layer)
*   Responsible for the entry point of the application (`Program.cs`).
*   Contains the **API Controllers** (`AuthController`, `BooksController`, `CategoriesController`, `FilesController`).
*   Manages user authentication with **JWT**.
*   Handles app configuration (`appsettings.json`) and Middleware.

### 2. LegalLibrary.Core (Domain & Application Layer)
*   The heart of the system.
*   Contains the database **Entities** (`AspNetUsers`, `Books`, `Categories`, `BookDownloads`).
*   Defines **DTOs** (Data Transfer Objects) and **Helpers**.
*   Hosts the **Interfaces** for Services and Repositories.

### 3. LegalLibrary.Infrastructure (Data Access & Services Layer)
*   Handles all external concerns and data access.
*   Contains the **Entity Framework Core Context** and database **Migrations**.
*   Implements the Repository pattern (`GenericRepository`, `BookRepository`, etc.).
*   Provides the concrete implementation for Business Logic **Services** (`AuthService`, `BookService`, `CategoryService`, `FileService`).
*   Manages interaction with the **File System** (PDF storage and Image upload).

---

## 🚀 API Documentation & Interactive UI

The API is fully documented using **Swagger / OpenAPI**, allowing for easy exploration and testing of all endpoints directly from the browser.

[Swagger UI Preview]
<img width="922" height="896" alt="image" src="https://github.com/user-attachments/assets/2ae2f165-e638-4ea4-ae91-b55888e49517" />


---

## 💻 Technology Stack

*   **Framework:** .NET 9 (ASP.NET Core Web API)
*   **Database ORM:** Entity Framework Core 9 (EF Core 9)
*   **Database:** Microsoft SQL Server
*   **Authentication:** JWT (JSON Web Tokens) with a built-in JWT Token Generator.
*   **Documentation:** Swagger / OpenAPI

---

## 🗄️ Database Schema & Tables

*   `AspNetUsers`: Manages user and admin accounts.
*   `Books`: Stores information about legal books (titles, descriptions, file paths).
*   `Categories`: Organizes books into specific legal categories.
*   `BookDownloads`: Tracks book download activities.

---

## 🏁 Getting Started

### Prerequisites

*   .NET 9 SDK
*   Microsoft SQL Server
*   An IDE like Visual Studio 2022 or VS Code

### Installation & Setup

1.  **Clone** the project to your local machine.
2.  Navigate to the `LegalLibrary.API` folder and locate `appsettings.json`.
3.  Update the `DefaultConnection` string with your SQL Server credentials.
4.  Open the **Package Manager Console** and run the following command to create the database:
    
```bash
    Update-Database
    ```
5.  **Run** the project.
6.  Navigate to `/swagger` in your browser to access the API documentation.
