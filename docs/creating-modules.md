# Creating New Modules

This guide explains how to create new modules in the MicFx Orchard Core starter template. Modules are self-contained components that can include controllers, views, services, and other functionality.

## 📋 Overview

Orchard Core modules are individual ASP.NET Core projects that can be dynamically loaded by the main application. Each module can contain:

- Controllers and Actions
- Views and Layouts
- Services and Dependencies
- Routes and Middleware
- Background Tasks
- Content Types and Parts
- Themes and Assets

## 🛠️ Step-by-Step Module Creation

### 1. Create the Module Project

Use the Orchard Core MVC module template to create a new module:

```bash
# Navigate to the root directory
cd /path/to/MicFx

# Create a new module using the Orchard Core template
dotnet new ocmodulemvc -n MicFx.Module.YourModuleName -o src/Modules/MicFx.Module.YourModuleName
```

**Example:**
```bash
dotnet new ocmodulemvc -n MicFx.Module.HelloWorld -o src/Modules/MicFx.Module.HelloWorld
```

### 2. Add Module to Solution

Add the newly created module to the main solution:

```bash
dotnet sln add src/Modules/MicFx.Module.YourModuleName/MicFx.Module.YourModuleName.csproj
```

**Example:**
```bash
dotnet sln add src/Modules/MicFx.Module.HelloWorld/MicFx.Module.HelloWorld.csproj
```

### 3. Reference Module in Web Application

Add a project reference from the main web application to your module:

```bash
dotnet add src/MicFx.Mvc.Web/MicFx.Mvc.Web.csproj reference src/Modules/MicFx.Module.YourModuleName/MicFx.Module.YourModuleName.csproj
```

**Example:**
```bash
dotnet add src/MicFx.Mvc.Web/MicFx.Mvc.Web.csproj reference src/Modules/MicFx.Module.HelloWorld/MicFx.Module.HelloWorld.csproj
```

## 📂 Module Structure

After creation, your module will have the following structure:

```
src/Modules/MicFx.Module.YourModuleName/
├── Controllers/
│   └── HomeController.cs
├── Views/
│   └── Home/
│       └── Index.cshtml
├── Manifest.cs                 # Module metadata
├── Startup.cs                  # Module configuration
└── MicFx.Module.YourModuleName.csproj
```

## ⚙️ Module Configuration Files

### Manifest.cs

The manifest file defines module metadata:

```csharp
using OrchardCore.Modules.Manifest;

[assembly: Module(
    Name = "MicFx.Module.YourModuleName",
    Author = "Your Name",
    Website = "https://yourwebsite.com",
    Version = "1.0.0",
    Description = "Description of your module",
    Category = "Content",
    Dependencies = new[] { "OrchardCore.Contents" } // Optional dependencies
)]
```

### Startup.cs

The startup file configures services and middleware:

```csharp
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using OrchardCore.Modules;

namespace MicFx.Module.YourModuleName;

public sealed class Startup : StartupBase
{
    public override void ConfigureServices(IServiceCollection services)
    {
        // Register your services here
        services.AddMvc();
        
        // Example: Register a custom service
        // services.AddScoped<IYourService, YourService>();
    }

    public override void Configure(IApplicationBuilder builder, IEndpointRouteBuilder routes, IServiceProvider serviceProvider)
    {
        // Configure routes for your module
        routes.MapAreaControllerRoute(
            name: "YourModuleName",
            areaName: "YourModuleName",
            pattern: "YourModuleName/{controller=Home}/{action=Index}/{id?}"
        );
    }
}
```

## 🎮 Creating Controllers

Create controllers in the `Controllers` folder:

```csharp
using Microsoft.AspNetCore.Mvc;

namespace MicFx.Module.YourModuleName.Controllers;

[Route("YourModuleName")]
public class HomeController : Controller
{
    public ActionResult Index()
    {
        return View();
    }
    
    [HttpGet("details/{id}")]
    public ActionResult Details(int id)
    {
        // Your logic here
        return View();
    }
}
```

## 🎨 Creating Views with TailwindCSS

Create views in the `Views` folder. You can use TailwindCSS classes directly:

