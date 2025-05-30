# Development Workflow

This guide outlines the recommended development workflow for working with the MicFx Orchard Core + TailwindCSS starter template, including setup, daily development practices, and best practices.

## 🚀 Getting Started

### Initial Setup

1. **Clone and Setup Repository**
   ```bash
   git clone <repository-url>
   cd MicFx
   dotnet restore
   ```

2. **Install Frontend Dependencies**
   ```bash
   cd src/MicFx.Mvc.Web
   npm install
   ```

3. **Verify Setup**
   ```bash
   # Build the solution
   dotnet build
   
   # Start TailwindCSS watch mode
   npm run tailwind:watch
   
   # Run the application (in another terminal)
   dotnet run --project src/MicFx.Mvc.Web
   ```

### IDE Configuration

#### Visual Studio Code
```json
// .vscode/settings.json
{
  "files.exclude": {
    "**/bin": true,
    "**/obj": true,
    "**/node_modules": true
  },
  "dotnet.defaultSolution": "MicFx.sln",
  "tailwindCSS.includeLanguages": {
    "razor": "html",
    "cshtml": "html"
  },
  "tailwindCSS.experimental.classRegex": [
    ["class=\"([^\"]*)", "[\"'`]([^\"'`]*).*?[\"'`]"]
  ]
}
```

#### Visual Studio
- Install Orchard Core extension
- Configure TailwindCSS IntelliSense
- Set startup project to `MicFx.Mvc.Web`

## 📅 Daily Development Workflow

### 1. Start Development Session

```bash
# Terminal 1: Start TailwindCSS watch mode
cd src/MicFx.Mvc.Web
npm run tailwind:watch

# Terminal 2: Run application with hot reload
dotnet watch run --project src/MicFx.Mvc.Web
```

### 2. Development Loop

The typical development cycle involves:

1. **Code Changes**: Modify controllers, views, or services
2. **Auto Rebuild**: `dotnet watch` automatically rebuilds
3. **CSS Compilation**: TailwindCSS watch automatically recompiles CSS
4. **Browser Refresh**: Test changes in browser
5. **Commit Changes**: Regular commits with meaningful messages

### 3. Working with Modules

#### Creating a New Feature Module

```bash
# 1. Create the module
dotnet new ocmodulemvc -n MicFx.Module.UserManagement -o src/Modules/MicFx.Module.UserManagement

# 2. Add to solution
dotnet sln add src/Modules/MicFx.Module.UserManagement/MicFx.Module.UserManagement.csproj

# 3. Reference in web project
dotnet add src/MicFx.Mvc.Web/MicFx.Mvc.Web.csproj reference src/Modules/MicFx.Module.UserManagement/MicFx.Module.UserManagement.csproj

# 4. Restart application to load new module
```

#### Module Development Checklist

- [ ] Update `Manifest.cs` with proper metadata
- [ ] Configure services in `Startup.cs`
- [ ] Implement controllers with proper routing
- [ ] Create views using TailwindCSS classes
- [ ] Add unit tests for business logic
- [ ] Update documentation

### 4. Frontend Development

#### TailwindCSS Development Process

1. **Add Utility Classes**: Use TailwindCSS utilities in views
2. **Custom Components**: Create reusable CSS components when needed
3. **Responsive Design**: Test across different screen sizes
4. **Performance**: Monitor CSS file size and purging

#### Example View Development

```html
<!-- Before: Plain HTML -->
<div>
    <h1>User Profile</h1>
    <form>
        <input type="text" placeholder="Name">
        <button type="submit">Save</button>
    </form>
</div>

<!-- After: With TailwindCSS -->
<div class="max-w-md mx-auto bg-white rounded-xl shadow-md overflow-hidden">
    <div class="p-6">
        <h1 class="text-2xl font-bold text-gray-900 mb-4">User Profile</h1>
        <form class="space-y-4">
            <input type="text" 
                   placeholder="Name" 
                   class="w-full px-3 py-2 border border-gray-300 rounded-md focus:outline-none focus:ring-2 focus:ring-blue-500">
            <button type="submit" 
                    class="w-full bg-blue-500 hover:bg-blue-700 text-white font-bold py-2 px-4 rounded transition duration-200">
                Save
            </button>
        </form>
    </div>
