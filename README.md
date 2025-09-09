# TaskManager

This project provides a robust API for managing user tasks with features such as task creation, retrieval, and completion.  
It follows clean architecture principles, implements global exception handling, structured logging, and supports claims-based authentication for secure user-specific operations.  
The system is designed for maintainability, observability, and scalability.
The system is designed for maintainability, observability, and scalability.

---

## Setup Instructions

1. **Install prerequisites**  
   - .NET SDK 8.0 or later  
   - Optional: Visual Studio 2022 / JetBrains Rider / VS Code with C# extension  
   - SQL Server or another supported database provider

2. **Clone the repository**
   ```bash
   git clone https://github.com/cotuk3/TaskManager.git
   cd task-manager
  

3. **Restore NuGet packages**
   ```bash
   dotnet restore

4. **Build the solution**
   ```bash
   dotnet build

5. **Apply database migrations**
   ```bash
   dotnet ef database update

6. **Run the API**
   ```bash
   dotnet run --project TaskManager.Api

7. **Run unit tests**
   ```bash
   dotnet test

---

## Known Limitations & Edge Cases Not Handled

- No built-in pagination for large task lists.
- No recurring task scheduling.
- No soft-delete mechanism — deleted tasks are removed permanently.
- Due date rules are fixed in code; no configuration for custom rules.
- No concurrency conflict resolution — last update wins.
- Limited filtering/sorting options for task queries.
