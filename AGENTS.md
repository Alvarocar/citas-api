# AGENTS.md

Guide for agentic coding agents operating in this repository.

## Project Overview

This is a Clean Architecture monolith for an appointments/booking system built with .NET 9 and C# 13.

### Solution Structure

```
citas-api/
├── Citas.Domain/           # Entities, value objects, repository interfaces, exceptions
├── Citas.Application/      # Services, DTOs, factories, strategies, business logic
├── Citas.Infrastructure/   # EF Core, repository implementations, DI setup, external services
├── WebApi/                 # Controllers, middleware, filters, API configuration
├── Genkey/                 # Utility for generating JWT signing keys
└── docker-compose.yml      # Local PostgreSQL database
```

### Dependency Flow

```
WebApi → Infrastructure → Application → Domain
```

## Build Commands

```bash
# Build the solution
dotnet build

# Build in Release mode
dotnet build --configuration Release

# Run the API (from root)
dotnet run --project WebApi

# Run with hot reload (development)
dotnet watch --project WebApi
```

## Database Commands

```bash
# Start local PostgreSQL database
docker-compose up -d db

# Create a new migration (after model changes)
dotnet ef migrations add MigrationName --project Citas.Infrastructure --startup-project WebApi

# Apply migrations
dotnet ef database update --project Citas.Infrastructure --startup-project WebApi

# Rollback to specific migration
dotnet ef database update PreviousMigrationName --project Citas.Infrastructure --startup-project WebApi
```

## Testing

**Note:** No test project exists yet. When creating one:

```bash
# Create test project (recommended location)
dotnet new xunit -o Citas.Tests

# Run all tests
dotnet test

# Run specific test class
dotnet test --filter "FullyQualifiedName~ClassName"

# Run specific test method
dotnet test --filter "FullyQualifiedName~ClassName.MethodName"
```

## Prerequisites Before Running

1. Install .NET 9 SDK
2. Generate JWT signing key: `dotnet run --project Genkey`
3. Trust HTTPS certificate: `dotnet dev-certs https --trust`
4. Start database: `docker-compose up -d db`

## Code Style Guidelines

### Formatting

- Use spaces, not tabs (configure editor to show 2-space indentation visually)
- Opening braces on same line for most constructs
- No blank lines between method declarations in classes with few methods

### Imports

- `ImplicitUsings` is enabled - no need for common `System.*` imports
- Order imports alphabetically
- Place namespace imports before namespace declaration (file-scoped namespaces preferred for new files)

### Primary Constructors for Dependency Injection

Use primary constructors for constructor injection:

```csharp
public class EmployeeService(
    IEmployeeRepository _employeeRepository,
    IRolRepository _rolRepository,
    IUnitOfWork _unitOfWork
)
{
    async public Task<Employee> GetById(int id, CancellationToken ct)
    {
        // _employeeRepository is available directly
    }
}
```

### Async Methods

Always include `CancellationToken` in async public methods:

```csharp
async public Task<Employee> GetById(int id, CancellationToken ct)
```

### Nullable Reference Types

Nullable reference types are enabled (`<Nullable>enable</Nullable>`):

- Use `string?` for nullable strings
- Use `required` keyword for non-nullable properties that must be set:
  ```csharp
  public required string FirstName { get; set; }
  ```
- Use `null!` for EF navigation properties that will be populated:
  ```csharp
  public DbSet<Employee> Employees { get; set; } = null!;
  ```

### Naming Conventions

| Element | Convention | Example |
|---------|------------|---------|
| Classes | PascalCase | `EmployeeService` |
| Methods | PascalCase | `GetById` |
| Properties | PascalCase | `FirstName` |
| Parameters | camelCase | `employeeId` |
| Private fields | Underscore prefix | `_employeeRepository` |
| Interfaces | I prefix | `IEmployeeRepository` |
| DTOs | Suffix Dto | `EmployeeCreateDto` |
| Exceptions | Suffix Exception | `NotFoundException` |

### Database Naming

Database uses snake_case. Entity configurations map between C# PascalCase and DB snake_case:

```csharp
builder.Property(x => x.FirstName)
    .HasColumnName("first_name");
```

### No Code Comments

Do not add comments to code. Write self-documenting code with clear naming.

Exception: XML documentation on public APIs that explain business rules:

```csharp
/// <exception cref="ForbiddenException">
/// The role of the employee cannot be created by the current user.
/// </exception>
```

### Language for User-Facing Messages

Use Spanish for user-facing error messages:

```csharp
throw new NotFoundException("Empleado no encontrado");
```

## Architecture Patterns

### Repository Pattern

- Generic repository: `IRepository<T, TKey>` in Domain
- Specific repositories extend generic interface in Domain
- Implementations in Infrastructure/Persistence/Repositories
- Use `AsNoTracking()` for read queries

### Unit of Work

For operations requiring transaction:

```csharp
using (_unitOfWork)
{
    await _unitOfWork.BeginTransactionAsync(ct);
    // ... operations ...
    await _unitOfWork.SaveChangesAsync(ct);
    await _unitOfWork.CommitTransactionAsync(ct);
}
```

### Factory Pattern

Use factories for complex entity creation:

```csharp
public interface IEmployeeFactory
{
    Employee Create(EmployeeCreateDto dto, Rol rol, Company company);
    UserTokenDto CreateToken(Employee employee);
}
```

### Strategy Pattern

Use strategies for role-based business logic variations:

```csharp
public interface IEmployeeCreateOneStrategy
{
    Task<Employee> ExecuteAsync(EmployeeCreateDto dto, CancellationToken ct);
}
```

## Error Handling

### Exception Hierarchy

```
CitasException (base)
├── NotFoundException (404)
├── AlreadyExistException (409)
├── ForbiddenException (403)
├── NotAuthorizedException (401)
├── ReservationAssignedException (409)
└── CitasInternalException (500)
```

### Creating New Exceptions

1. Inherit from `CitasException`
2. Add to `ErrorsMiddleware` switch for HTTP status mapping
3. Add message to `Citas.Domain/Resources/Exceptions.resx`

### Controller Error Handling

Do not wrap service calls in try-catch. Let exceptions propagate to `ErrorsMiddleware`.

## DTOs and Validation

Use Data Annotations for validation:

```csharp
public class EmployeeCreateDto
{
    [Required(ErrorMessageResourceType = typeof(Resources.Validations), 
              ErrorMessageResourceName = "Required")]
    public required string Firstname { get; set; }
    
    [EmailAddress(ErrorMessageResourceType = typeof(Resources.Validations), 
                  ErrorMessageResourceName = "EmailAddress")]
    public string? Email { get; set; }
}
```

Store validation messages in `Citas.Application/Resources/Validations.resx`.

## Entity Framework Core

### Migrations

- Migrations live in `Citas.Infrastructure/Migrations/`
- Entity configurations in `Citas.Infrastructure/Persistence/Configurations/`
- Use `internal` class for configurations

### PostgreSQL Enums

Register enums in `DatabaseSetup.cs`:

```csharp
dataSourceBuilder.MapEnum<EReservationState>();
o.MapEnum<EReservationState>("enum__reservation_state");
```

## API Controllers

- Inherit from `BaseController` for authentication helpers
- Use `[Authorize(Roles = "...")]` for role-based authorization
- Return `CancellationToken ct` from endpoints for cancellation support
- Use `Created()`, `Ok()`, `NoContent()`, `NotFound()` for responses

## Configuration

Connection strings and secrets stored in:
- Development: `appsettings.Development.json` and User Secrets
- Production: Environment variables

Run `dotnet user-secrets init --project WebApi` if not initialized.
