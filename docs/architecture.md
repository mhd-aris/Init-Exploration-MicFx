# Architecture Overview

This document provides a comprehensive overview of the MicFx Orchard Core + TailwindCSS starter template architecture, including its design principles, project structure, and key components.

## 🏗️ Architecture Principles

The MicFx starter template is built on **Clean Architecture** principles with the following core concepts:

### 1. **Clean Architecture Layers**
- **Domain Layer**: Core business logic and entities
- **Application Layer**: Use cases and business workflows
- **Infrastructure Layer**: External concerns (data access, services)
- **Presentation Layer**: UI, controllers, and user interaction

### 2. **Dependency Inversion**
- High-level modules don't depend on low-level modules
- Both depend on abstractions (interfaces)
- Abstractions don't depend on details
- Details depend on abstractions

### 3. **Modular Design**
- Self-contained modules with clear boundaries
- Loose coupling between modules
- High cohesion within modules
- Plugin-based architecture via Orchard Core

### 4. **Separation of Concerns**
- Each layer has distinct responsibilities
- Cross-cutting concerns are properly isolated
- Business logic is independent of frameworks

## 📊 Clean Architecture Diagram

```
┌─────────────────────────────────────────────────────────────┐
│                    External Systems                         │
│  ┌─────────────┐  ┌─────────────┐  ┌─────────────┐        │
│  │   Browser   │  │  Database   │  │ External    │        │
│  │   Clients   │  │   Server    │  │ Services    │        │
│  └─────────────┘  └─────────────┘  └─────────────┘        │
└─────────────────────────────────────────────────────────────┘
                              │
                              ▼
┌─────────────────────────────────────────────────────────────┐
│                Infrastructure Layer                         │
│              (MicFx.Core.Infrastructure)                    │
│  ┌─────────────────────────────────────────────────────────┐ │
│  │  Data Access │  External APIs │  File System │  Email  │ │
│  │ Repositories │    Services   │   Services   │ Services │ │
│  └─────────────────────────────────────────────────────────┘ │
└─────────────────────────────────────────────────────────────┘
                              │
                              ▼
┌─────────────────────────────────────────────────────────────┐
│                 Presentation Layer                          │
│              (MicFx.Mvc.Web + Modules)                     │
│  ┌─────────────────────────────────────────────────────────┐ │
│  │ Controllers │    Views     │   API      │  TailwindCSS │ │
│  │   Actions   │   Layouts    │ Endpoints  │    Assets    │ │
│  └─────────────────────────────────────────────────────────┘ │
└─────────────────────────────────────────────────────────────┘
                              │
                              ▼
┌─────────────────────────────────────────────────────────────┐
│                Application Layer                            │
│                 (Module Services)                           │
│  ┌─────────────────────────────────────────────────────────┐ │
│  │  Use Cases  │   Services   │ Command/Query │  Handlers │ │
│  │  Workflows  │ Coordinators │   Handlers    │ Validators│ │
│  └─────────────────────────────────────────────────────────┘ │
└─────────────────────────────────────────────────────────────┘
                              │
                              ▼
┌─────────────────────────────────────────────────────────────┐
│                   Domain Layer                              │
│              (MicFx.Core.SharedKernel)                     │
│  ┌─────────────────────────────────────────────────────────┐ │
│  │  Entities   │    Value     │   Domain    │  Business   │ │
│  │   Models    │   Objects    │   Events    │    Rules    │ │
│  └─────────────────────────────────────────────────────────┘ │
└─────────────────────────────────────────────────────────────┘
                              │
                              ▼
┌─────────────────────────────────────────────────────────────┐
│                   Abstractions                              │
│              (MicFx.Core.Abstractions)                     │
│  ┌─────────────────────────────────────────────────────────┐ │
│  │ Interfaces  │  Contracts   │    DTOs     │  Constants  │ │
│  │  Services   │ Repositories │  Commands   │    Enums    │ │
│  └─────────────────────────────────────────────────────────┘ │
└─────────────────────────────────────────────────────────────┘
```

## 📁 Project Structure Analysis

### Root Level Structure
```
MicFx/
├── src/                           # Source code
│   ├── MicFx.Mvc.Web/            # Web host (Presentation)
│   ├── Core/
│   │   ├── MicFx.Core.Abstractions/    # Interfaces & contracts
│   │   ├── MicFx.Core.Infrastructure/  # Infrastructure implementations
│   │   └── MicFx.Core.SharedKernel/    # Domain layer
│   └── Modules/                   # Application features
├── tests/                         # Test projects
├── docs/                         # Documentation
├── global.json                   # .NET SDK version
└── MicFx.sln                    # Solution file
```