</div>
```

## 🔧 Development Tools and Scripts

### Package.json Scripts

```json
{
  "scripts": {
    "tailwind:watch": "tailwindcss -i ./Assets/css/input.css -o ./wwwroot/css/site.css --watch",
    "tailwind:build": "tailwindcss -i ./Assets/css/input.css -o ./wwwroot/css/site.css",
    "tailwind:build-prod": "tailwindcss -i ./Assets/css/input.css -o ./wwwroot/css/site.css --minify",
    "dev": "concurrently \"npm run tailwind:watch\" \"dotnet watch run\"",
    "css:purge": "tailwindcss -i ./Assets/css/input.css -o ./wwwroot/css/site.css --minify"
  }
}
```

### Development Helper Scripts

Create a `scripts/` folder in the root with helper scripts:

#### `scripts/new-module.sh`
```bash
#!/bin/bash
MODULE_NAME=$1

if [ -z "$MODULE_NAME" ]; then
    echo "Usage: ./new-module.sh ModuleName"
    exit 1
fi

FULL_NAME="MicFx.Module.$MODULE_NAME"
MODULE_PATH="src/Modules/$FULL_NAME"

# Create module
dotnet new ocmodulemvc -n $FULL_NAME -o $MODULE_PATH

# Add to solution
dotnet sln add $MODULE_PATH/$FULL_NAME.csproj

# Add reference to web project
dotnet add src/MicFx.Mvc.Web/MicFx.Mvc.Web.csproj reference $MODULE_PATH/$FULL_NAME.csproj

echo "Module $FULL_NAME created successfully!"
echo "Restart the application to load the new module."
```

#### `scripts/dev-setup.sh`
```bash
#!/bin/bash
echo "Setting up MicFx development environment..."

# Restore .NET packages
echo "Restoring .NET packages..."
dotnet restore

# Install npm dependencies
echo "Installing npm dependencies..."
cd src/MicFx.Mvc.Web
npm install

# Build TailwindCSS
echo "Building TailwindCSS..."
npm run tailwind:build

echo "Setup complete! Run 'npm run dev' to start development."
```

### Git Workflow

#### Branch Naming Conventions
- `feature/module-name` - New features
- `bugfix/issue-description` - Bug fixes
- `hotfix/critical-issue` - Critical production fixes
- `refactor/component-name` - Code refactoring

#### Commit Message Format
```
type(scope): description

[optional body]

[optional footer]
```

Examples:
```
feat(user-module): add user profile management
fix(tailwind): resolve CSS compilation issues
docs(readme): update installation instructions
refactor(core): improve service registration
```

## 🧪 Testing Workflow

### Unit Testing

#### Test Project Structure
```
tests/
├── MicFx.Tests.Unit/
│   ├── Core/
│   │   ├── Services/
│   │   └── Utilities/
│   └── Modules/
│       └── UserManagement/
│           ├── Controllers/
│           └── Services/
└── MicFx.Tests.Integration/
    ├── Controllers/
    └── Modules/
```

#### Example Unit Test
```csharp
// tests/MicFx.Tests.Unit/Modules/UserManagement/Services/UserServiceTests.cs
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
    }
}
```

### Integration Testing

#### Test Configuration
```csharp
// tests/MicFx.Tests.Integration/TestStartup.cs
public class TestStartup : Startup
{
    public override void ConfigureServices(IServiceCollection services)
    {
        base.ConfigureServices(services);
        
        // Override services for testing
        services.AddSingleton<IEmailService, MockEmailService>();
    }
}
```

### Running Tests

```bash
# Run all tests
dotnet test

# Run specific test project
dotnet test tests/MicFx.Tests.Unit

# Run with coverage
dotnet test --collect:"XPlat Code Coverage"

# Watch mode for TDD
dotnet watch test tests/MicFx.Tests.Unit
```

## 🚀 Build and Deployment Workflow

### Development Build

```bash
# Clean and build
dotnet clean
dotnet build

# Build with specific configuration
dotnet build --configuration Release

# Publish for deployment
dotnet publish src/MicFx.Mvc.Web --configuration Release --output ./publish
```

### CSS Build for Production

```bash
# Navigate to web project
cd src/MicFx.Mvc.Web