```html
<!-- Views/Home/Index.cshtml -->
<div class="container mx-auto px-4 py-8">
    <h1 class="text-3xl font-bold text-gray-800 mb-6">
        Welcome to @ViewData["Title"]
    </h1>
    
    <div class="bg-white rounded-lg shadow-md p-6">
        <p class="text-gray-600 mb-4">
            This is a sample view using TailwindCSS classes.
        </p>
        
        <button class="bg-blue-500 hover:bg-blue-700 text-white font-bold py-2 px-4 rounded">
            Click Me
        </button>
    </div>
</div>
```

## 🔧 Advanced Module Features

### Adding Services

Create services in your module:

```csharp
// IYourService.cs
public interface IYourService
{
    Task<string> GetDataAsync();
}

// YourService.cs
public class YourService : IYourService
{
    public async Task<string> GetDataAsync()
    {
        // Your implementation
        return await Task.FromResult("Sample data");
    }
}

// Register in Startup.cs
public override void ConfigureServices(IServiceCollection services)
{
    services.AddScoped<IYourService, YourService>();
}
```

### Adding Background Tasks

Create background tasks for your module:

```csharp
using OrchardCore.BackgroundTasks;

public class YourBackgroundTask : IBackgroundTask
{
    public Task DoWorkAsync(IServiceProvider serviceProvider, CancellationToken cancellationToken)
    {
        // Your background task logic
        return Task.CompletedTask;
    }
}

// Register in Startup.cs
public override void ConfigureServices(IServiceCollection services)
{
    services.AddSingleton<IBackgroundTask, YourBackgroundTask>();
}
```

### Module Dependencies

Specify dependencies in your `Manifest.cs`:

```csharp
[assembly: Module(
    Name = "MicFx.Module.YourModuleName",
    Dependencies = new[] { 
        "OrchardCore.Contents",
        "OrchardCore.Users",
        "MicFx.Module.AnotherModule"
    }
)]
```

## 🔄 Module Lifecycle

1. **Creation**: Module project is created and added to solution
2. **Configuration**: Manifest and Startup files are configured
3. **Development**: Controllers, views, and services are implemented
4. **Registration**: Module is referenced by the main web application
5. **Loading**: Module is automatically discovered and loaded by Orchard Core

## 🚀 Best Practices

### Naming Conventions
- Use `MicFx.Module.` prefix for all modules
- Use PascalCase for module names
- Keep module names descriptive and concise

### Project Structure
- Group related functionality in separate modules
- Keep modules focused on a single responsibility
- Use the `Core` projects for shared functionality

### TailwindCSS Integration
- All module views automatically have access to TailwindCSS
- Use utility classes for consistent styling
- Follow TailwindCSS best practices for maintainable CSS

### Performance
- Register services with appropriate lifetimes (Singleton, Scoped, Transient)
- Use async/await for I/O operations
- Implement proper caching where needed

## 🔍 Common Issues and Solutions

### Module Not Loading
- Ensure the module is referenced in `MicFx.Mvc.Web.csproj`
- Check that the `Manifest.cs` is properly configured
- Verify that dependencies are available

### Routing Issues
- Check route configuration in `Startup.cs`
- Ensure area name matches module name
- Verify controller and action names

### TailwindCSS Not Working
- Ensure TailwindCSS is running in watch mode
- Check that module views are included in `tailwind.config.js`
- Verify the output CSS file is being generated

## 📚 Additional Resources

- [Orchard Core Modules Documentation](https://docs.orchardcore.net/en/dev/docs/getting-started/templates/)
- [TailwindCSS Documentation](https://tailwindcss.com/docs)
- [ASP.NET Core MVC](https://docs.microsoft.com/en-us/aspnet/core/mvc/)

## 💡 Example Module Templates

### Simple Content Module
```bash
dotnet new ocmodulemvc -n MicFx.Module.Blog -o src/Modules/MicFx.Module.Blog
```

### API Module
```bash
dotnet new ocmodulemvc -n MicFx.Module.ApiEndpoints -o src/Modules/MicFx.Module.ApiEndpoints
```

### Admin Interface Module
```bash
dotnet new ocmodulemvc -n MicFx.Module.AdminDashboard -o src/Modules/MicFx.Module.AdminDashboard
``` 