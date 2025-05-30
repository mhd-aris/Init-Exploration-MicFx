# MicFx Documentation

Welcome to the comprehensive documentation for the MicFx Orchard Core + TailwindCSS starter template.

## 📖 Documentation Overview

This documentation provides in-depth guides for all aspects of the MicFx template. For a quick overview and getting started, see the main [README.md](../README.md) in the root directory.

## 📑 Available Guides

### Essential Guides
- [🏗️ Architecture Overview](./architecture.md) - Detailed system architecture and design patterns
- [📝 Creating New Modules](./creating-modules.md) - Step-by-step module development guide
- [🎨 TailwindCSS Integration](./tailwind-integration.md) - Styling workflow and best practices

### Development Guides
- [🔧 Development Workflow](./development-workflow.md) - Best practices and development guidelines
- [🧪 Testing Guide](./testing.md) - Comprehensive testing strategies and examples
- [🚀 Deployment Guide](./deployment.md) - Multi-platform deployment strategies

## 🚀 Quick Navigation

### For New Developers
1. Start with the main [README.md](../README.md) for project overview
2. Follow [Creating New Modules](./creating-modules.md) for your first module
3. Review [Development Workflow](./development-workflow.md) for best practices

### For Architects
1. Read [Architecture Overview](./architecture.md) for system design
2. Review [Testing Guide](./testing.md) for quality assurance strategies
3. Check [Deployment Guide](./deployment.md) for production considerations

### For Frontend Developers
1. Focus on [TailwindCSS Integration](./tailwind-integration.md)
2. Review module creation for view development
3. Check testing guide for frontend testing approaches

## 🏗️ Project Architecture Summary

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

## 🔧 Core Components

- **MicFx.Core.Abstractions**: Interfaces and contracts
- **MicFx.Core.Infrastructure**: Infrastructure implementations
- **MicFx.Core.SharedKernel**: Shared domain logic
- **MicFx.Mvc.Web**: Web application host
- **Modules**: Self-contained feature modules

## 📋 Common Tasks

### Creating a New Module
```bash
cd src/Modules
dotnet new ocmodulemvc -n MicFx.Module.YourModule
dotnet sln add ../../MicFx.sln src/Modules/MicFx.Module.YourModule
```

### Running Tests
```bash
dotnet test                              # All tests
dotnet test tests/MicFx.Tests.Unit      # Unit tests only
dotnet test --collect:"XPlat Code Coverage"  # With coverage
```

### TailwindCSS Development
```bash
npm run tailwind:watch                  # Watch mode
dotnet watch run --project src/MicFx.Mvc.Web  # Hot reload
```

## 🤝 Contributing to Documentation

To improve this documentation:

1. Follow the established format and structure
2. Include practical examples where possible
3. Keep language clear and concise
4. Test all code examples before submitting
5. Update cross-references when adding new sections

## 📚 External Resources

- [Orchard Core Documentation](https://docs.orchardcore.net/)
- [TailwindCSS Documentation](https://tailwindcss.com/docs)
- [.NET 8.0 Documentation](https://docs.microsoft.com/en-us/dotnet/)
- [xUnit Testing Framework](https://xunit.net/)

---

*For questions about this documentation, please create an issue in the repository.* 