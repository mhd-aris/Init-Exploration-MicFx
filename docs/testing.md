# Testing Guide

This guide covers the testing strategy and implementation for the MicFx Orchard Core + TailwindCSS starter template.

## 🧪 Testing Strategy

### Testing Pyramid

The MicFx project follows the testing pyramid approach:

```
        ┌─────────────────┐
        │   E2E Tests     │  ← Few, Slow, Expensive
        │   (Browser)     │
        └─────────────────┘
       ┌───────────────────┐
       │ Integration Tests │  ← Some, Medium Speed
       │  (API, HTTP)      │
       └───────────────────┘
     ┌─────────────────────────┐
     │     Unit Tests          │  ← Many, Fast, Cheap
     │  (Business Logic)       │
     └─────────────────────────┘
```

### Test Projects Structure

```
tests/
├── MicFx.Tests.Unit/              # Unit tests for business logic and services
│   └── Modules/
│       └── HelloWorld/
│           └── Services/
├── MicFx.Tests.Integration/        # Integration tests for HTTP endpoints
│   └── Controllers/
└── Modules/
    └── MicFx.Module.HelloWorld.Tests/  # Module-specific tests
        └── Controllers/
```

## 🏗️ Architecture & Testing Layers

### Clean Architecture Testing Strategy

The MicFx template follows Clean Architecture principles, and our testing strategy aligns with each layer:

```
┌─────────────────────────────────────────┐
│         Presentation Layer              │
│    (Controllers, Views, HTTP)           │  ← Integration Tests
├─────────────────────────────────────────┤
│        Application Layer                │
│      (Module Services)                  │  ← Unit Tests
├─────────────────────────────────────────┤
│          Domain Layer                   │
│     (Core.SharedKernel)                │  ← Unit Tests
├─────────────────────────────────────────┤
│       Infrastructure Layer             │
│    (Core.Infrastructure)               │  ← Integration Tests
└─────────────────────────────────────────┘
```

### Testing Focus Areas

- **Domain Layer**: Pure business logic, entities, value objects
- **Application Layer**: Services, use cases, business workflows
- **Infrastructure Layer**: Data access, external service integrations
- **Presentation Layer**: Controllers, API endpoints, HTTP responses

## 🔧 Test Project Configuration

### Dependencies

All test projects use the following packages:

- **xUnit** - Testing framework
- **Moq** - Mocking framework for unit tests
- **Microsoft.AspNetCore.Mvc.Testing** - Integration testing support
- **coverlet.collector** - Code coverage analysis

### Project File Template

```xml
<Project Sdk="Microsoft.NET.Sdk">
  <PropertyGroup>
    <TargetFramework>net8.0</TargetFramework>
    <ImplicitUsings>enable</ImplicitUsings>
    <Nullable>enable</Nullable>
    <IsPackable>false</IsPackable>
    <IsTestProject>true</IsTestProject>
  </PropertyGroup>

  <ItemGroup>
    <PackageReference Include="coverlet.collector" Version="6.0.0" />
    <PackageReference Include="Microsoft.NET.Test.Sdk" Version="17.8.0" />
    <PackageReference Include="Moq" Version="4.20.69" />
    <PackageReference Include="xunit" Version="2.5.3" />
    <PackageReference Include="xunit.runner.visualstudio" Version="2.5.3" />
  </ItemGroup>

  <ItemGroup>
    <Using Include="Xunit" />
  </ItemGroup>
</Project>
```

## 🚀 Running Tests

### Command Line Options

```bash
# Run all tests
dotnet test

# Run specific test project
dotnet test tests/MicFx.Tests.Unit

# Run tests with code coverage
dotnet test --collect:"XPlat Code Coverage"

# Run tests in watch mode (for TDD)
dotnet watch test tests/MicFx.Tests.Unit

# Run with detailed output
dotnet test --verbosity normal

# Run specific test method
dotnet test --filter "TestMethodName"

# Run tests for specific namespace
dotnet test --filter "FullyQualifiedName~MicFx.Tests.Unit.Modules.HelloWorld"
```

### Development Workflow

```bash
# Terminal 1: Watch tests
dotnet watch test tests/MicFx.Tests.Unit

# Terminal 2: Watch application
dotnet watch run --project src/MicFx.Mvc.Web

# Terminal 3: Watch TailwindCSS
npm run tailwind:watch
```

## 📝 Unit Testing

### Testing Services (Application Layer)

