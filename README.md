# MicFx - Orchard Core + TailwindCSS Starter Template

A modern starter template for building modular web applications using **Orchard Core CMS** with **TailwindCSS** for styling. This template provides a clean foundation for developing scalable, modular web applications with a modern utility-first CSS framework.

## 🚀 Features

- **Orchard Core Framework**: Modular ASP.NET Core CMS framework
- **TailwindCSS Integration**: Utility-first CSS framework for rapid UI development
- **Clean Architecture**: Separated layers with abstractions, infrastructure, and shared kernel
- **Modular Design**: Self-contained modules with their own controllers, views, and services
- **.NET 8.0**: Latest .NET version support
- **Development Ready**: Pre-configured build pipeline and development tools
- **Comprehensive Testing**: Unit, integration, and module-specific tests
- **Hot Reload Support**: TailwindCSS watch mode and .NET hot reload

## 📁 Project Structure

```
MicFx/
├── src/
│   ├── MicFx.Mvc.Web/                  # Main web application host
│   ├── Core/
│   │   ├── MicFx.Core.Abstractions/    # Core interfaces and contracts
│   │   ├── MicFx.Core.Infrastructure/  # Infrastructure implementations
│   │   └── MicFx.Core.SharedKernel/    # Shared domain logic and utilities
│   └── Modules/
│       └── MicFx.Module.HelloWorld/    # Example module implementation
├── tests/                              # Test projects
│   ├── MicFx.Tests.Unit/              # Unit tests for business logic
│   ├── MicFx.Tests.Integration/        # Integration tests for HTTP endpoints
│   └── Modules/
│       └── MicFx.Module.HelloWorld.Tests/  # Module-specific tests
├── docs/                              # Detailed documentation
├── global.json                        # .NET SDK version specification
├── MicFx.sln                         # Solution file
└── README.md                         # This file
```

## 🛠️ Prerequisites

- **.NET 8.0 SDK** or later
- **Node.js** (for TailwindCSS compilation)
- **npm** package manager

## ⚡ Quick Start

### 1. Clone the Repository

```bash
git clone https://github.com/mhd-aris/Init-Exploration-MicFx.git
cd MicFx
```

### 2. Restore Dependencies

```bash
# Restore .NET packages
dotnet restore

# Navigate to web project and install npm dependencies
cd src/MicFx.Mvc.Web
npm install
```

### 3. Build TailwindCSS

```bash
# Watch mode for development (auto-recompile on changes)
npm run tailwind:watch

# Or build once
npx tailwindcss -i ./Assets/css/input.css -o ./wwwroot/css/site.css
```

### 4. Run the Application

```bash
# From root directory
dotnet run --project src/MicFx.Mvc.Web

# Or from web project directory
cd src/MicFx.Mvc.Web
dotnet run
```

The application will be available at `https://localhost:5001` or `http://localhost:5000`.

### 5. Run Tests

```bash
# Run all tests
dotnet test

# Run specific test types
dotnet test tests/MicFx.Tests.Unit           # Unit tests
dotnet test tests/MicFx.Tests.Integration    # Integration tests
```

## 🏗️ Architecture

### Clean Architecture Layers

The MicFx template follows Clean Architecture principles with clear separation of concerns:

```
┌─────────────────────────────────────────┐
│             Presentation Layer          │
│         (MicFx.Mvc.Web + Modules)      │
├─────────────────────────────────────────┤
│            Application Layer            │
│           (Module Services)             │
├─────────────────────────────────────────┤
│             Domain Layer                │
│        (Core.SharedKernel)             │
├─────────────────────────────────────────┤
│           Infrastructure Layer          │
│       (Core.Infrastructure)            │
└─────────────────────────────────────────┘
```

### Core Components

- **MicFx.Mvc.Web**: Main ASP.NET Core web application host and entry point
- **MicFx.Core.Abstractions**: Interfaces, contracts, and abstract base classes
- **MicFx.Core.Infrastructure**: Implementation of infrastructure concerns (data access, external services)
- **MicFx.Core.SharedKernel**: Shared domain logic, common utilities, and base entities

### Module System

This template leverages Orchard Core's powerful modular architecture:

- **Self-Contained Modules**: Each module is independent with its own controllers, views, services
- **Dependency Injection**: Automatic service registration and dependency resolution
- **Route Management**: Module-specific routing and area configuration
- **Asset Pipeline**: Module assets are automatically included in the build process

## 🎨 TailwindCSS Integration

TailwindCSS is fully integrated with intelligent content detection:

### Watched Files
- Main application views: `./Views/**/*.cshtml`
- Module views: `../Modules/**/Views/**/*.cshtml`
- Razor pages: `./Pages/**/*.cshtml`
- JavaScript files: `./wwwroot/**/*.js`