### Layer Mapping
```
┌─────────────────────────────────────────┐
│           Clean Architecture            │
├─────────────────────────────────────────┤
│ Presentation ← MicFx.Mvc.Web + Modules │
│ Application  ← Module Services          │
│ Domain       ← Core.SharedKernel        │
│ Infrastructure ← Core.Infrastructure    │
│ Abstractions ← Core.Abstractions        │
└─────────────────────────────────────────┘
```

## 🔧 Component Breakdown

### 1. MicFx.Mvc.Web (Presentation Layer)

**Purpose**: ASP.NET Core web application host that orchestrates the presentation layer.

**Responsibilities**:
- HTTP request/response handling
- Routing and middleware configuration
- Authentication and authorization
- Static file serving and asset pipeline
- TailwindCSS integration and compilation
- View rendering and layout management

**Key Technologies**:
- ASP.NET Core 8.0
- Orchard Core CMS
- TailwindCSS 3.4+
- Node.js build tools

**Configuration Files**:
```
MicFx.Mvc.Web/
├── Program.cs              # Application startup
├── appsettings.json        # Configuration
├── tailwind.config.js      # TailwindCSS settings
├── package.json            # Node.js dependencies
└── wwwroot/               # Static assets
```

### 2. MicFx.Core.Abstractions (Contracts Layer)

**Purpose**: Defines all interfaces and contracts used throughout the application.

**Key Components**:
- **Service Interfaces**: Contracts for application services
- **Repository Patterns**: Data access abstractions  
- **Domain Contracts**: Business rule interfaces
- **DTOs**: Data transfer objects for inter-layer communication
- **Events**: Domain and integration event definitions

**Example Interfaces**:
```csharp
// Service contracts
public interface IEmailService
{
    Task SendEmailAsync(string to, string subject, string body);
}

public interface IFileStorageService
{
    Task<string> SaveFileAsync(Stream fileStream, string fileName);
    Task<Stream> GetFileAsync(string filePath);
}

// Repository patterns
public interface IRepository<T> where T : class
{
    Task<T> GetByIdAsync(int id);
    Task<IEnumerable<T>> GetAllAsync();
    Task AddAsync(T entity);
    Task UpdateAsync(T entity);
    Task DeleteAsync(int id);
}

// Domain events
public interface IDomainEvent
{
    DateTime OccurredOn { get; }
    string EventType { get; }
}
```

### 3. MicFx.Core.Infrastructure (Infrastructure Layer)

**Purpose**: Implements infrastructure concerns and external dependencies.

**Responsibilities**:
- Database access and Entity Framework implementation
- External API integrations
- File system operations
- Email service implementations
- Caching strategies
- Logging and monitoring

**Key Components**:
```csharp
// Repository implementations
public class EntityFrameworkRepository<T> : IRepository<T> where T : class
{
    private readonly DbContext _context;
    
    public async Task<T> GetByIdAsync(int id)
    {
        return await _context.Set<T>().FindAsync(id);
    }
    // ... other implementations
}

// Service implementations
public class SmtpEmailService : IEmailService
{
    public async Task SendEmailAsync(string to, string subject, string body)
    {
        // SMTP implementation
    }
}
```

**Structure**:
```
MicFx.Core.Infrastructure/
├── Data/
│   ├── Repositories/      # Repository implementations
│   ├── Configurations/    # Entity configurations
│   └── Migrations/        # Database migrations
├── Services/
│   ├── EmailServices/     # Email implementations
│   ├── FileServices/      # File handling
│   └── ExternalApis/      # Third-party integrations
└── Extensions/            # DI registration helpers
```

### 4. MicFx.Core.SharedKernel (Domain Layer)

**Purpose**: Contains pure business logic and domain entities.

**Key Components**:
- **Entities**: Core business objects
- **Value Objects**: Immutable domain concepts
- **Domain Services**: Business logic that doesn't belong to entities
- **Business Rules**: Domain validation and constraints
- **Domain Events**: Business event definitions

**Example Domain Objects**:
```csharp
// Base entity
public abstract class BaseEntity
{
    public int Id { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    
    private List<IDomainEvent> _domainEvents = new();
    public IReadOnlyCollection<IDomainEvent> DomainEvents => _domainEvents.AsReadOnly();
    
    protected void AddDomainEvent(IDomainEvent domainEvent)
    {
        _domainEvents.Add(domainEvent);
    }
}

// Value objects
public class Email : ValueObject
{
    public string Value { get; }
    
    public Email(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException("Email cannot be empty");
            
        if (!IsValidEmail(value))
            throw new ArgumentException("Invalid email format");
            
        Value = value;
    }
    
    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}

// Domain services
public class UserDomainService
{
    public bool CanUserPerformAction(User user, string action)
    {
        // Business logic for user permissions
        return user.IsActive && user.HasPermission(action);
    }
}
```