```csharp
using MicFx.Module.HelloWorld.Services;

namespace MicFx.Tests.Unit.Modules.HelloWorld.Services;

public class GreeterServiceTests
{
    private readonly GreeterService _greeterService;

    public GreeterServiceTests()
    {
        _greeterService = new GreeterService();
    }

    [Fact]
    public void Greet_WithValidName_ReturnsExpectedGreeting()
    {
        // Arrange
        var name = "MicFx";
        var expectedGreeting = "Hello, MicFx!";

        // Act
        var result = _greeterService.Greet(name);

        // Assert
        Assert.Equal(expectedGreeting, result);
    }

    [Theory]
    [InlineData("John", "Hello, John!")]
    [InlineData("Alice", "Hello, Alice!")]
    [InlineData("World", "Hello, World!")]
    public void Greet_WithDifferentNames_ReturnsCorrectGreeting(string name, string expected)
    {
        // Act
        var result = _greeterService.Greet(name);

        // Assert
        Assert.Equal(expected, result);
    }

    [Fact]
    public void Greet_WithNull_ReturnsGreetingWithEmptyName()
    {
        // Arrange
        string name = null!;
        var expectedGreeting = "Hello, !";

        // Act
        var result = _greeterService.Greet(name);

        // Assert
        Assert.Equal(expectedGreeting, result);
    }
}
```

### Testing Controllers (Presentation Layer)

```csharp
using Microsoft.AspNetCore.Mvc;
using MicFx.Module.HelloWorld.Controllers;

namespace MicFx.Module.HelloWorld.Tests.Controllers;

public class HelloWorldControllerTests
{
    private readonly HelloWorldController _controller;

    public HelloWorldControllerTests()
    {
        _controller = new HelloWorldController();
    }

    [Fact]
    public void Index_ReturnsViewResult()
    {
        // Act
        var result = _controller.Index();

        // Assert
        Assert.IsType<ViewResult>(result);
    }

    [Fact]
    public void Test_ReturnsJsonResult()
    {
        // Act
        var result = _controller.Test();

        // Assert
        var jsonResult = Assert.IsType<JsonResult>(result);
        var value = jsonResult.Value;
        
        // Verify the anonymous object structure
        var messageProperty = value?.GetType().GetProperty("message");
        Assert.NotNull(messageProperty);
        Assert.Equal("Hello from MicFx!", messageProperty.GetValue(value));
    }
}
```

### Testing with Dependencies (Mocking)

```csharp
using Moq;
using MicFx.Core.Abstractions.Services;

public class UserServiceTests
{
    private readonly Mock<IUserRepository> _mockRepository;
    private readonly UserService _userService;

    public UserServiceTests()
    {
        _mockRepository = new Mock<IUserRepository>();
        _userService = new UserService(_mockRepository.Object);
    }

    [Fact]
    public async Task GetUserAsync_WithValidId_ReturnsUser()
    {
        // Arrange
        var userId = 1;
        var expectedUser = new User { Id = userId, Name = "Test User" };
        _mockRepository.Setup(r => r.GetByIdAsync(userId))
                      .ReturnsAsync(expectedUser);

        // Act
        var result = await _userService.GetUserAsync(userId);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(expectedUser.Id, result.Id);
        Assert.Equal(expectedUser.Name, result.Name);
        _mockRepository.Verify(r => r.GetByIdAsync(userId), Times.Once);
    }
}
```

## 🌐 Integration Testing

### HTTP Endpoint Testing

```csharp
using Microsoft.AspNetCore.Mvc.Testing;
using System.Net;
using System.Text.Json;

namespace MicFx.Tests.Integration.Controllers;

public class HelloWorldControllerTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient _client;

    public HelloWorldControllerTests(WebApplicationFactory<Program> factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task Get_RootPath_ReturnsSuccessOrRedirect()
    {
        // Act
        var response = await _client.GetAsync("/");

        // Assert - Orchard Core may redirect to setup page
        Assert.True(response.IsSuccessStatusCode || response.StatusCode == HttpStatusCode.Redirect);
    }

    [Fact]
    public async Task Get_HelloWorldTest_ReturnsJsonResponse()
    {
        // Act
        var response = await _client.GetAsync("/Hello/test");
        var content = await response.Content.ReadAsStringAsync();

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.Contains("application/json", response.Content.Headers.ContentType?.ToString());
        
        // Verify JSON structure
        var jsonDoc = JsonDocument.Parse(content);
        Assert.True(jsonDoc.RootElement.TryGetProperty("message", out var messageProperty));
        Assert.Equal("Hello from MicFx!", messageProperty.GetString());
    }
}
```

### Testing Infrastructure Layer

```csharp
using Microsoft.EntityFrameworkCore;
using MicFx.Core.Infrastructure.Data;

public class RepositoryIntegrationTests : IDisposable
{
    private readonly ApplicationDbContext _context;
    private readonly UserRepository _repository;

    public RepositoryIntegrationTests()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        _context = new ApplicationDbContext(options);
        _repository = new UserRepository(_context);
    }

    [Fact]
    public async Task CreateUser_WithValidData_SavesToDatabase()
    {
        // Arrange
        var user = new User { Name = "Test User", Email = "test@example.com" };

        // Act
        await _repository.CreateAsync(user);
        await _context.SaveChangesAsync();

        // Assert
        var savedUser = await _context.Users.FirstOrDefaultAsync(u => u.Email == "test@example.com");
        Assert.NotNull(savedUser);
        Assert.Equal("Test User", savedUser.Name);
    }

    public void Dispose()
    {
        _context.Dispose();
    }
}
```