# Build and minify CSS
npm run tailwind:build-prod

# Verify file size
ls -la wwwroot/css/site.css
```

### CI/CD Pipeline Example (GitHub Actions)

```yaml
# .github/workflows/ci.yml
name: CI/CD Pipeline

on:
  push:
    branches: [ main, develop ]
  pull_request:
    branches: [ main ]

jobs:
  test:
    runs-on: ubuntu-latest
    
    steps:
    - uses: actions/checkout@v3
    
    - name: Setup .NET
      uses: actions/setup-dotnet@v3
      with:
        dotnet-version: '8.0.x'
    
    - name: Setup Node.js
      uses: actions/setup-node@v3
      with:
        node-version: '18'
    
    - name: Restore dependencies
      run: dotnet restore
    
    - name: Install npm dependencies
      run: |
        cd src/MicFx.Mvc.Web
        npm install
    
    - name: Build TailwindCSS
      run: |
        cd src/MicFx.Mvc.Web
        npm run tailwind:build-prod
    
    - name: Build
      run: dotnet build --no-restore
    
    - name: Test
      run: dotnet test --no-build --verbosity normal
```

## 🔍 Debugging and Troubleshooting

### Common Issues and Solutions

#### 1. TailwindCSS Not Compiling

**Problem**: CSS changes not reflected in browser

**Solutions**:
```bash
# Check if watch mode is running
npm run tailwind:watch

# Force rebuild
npm run tailwind:build

# Check file paths in tailwind.config.js
# Verify input.css exists and has correct directives
```

#### 2. Module Not Loading

**Problem**: New module not appearing in application

**Solutions**:
- Verify module reference in web project
- Check Manifest.cs configuration
- Restart application
- Check for compilation errors

#### 3. Hot Reload Not Working

**Problem**: Changes require manual restart

**Solutions**:
```bash
# Ensure using watch mode
dotnet watch run --project src/MicFx.Mvc.Web

# Check file watchers aren't exceeded (Linux)
echo fs.inotify.max_user_watches=524288 | sudo tee -a /etc/sysctl.conf

# Clear browser cache
# Check for compilation errors
```

### Debugging Tools

#### Browser Developer Tools
- Use for CSS inspection and responsive testing
- Monitor network requests and performance
- Debug JavaScript (if added)

#### .NET Debugging
```csharp
// Add conditional compilation for debugging
#if DEBUG
    services.AddDeveloperExceptionPage();
#endif

// Use logging for debugging
private readonly ILogger<HomeController> _logger;

public ActionResult Index()
{
    _logger.LogInformation("Index action called");
    return View();
}
```

#### Performance Monitoring
```bash
# Monitor CSS file size
ls -la src/MicFx.Mvc.Web/wwwroot/css/site.css

# Check application startup time
dotnet run --project src/MicFx.Mvc.Web --verbosity detailed

# Monitor memory usage
dotnet-counters monitor --process-id <PID>
```

## 📝 Code Quality and Standards

### Code Style Guidelines

1. **C# Conventions**: Follow Microsoft C# coding conventions
2. **Naming**: Use PascalCase for public members, camelCase for private
3. **File Organization**: One class per file, logical folder structure
4. **Comments**: XML documentation for public APIs

### Code Quality Tools

#### .editorconfig
```ini
# .editorconfig
root = true

[*]
charset = utf-8
insert_final_newline = true
trim_trailing_whitespace = true

[*.cs]
indent_style = space
indent_size = 4

[*.{js,ts,json}]
indent_style = space
indent_size = 2
```

#### Code Analysis
```xml
<!-- Directory.Build.props -->
<Project>
  <PropertyGroup>
    <TreatWarningsAsErrors>true</TreatWarningsAsErrors>
    <WarningsAsErrors />
    <WarningsNotAsErrors>CS1591</WarningsNotAsErrors>
    <CodeAnalysisRuleSet>$(MSBuildThisFileDirectory)CodeAnalysis.ruleset</CodeAnalysisRuleSet>
  </PropertyGroup>
</Project>
```

This development workflow provides a solid foundation for productive development with the MicFx starter template. 