### 5. Module System (Application Layer)

**Purpose**: Implements specific business features as self-contained modules.

**Module Structure**:
```
MicFx.Module.Example/
├── Controllers/           # HTTP endpoints
├── Views/                # UI templates
├── Services/             # Application services
├── Models/               # ViewModels and DTOs
├── Assets/               # Module-specific assets
├── Migrations/           # Module data migrations
├── Startup.cs            # Module configuration
└── Manifest.cs           # Module metadata
```

**Example Application Service**:
```csharp
public class UserManagementService
{
    private readonly IRepository<User> _userRepository;
    private readonly IEmailService _emailService;
    private readonly UserDomainService _userDomainService;
    
    public UserManagementService(
        IRepository<User> userRepository,
        IEmailService emailService,
        UserDomainService userDomainService)
    {
        _userRepository = userRepository;
        _emailService = emailService;
        _userDomainService = userDomainService;
    }
    
    public async Task<UserDto> CreateUserAsync(CreateUserCommand command)
    {
        // Application logic coordinating domain and infrastructure
        var user = new User(command.Email, command.Name);
        
        if (!_userDomainService.CanUserBeCreated(user))
            throw new BusinessRuleViolationException("User cannot be created");
            
        await _userRepository.AddAsync(user);
        await _emailService.SendWelcomeEmailAsync(user.Email.Value);
        
        return new UserDto(user);
    }
}
```

## 🔄 Data Flow Architecture

### Request Processing Flow

```
1. HTTP Request
   ↓
2. ASP.NET Core Middleware Pipeline
   ↓
3. Controller (Presentation Layer)
   ↓
4. Application Service (Application Layer)
   ↓
5. Domain Service (Domain Layer)
   ↓
6. Repository (Infrastructure Layer)
   ↓
7. Database/External Service
```

### Dependency Flow

```
Presentation Layer
    ↓ (depends on)
Application Layer
    ↓ (depends on)
Domain Layer
    ↑ (implements)
Abstractions Layer
    ↑ (implements)
Infrastructure Layer
```

## 🎯 Benefits of This Architecture

### 1. **Testability**
- Each layer can be tested in isolation
- Dependencies can be easily mocked
- Business logic is framework-independent

### 2. **Maintainability**
- Clear separation of concerns
- Changes in one layer don't affect others
- Easy to locate and modify functionality

### 3. **Scalability**
- Modules can be developed independently
- Infrastructure can be swapped without affecting business logic
- Horizontal scaling through module distribution

### 4. **Flexibility**
- Framework-agnostic business logic
- Easy to change databases or external services
- Support for multiple UI frameworks

## 🚀 Development Workflow

### Adding New Features

1. **Define Contracts** (Abstractions Layer)
   ```csharp
   public interface INewFeatureService
   {
       Task<Result> ProcessAsync(Command command);
   }
   ```

2. **Implement Domain Logic** (SharedKernel)
   ```csharp
   public class NewFeatureEntity : BaseEntity
   {
       // Domain logic and business rules
   }
   ```

3. **Create Application Services** (Module)
   ```csharp
   public class NewFeatureService : INewFeatureService
   {
       // Coordinate domain and infrastructure
   }
   ```

4. **Implement Infrastructure** (Infrastructure Layer)
   ```csharp
   public class NewFeatureRepository : IRepository<NewFeatureEntity>
   {
       // Data access implementation
   }
   ```

5. **Add Presentation Layer** (Controllers/Views)
   ```csharp
   public class NewFeatureController : Controller
   {
       // HTTP endpoints and UI coordination
   }
   ```

### Module Development Process

1. Create module using Orchard Core template
2. Define domain entities and business rules
3. Create application services for use cases
4. Implement controllers for user interaction
5. Add views with TailwindCSS styling
6. Write comprehensive tests
7. Register services in module startup

## 📚 Technology Stack Summary

| Layer | Technologies |
|-------|-------------|
| **Presentation** | ASP.NET Core, Orchard Core, TailwindCSS, Razor Views |
| **Application** | C# Services, Dependency Injection, MediatR (optional) |
| **Domain** | C# Entities, Value Objects, Domain Services |
| **Infrastructure** | Entity Framework, External APIs, File System |
| **Cross-Cutting** | Logging, Caching, Authentication, Validation |

This architecture ensures a robust, maintainable, and scalable foundation for building modern web applications with Orchard Core and TailwindCSS. 