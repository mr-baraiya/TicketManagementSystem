# Ticket Management System - Database Setup Guide

This guide provides step-by-step instructions to set up the database and integrate it with the Ticket Management System API using Entity Framework Core.

## Prerequisites
- .NET 8 SDK installed
- SQL Server instance (e.g., SQL Server Express)
- Visual Studio or VS Code

## 1. Install Required NuGet Packages
Navigate to the API project directory and run the following commands:

```
dotnet add package Microsoft.EntityFrameworkCore.SqlServer --version 8.0.8
```
```
dotnet add package Microsoft.EntityFrameworkCore.Tools --version 8.0.8
```
```
dotnet add package Microsoft.EntityFrameworkCore.Design --version 8.0.8
```
```
dotnet add package Microsoft.AspNetCore.Authentication.JwtBearer --version 8.0.8
```
```
dotnet add package FluentValidation.AspNetCore --version 11.3.0
```
```
dotnet add package BCrypt.Net-Next
```

## 2. Prepare the Database
- Ensure your SQL Server is running.
- Create a database named `TicketManagementSystem` (or use the provided SQL script in `Database/SQLQuery.sql`).

## 3. Scaffold the Database Context and Models
Run the following command in the API project directory to generate models and DbContext from your database:

```
dotnet ef dbcontext scaffold "Server=DESKTOP-VISHAL\SQLEXPRESS;Database=TicketManagementSystem;Trusted_Connection=True;TrustServerCertificate=True;" Microsoft.EntityFrameworkCore.SqlServer -o Models -f
```
- This will create entity classes and a DbContext in the `Models` folder.
- The `-f` flag forces overwrite if models already exist.

## 4. Update Connection String
- Move your connection string to `appsettings.json` for security.
- Example:

```
"ConnectionStrings": {
  "DefaultConnection": "Server=server_name;Database=database_name;Trusted_Connection=True;TrustServerCertificate=True;"
}
```
- Update `Program.cs` or `Startup.cs` to use the connection string from configuration.

## 5. Run the Application
Start the API:
```
dotnet run
```
