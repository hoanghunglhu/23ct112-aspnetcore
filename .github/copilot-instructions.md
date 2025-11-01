# Copilot Instructions for 23ct112-aspnetcore

## Project Overview
This is an ASP.NET Core Web API project targeting .NET 6.0. It uses Entity Framework Core for data access and SQL Server as the database. The main domain is user management, with CRUD operations exposed via RESTful endpoints.

## Architecture & Key Components
- **Controllers/**: Contains API controllers (e.g., `UserController.cs`) that handle HTTP requests and responses.
- **Entity/**: Contains data models (`User.cs`), database context (`AppDbContext.cs`), and SQL schema (`Database.sql`).
- **Models/**: Contains DTOs for API requests/responses (e.g., `UserModel.cs`).
- **Program.cs**: Configures services, middleware, and endpoint routing. Registers `AppDbContext` with SQL Server using connection string from configuration.
- **Properties/launchSettings.json**: Defines launch profiles for local development (http/https/IIS Express) and sets environment variables.
- **LearnAspNetCore.csproj**: Project file with dependencies (EF Core, Swashbuckle for Swagger UI).

## Data Flow
- API requests are routed to controllers (e.g., `UserController`).
- Controllers use `AppDbContext` to query/update the database via EF Core.
- Data models (`User`) map to database tables; DTOs (`UserModel`) are used for input/output.

## Developer Workflows
- **Run/Debug**: Use `dotnet run` or launch via Visual Studio/VS Code. Swagger UI is enabled in Development mode for API testing.
- **Database Setup**: Use `Database.sql` to create/update SQL Server schema. Connection string must be set in `appsettings.json` (not present in repo, must be added).
- **Migrations**: Use EF Core CLI (`dotnet ef migrations add <name>` and `dotnet ef database update`) for schema changes.
- **Testing Endpoints**: Use Swagger UI (`/swagger`) or tools like Postman. Example endpoints: `GET /api/user`, `POST /api/user`.

## Conventions & Patterns
- **RESTful Routing**: `[Route("api/[controller]")]` and attribute-based routing in controllers.
- **Dependency Injection**: DbContext injected into controllers via constructor.
- **DTO Usage**: Separate DTOs for API input/output (`UserModel`) vs. database entities (`User`).
- **Swagger**: Always enabled in Development for API exploration.
- **Environment Variables**: Set via `launchSettings.json` for local development.

## Integration Points
- **EF Core**: Handles ORM and migrations.
- **SQL Server**: Main database, schema defined in `Database.sql`.
- **Swagger (Swashbuckle)**: Auto-generates API docs/UI.

## Examples
- To add a new user: `POST /api/user` with JSON body matching `UserModel`.
- To fetch all users: `GET /api/user`.

## Key Files
- `Controllers/UserController.cs`: Main API logic.
- `Entity/AppDbContext.cs`: EF Core context.
- `Entity/User.cs`: User entity.
- `Models/UserModel.cs`: DTO for user operations.
- `Program.cs`: App startup/configuration.
- `Entity/Database.sql`: SQL schema.

## Notes
- Connection string must be provided in `appsettings.json` for database access.
- No tests or custom build scripts detected; standard .NET workflows apply.

---
For questions or missing details, review the above files or ask for clarification.