## 🎯 Testing Best Practices

### Unit Testing Guidelines

1. **Follow AAA Pattern**: Arrange, Act, Assert
2. **One Assertion Per Test**: Focus on single behavior
3. **Descriptive Test Names**: Clearly describe what is being tested
4. **Independent Tests**: No dependencies between tests
5. **Fast Execution**: Unit tests should run quickly

### Integration Testing Guidelines

1. **Test Real Scenarios**: Use realistic data and workflows
2. **Minimal Mocking**: Test actual integrations where possible
3. **Database Isolation**: Use in-memory or test databases
4. **Environment Consistency**: Ensure repeatable test environments

### Orchard Core Specific Testing

1. **Module Testing**: Test modules in isolation when possible
2. **Content Type Testing**: Use content items for complex scenarios
3. **Service Resolution**: Test dependency injection configuration
4. **Routing Testing**: Verify module routes work correctly

## 📊 Code Coverage

### Generating Coverage Reports

```bash
# Install coverage tools globally
dotnet tool install --global dotnet-reportgenerator-globaltool

# Run tests with coverage collection
dotnet test --collect:"XPlat Code Coverage"

# Generate HTML coverage report
reportgenerator "-reports:tests/**/coverage.cobertura.xml" "-targetdir:coverage-report" "-reporttypes:Html"

# Open coverage report
open coverage-report/index.html  # macOS
start coverage-report/index.html  # Windows
```

### Coverage Goals

- **Unit Tests**: 80%+ line coverage for business logic
- **Critical Paths**: 100% coverage for core business rules
- **Integration Tests**: Focus on happy paths and error scenarios

### Coverage Analysis

```bash
# Run coverage with detailed output
dotnet test --collect:"XPlat Code Coverage" --logger "console;verbosity=detailed"

# View coverage summary in terminal
dotnet test --collect:"XPlat Code Coverage" | grep -A 10 "Coverage"
```

## 🔍 Troubleshooting

### Common Test Issues

1. **Test Discovery Problems**
   ```bash
   dotnet clean tests/
   dotnet build tests/
   dotnet test --list-tests
   ```

2. **Orchard Core Module Loading**
   - Ensure proper project references
   - Check module manifest configuration
   - Verify service registration

3. **Integration Test Failures**
   - Check application startup configuration
   - Verify database connection strings
   - Ensure test isolation

### Performance Optimization

1. **Slow Unit Tests**: Check for external dependencies
2. **Memory Issues**: Dispose of resources properly
3. **Parallel Execution**: Use `[Collection]` attributes for test isolation

## 📈 Test Metrics

Current test coverage for MicFx template:

- **Total Tests**: 10 tests
- **Unit Tests**: 6 tests (service layer)
- **Integration Tests**: 2 tests (HTTP endpoints)
- **Module Tests**: 2 tests (controller functionality)
- **Success Rate**: 100% (10/10 passing)

## 🚀 Continuous Integration

### GitHub Actions Example

```yaml
name: Tests

on:
  push:
    branches: [ main, develop ]
  pull_request:
    branches: [ main ]

jobs:
  test:
    runs-on: ubuntu-latest
    
    steps:
    - uses: actions/checkout@v4
    
    - name: Setup .NET
      uses: actions/setup-dotnet@v4
      with:
        dotnet-version: '8.0.x'
    
    - name: Restore dependencies
      run: dotnet restore
    
    - name: Build
      run: dotnet build --no-restore
    
    - name: Run Tests
      run: dotnet test --no-build --verbosity normal --collect:"XPlat Code Coverage"
    
    - name: Generate Coverage Report
      run: |
        dotnet tool install --global dotnet-reportgenerator-globaltool
        reportgenerator "-reports:tests/**/coverage.cobertura.xml" "-targetdir:coverage" "-reporttypes:Html"
    
    - name: Upload Coverage
      uses: actions/upload-artifact@v4
      with:
        name: coverage-report
        path: coverage/
```

## 📚 Testing Resources

- [xUnit Documentation](https://xunit.net/)
- [Moq Framework](https://github.com/moq/moq4)
- [ASP.NET Core Testing](https://docs.microsoft.com/en-us/aspnet/core/test/)
- [Orchard Core Testing](https://docs.orchardcore.net/en/dev/docs/getting-started/testing/)

---

This testing guide ensures comprehensive quality assurance for the MicFx starter template while providing practical examples for developing robust, well-tested modules and features. 