### Build Configuration
- **Input**: `src/MicFx.Mvc.Web/Assets/css/input.css`
- **Output**: `src/MicFx.Mvc.Web/wwwroot/css/site.css`
- **Config**: `src/MicFx.Mvc.Web/tailwind.config.js`

### Development Workflow
```bash
# Start TailwindCSS watch mode
npm run tailwind:watch

# In another terminal, start the application with hot reload
dotnet watch run --project src/MicFx.Mvc.Web
```

## 📖 Documentation

For detailed documentation, see the `docs/` folder:

- [📝 Creating New Modules](./docs/creating-modules.md) - Step-by-step module creation guide
- [🎨 TailwindCSS Integration](./docs/tailwind-integration.md) - Styling and CSS workflow
- [🏗️ Architecture Overview](./docs/architecture.md) - Detailed architecture documentation
- [🔧 Development Workflow](./docs/development-workflow.md) - Best practices and guidelines
- [🧪 Testing Guide](./docs/testing.md) - Comprehensive testing documentation
- [🚀 Deployment Guide](./docs/deployment.md) - Multi-platform deployment strategies

## 🧪 Testing

The project includes comprehensive testing coverage:

### Test Structure
```
tests/
├── MicFx.Tests.Unit/              # Business logic and service testing
├── MicFx.Tests.Integration/        # HTTP endpoint and full-stack testing
└── Modules/                       # Module-specific functionality testing
```

### Test Results
- **Unit Tests**: 6 tests covering service layer and business logic
- **Integration Tests**: 2 tests for HTTP endpoints and API responses
- **Module Tests**: 2 tests for controller functionality

### Running Tests
```bash
dotnet test                              # All tests
dotnet test --logger "console;verbosity=detailed"  # Detailed output
dotnet test --collect:"XPlat Code Coverage"        # With coverage
```

## 📦 Dependencies

### Main Dependencies
- **OrchardCore.Application.Mvc.Targets** (2.1.7) - Orchard Core framework
- **OrchardCore.Module.Targets** (2.1.7) - Module development targets

### Frontend Dependencies
- **tailwindcss** (^3.4.17) - Utility-first CSS framework
- **autoprefixer** (^10.4.21) - CSS vendor prefix automation
- **postcss** (^8.5.4) - CSS transformation pipeline

### Testing Dependencies
- **xunit** (2.5.3) - Testing framework
- **Moq** (4.20.69) - Mocking framework for unit tests
- **Microsoft.AspNetCore.Mvc.Testing** (8.0.0) - Integration testing support

## 🚀 Getting Started with Development

### Creating Your First Module

```bash
# Navigate to modules directory
cd src/Modules

# Create new module using Orchard Core template
dotnet new ocmodulemvc -n MicFx.Module.YourModuleName

# Add to solution
dotnet sln add ../../MicFx.sln src/Modules/MicFx.Module.YourModuleName
```

### Adding TailwindCSS Classes

TailwindCSS will automatically detect and include classes used in your views:

```html
<!-- Example: Module view with TailwindCSS -->
<div class="container mx-auto px-4 py-8">
    <h1 class="text-3xl font-bold text-gray-900 mb-6">Your Module</h1>
    <div class="bg-white shadow-lg rounded-lg p-6">
        <!-- Your content here -->
    </div>
</div>
```

## 🤝 Contributing

1. Fork the repository
2. Create a feature branch (`git checkout -b feature/amazing-feature`)
3. Make your changes following the established patterns
4. Add tests for new functionality
5. Ensure all tests pass (`dotnet test`)
6. Commit your changes (`git commit -m 'Add amazing feature'`)
7. Push to the branch (`git push origin feature/amazing-feature`)
8. Open a Pull Request

## 📋 Project Roadmap

- [ ] Additional module templates
- [ ] Enhanced testing utilities
- [ ] Docker development environment
- [ ] CI/CD pipeline templates
- [ ] Performance optimization guides
- [ ] Security best practices documentation

## 📄 License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.

## 🙋 Support & Community

- **Issues**: Report bugs and request features via [GitHub Issues]
- **Documentation**: Comprehensive guides in the `docs/` folder
- **Orchard Core**: Official documentation at [docs.orchardcore.net](https://docs.orchardcore.net/)
- **TailwindCSS**: Official documentation at [tailwindcss.com](https://tailwindcss.com/)

## 🌟 Acknowledgments

- [Orchard Core Team](https://github.com/OrchardCMS/OrchardCore) for the amazing modular framework
- [TailwindCSS Team](https://github.com/tailwindlabs/tailwindcss) for the utility-first CSS framework
- The .NET Community for continuous innovation and support 