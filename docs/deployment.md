# Deployment Guide

This guide covers deployment strategies and configurations for the MicFx Orchard Core + TailwindCSS starter template across different environments and platforms.

## 🌍 Deployment Overview

### Supported Platforms
- **Windows Server** (IIS)
- **Linux** (Nginx/Apache)
- **Docker** (Containerized)
- **Cloud Platforms** (Azure, AWS, Google Cloud)
- **Platform as a Service** (Azure App Service, Heroku)

### Deployment Types
- **Self-Contained**: Includes .NET runtime
- **Framework-Dependent**: Requires .NET runtime on target machine
- **Docker**: Containerized deployment
- **Single File**: All dependencies in one executable

## 🚀 Pre-Deployment Preparation

### 1. Build Optimization

#### Production CSS Build
```bash
# Navigate to web project
cd src/MicFx.Mvc.Web

# Build optimized CSS
npm run tailwind:build-prod

# Verify CSS file size
ls -la wwwroot/css/site.css
```

#### .NET Optimization
```bash
# Clean previous builds
dotnet clean

# Restore packages
dotnet restore

# Build in Release mode
dotnet build --configuration Release

# Publish for deployment
dotnet publish src/MicFx.Mvc.Web \
  --configuration Release \
  --runtime linux-x64 \
  --self-contained true \
  --output ./publish
```

### 2. Configuration Management

#### appsettings.Production.json
```json
{
  "Logging": {
    "LogLevel": {
      "Default": "Warning",
      "Microsoft.AspNetCore": "Warning",
      "OrchardCore": "Warning"
    }
  },
  "ConnectionStrings": {
    "DefaultConnection": "#{ConnectionString}#"
  },
  "OrchardCore": {
    "Email": {
      "DefaultSender": "noreply@yourdomain.com"
    }
  }
}
```

#### Environment Variables
```bash
# Required environment variables
export ASPNETCORE_ENVIRONMENT=Production
export ConnectionStrings__DefaultConnection="Server=...;Database=...;"
export OrchardCore__Email__DefaultSender="noreply@yourdomain.com"
```

### 3. Security Hardening

#### SSL/HTTPS Configuration
```csharp
// Program.cs - Production security
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
    app.UseHttpsRedirection();
}

// Force HTTPS
app.Use(async (context, next) =>
{
    if (!context.Request.IsHttps)
    {
        var httpsUrl = $"https://{context.Request.Host}{context.Request.Path}{context.Request.QueryString}";
        context.Response.Redirect(httpsUrl, permanent: true);
        return;
    }
    await next();
});
```

#### Security Headers
```csharp
// Add security headers middleware
app.Use(async (context, next) =>
{
    context.Response.Headers.Add("X-Frame-Options", "DENY");
    context.Response.Headers.Add("X-Content-Type-Options", "nosniff");
    context.Response.Headers.Add("X-XSS-Protection", "1; mode=block");
    context.Response.Headers.Add("Referrer-Policy", "strict-origin-when-cross-origin");
    
    await next();
});
```

## 🐳 Docker Deployment

### Dockerfile
```dockerfile
# Build stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Install Node.js for TailwindCSS
RUN curl -fsSL https://deb.nodesource.com/setup_18.x | bash - \
    && apt-get install -y nodejs

# Copy project files
COPY ["src/MicFx.Mvc.Web/MicFx.Mvc.Web.csproj", "src/MicFx.Mvc.Web/"]
COPY ["src/Core/MicFx.Core.Abstractions/MicFx.Core.Abstractions.csproj", "src/Core/MicFx.Core.Abstractions/"]
COPY ["src/Core/MicFx.Core.SharedKernel/MicFx.Core.SharedKernel.csproj", "src/Core/MicFx.Core.SharedKernel/"]
COPY ["src/Modules/MicFx.Module.HelloWorld/MicFx.Module.HelloWorld.csproj", "src/Modules/MicFx.Module.HelloWorld/"]

# Restore .NET packages
RUN dotnet restore "src/MicFx.Mvc.Web/MicFx.Mvc.Web.csproj"

# Copy all source code
COPY . .

# Build TailwindCSS
WORKDIR /src/src/MicFx.Mvc.Web
RUN npm install
RUN npm run tailwind:build-prod

# Build and publish .NET application
WORKDIR /src
RUN dotnet publish "src/MicFx.Mvc.Web/MicFx.Mvc.Web.csproj" \
    -c Release \
    -o /app/publish \
    --no-restore

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app

# Create non-root user
RUN groupadd -r appuser && useradd -r -g appuser appuser
RUN chown -R appuser:appuser /app
USER appuser

# Copy published application
COPY --from=build /app/publish .

# Expose port
EXPOSE 8080

# Set environment variables
ENV ASPNETCORE_URLS=http://+:8080
ENV ASPNETCORE_ENVIRONMENT=Production

# Health check
HEALTHCHECK --interval=30s --timeout=10s --start-period=5s --retries=3 \
  CMD curl -f http://localhost:8080/health || exit 1

# Start application
ENTRYPOINT ["dotnet", "MicFx.Mvc.Web.dll"]
```

### Docker Compose
```yaml
# docker-compose.yml
version: '3.8'

services:
  micfx-web:
    build:
      context: .
      dockerfile: Dockerfile
    ports:
      - "80:8080"
    environment:
      - ASPNETCORE_ENVIRONMENT=Production
      - ConnectionStrings__DefaultConnection=Server=db;Database=MicFx;User=sa;Password=YourStrong@Passw0rd;
    depends_on:
      - db
    volumes:
      - ./app_data:/app/App_Data
    restart: unless-stopped

  db:
    image: mcr.microsoft.com/mssql/server:2022-latest
    environment:
      - ACCEPT_EULA=Y
      - SA_PASSWORD=YourStrong@Passw0rd
    volumes:
      - sqldata:/var/opt/mssql
    restart: unless-stopped

volumes:
  sqldata:
```

### Build and Run
```bash
# Build Docker image
docker build -t micfx-app .

# Run with Docker Compose
docker-compose up -d

# View logs
docker-compose logs -f micfx-web

# Scale services
docker-compose up --scale micfx-web=3
```

## ☁️ Cloud Platform Deployments

### Azure App Service

#### Azure CLI Deployment
```bash
# Login to Azure
az login

# Create resource group
az group create --name MicFx-RG --location "East US"

# Create App Service plan
az appservice plan create \
  --name MicFx-Plan \
  --resource-group MicFx-RG \
  --sku B1 \
  --is-linux

# Create web app
az webapp create \
  --resource-group MicFx-RG \
  --plan MicFx-Plan \
  --name micfx-webapp \
  --runtime "DOTNETCORE:8.0"

# Deploy application
az webapp deployment source config-zip \
  --resource-group MicFx-RG \
  --name micfx-webapp \
  --src ./publish.zip
```

#### Azure DevOps Pipeline
```yaml
# azure-pipelines.yml
trigger:
  branches:
    include:
    - main

pool:
  vmImage: 'ubuntu-latest'

variables:
  buildConfiguration: 'Release'
  azureSubscription: 'Your-Azure-Subscription'
  webAppName: 'micfx-webapp'

stages:
- stage: Build
  jobs:
  - job: Build
    steps:
    - task: UseDotNet@2
      inputs:
        packageType: 'sdk'
        version: '8.0.x'

    - task: NodeTool@0
      inputs:
        versionSpec: '18.x'

    - script: |
        cd src/MicFx.Mvc.Web
        npm install
        npm run tailwind:build-prod
      displayName: 'Build TailwindCSS'

    - task: DotNetCoreCLI@2
      inputs:
        command: 'restore'
        projects: '**/*.csproj'

    - task: DotNetCoreCLI@2
      inputs:
        command: 'build'
        projects: '**/*.csproj'
        arguments: '--configuration $(buildConfiguration)'

    - task: DotNetCoreCLI@2
      inputs:
        command: 'test'
        projects: 'tests/**/*.csproj'
        arguments: '--configuration $(buildConfiguration) --collect:"XPlat Code Coverage"'

    - task: DotNetCoreCLI@2
      inputs:
        command: 'publish'
        projects: 'src/MicFx.Mvc.Web/MicFx.Mvc.Web.csproj'
        arguments: '--configuration $(buildConfiguration) --output $(Build.ArtifactStagingDirectory)'
        zipAfterPublish: true

    - task: PublishBuildArtifacts@1
      inputs:
        PathtoPublish: '$(Build.ArtifactStagingDirectory)'
        ArtifactName: 'drop'

- stage: Deploy
  condition: succeeded()
  jobs:
  - deployment: DeployToAzure
    environment: 'Production'
    strategy:
      runOnce:
        deploy:
          steps:
          - task: AzureWebApp@1
            inputs:
              azureSubscription: '$(azureSubscription)'
              appType: 'webAppLinux'
              appName: '$(webAppName)'
              package: '$(Pipeline.Workspace)/drop/*.zip'
```

### AWS Elastic Beanstalk

#### Deployment Package
```bash
# Create deployment package
dotnet publish src/MicFx.Mvc.Web \
  --configuration Release \
  --runtime linux-x64 \
  --self-contained false \
  --output ./aws-deploy

# Create deployment zip
cd aws-deploy
zip -r ../micfx-deploy.zip .
```

#### .ebextensions Configuration
```yaml
# .ebextensions/01-nginx.config
files:
  "/etc/nginx/conf.d/https_redirect.conf":
    mode: "000644"
    owner: root
    group: root
    content: |
      server {
        listen 80;
        return 301 https://$host$request_uri;
      }

# .ebextensions/02-environment.config
option_settings:
  aws:elasticbeanstalk:application:environment:
    ASPNETCORE_ENVIRONMENT: Production
    ASPNETCORE_URLS: http://+:5000
```

### Google Cloud Platform

#### Cloud Run Deployment
```bash
# Build and push to Container Registry
gcloud builds submit --tag gcr.io/PROJECT-ID/micfx-app

# Deploy to Cloud Run
gcloud run deploy micfx-service \
  --image gcr.io/PROJECT-ID/micfx-app \
  --platform managed \
  --region us-central1 \
  --allow-unauthenticated \
  --port 8080 \
  --memory 512Mi \
  --cpu 1
```

## 🔧 Traditional Server Deployments

### IIS (Windows Server)

#### IIS Configuration
```xml
<!-- web.config -->
<?xml version="1.0" encoding="utf-8"?>
<configuration>
  <location path="." inheritInChildApplications="false">
    <system.webServer>
      <handlers>
        <add name="aspNetCore" path="*" verb="*" modules="AspNetCoreModuleV2" resourceType="Unspecified" />
      </handlers>
      <aspNetCore processPath="dotnet" 
                  arguments=".\MicFx.Mvc.Web.dll" 
                  stdoutLogEnabled="false" 
                  stdoutLogFile=".\logs\stdout" 
                  hostingModel="inprocess" />
      <security>
        <requestFiltering>
          <requestLimits maxAllowedContentLength="52428800" />
        </requestFiltering>
      </security>
    </system.webServer>
  </location>
</configuration>
```

#### Deployment Script
```powershell
# deploy-iis.ps1
param(
    [Parameter(Mandatory=$true)]
    [string]$SiteName,
    
    [Parameter(Mandatory=$true)]
    [string]$PhysicalPath
)

# Stop IIS site
Stop-IISSite -Name $SiteName

# Backup current deployment
$BackupPath = "C:\Backups\$SiteName-$(Get-Date -Format 'yyyyMMdd-HHmmss')"
Copy-Item -Path $PhysicalPath -Destination $BackupPath -Recurse

# Deploy new version
Remove-Item -Path "$PhysicalPath\*" -Recurse -Force
Copy-Item -Path ".\publish\*" -Destination $PhysicalPath -Recurse

# Start IIS site
Start-IISSite -Name $SiteName

Write-Host "Deployment completed successfully"
```

### Nginx (Linux)

#### Nginx Configuration
```nginx
# /etc/nginx/sites-available/micfx
server {
    listen 80;
    server_name yourdomain.com www.yourdomain.com;
    return 301 https://$server_name$request_uri;
}

server {
    listen 443 ssl http2;
    server_name yourdomain.com www.yourdomain.com;

    ssl_certificate /path/to/certificate.crt;
    ssl_certificate_key /path/to/private.key;
    ssl_protocols TLSv1.2 TLSv1.3;
    ssl_ciphers ECDHE-RSA-AES256-GCM-SHA384:ECDHE-RSA-AES128-GCM-SHA256;

    location / {
        proxy_pass http://localhost:5000;
        proxy_http_version 1.1;
        proxy_set_header Upgrade $http_upgrade;
        proxy_set_header Connection keep-alive;
        proxy_set_header Host $host;
        proxy_set_header X-Real-IP $remote_addr;
        proxy_set_header X-Forwarded-For $proxy_add_x_forwarded_for;
        proxy_set_header X-Forwarded-Proto $scheme;
        proxy_cache_bypass $http_upgrade;
        proxy_redirect off;
    }

    location ~* \.(css|js|png|jpg|jpeg|gif|ico|svg)$ {
        expires 1y;
        add_header Cache-Control "public, immutable";
    }
}
```

#### Systemd Service
```ini
# /etc/systemd/system/micfx.service
[Unit]
Description=MicFx Web Application
After=network.target

[Service]
Type=notify
ExecStart=/usr/bin/dotnet /var/www/micfx/MicFx.Mvc.Web.dll
Restart=always
RestartSec=5
KillSignal=SIGINT
SyslogIdentifier=micfx
User=www-data
Environment=ASPNETCORE_ENVIRONMENT=Production
Environment=ASPNETCORE_URLS=http://localhost:5000
WorkingDirectory=/var/www/micfx

[Install]
WantedBy=multi-user.target
```

#### Deployment Script
```bash
#!/bin/bash
# deploy-linux.sh

APP_NAME="micfx"
APP_PATH="/var/www/$APP_NAME"
PUBLISH_PATH="./publish"
BACKUP_PATH="/var/backups/$APP_NAME-$(date +%Y%m%d-%H%M%S)"

echo "Starting deployment of $APP_NAME..."

# Create backup
sudo mkdir -p $BACKUP_PATH
sudo cp -r $APP_PATH/* $BACKUP_PATH/

# Stop service
sudo systemctl stop $APP_NAME

# Deploy new version
sudo rm -rf $APP_PATH/*
sudo cp -r $PUBLISH_PATH/* $APP_PATH/
sudo chown -R www-data:www-data $APP_PATH

# Start service
sudo systemctl start $APP_NAME
sudo systemctl status $APP_NAME

echo "Deployment completed successfully"
```

## 📊 Monitoring and Health Checks

### Health Check Endpoint
```csharp
// Program.cs
builder.Services.AddHealthChecks()
    .AddCheck("self", () => HealthCheckResult.Healthy())
    .AddSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));

app.MapHealthChecks("/health", new HealthCheckOptions
{
    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
});
```

### Application Insights (Azure)
```csharp
// Program.cs
builder.Services.AddApplicationInsightsTelemetry();

// appsettings.json
{
  "ApplicationInsights": {
    "InstrumentationKey": "your-instrumentation-key"
  }
}
```

### Logging Configuration
```json
{
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    },
    "ApplicationInsights": {
      "LogLevel": {
        "Default": "Information"
      }
    }
  }
}
```

## 🔄 Blue-Green Deployment

### Infrastructure Setup
```bash
# Create two identical environments
# Blue (current production)
# Green (new version)

# Deploy to Green environment
./deploy.sh green

# Test Green environment
curl -f https://green.yourdomain.com/health

# Switch traffic to Green
# Update load balancer configuration
# or DNS records

# Monitor Green environment
# If successful, Green becomes new Blue
# If failed, rollback to Blue
```

### Load Balancer Configuration
```nginx
# nginx load balancer
upstream micfx_blue {
    server 10.0.1.100:5000;
}

upstream micfx_green {
    server 10.0.1.101:5000;
}

upstream micfx_active {
    server 10.0.1.100:5000;  # Switch to 101 for green
}

server {
    listen 443 ssl;
    server_name yourdomain.com;
    
    location / {
        proxy_pass http://micfx_active;
    }
}
```

## 🚨 Rollback Procedures

### Quick Rollback Script
```bash
#!/bin/bash
# rollback.sh

BACKUP_VERSION=$1
APP_PATH="/var/www/micfx"
BACKUP_PATH="/var/backups/$BACKUP_VERSION"

if [ ! -d "$BACKUP_PATH" ]; then
    echo "Backup version $BACKUP_VERSION not found"
    exit 1
fi

echo "Rolling back to $BACKUP_VERSION..."

# Stop service
sudo systemctl stop micfx

# Restore backup
sudo rm -rf $APP_PATH/*
sudo cp -r $BACKUP_PATH/* $APP_PATH/
sudo chown -R www-data:www-data $APP_PATH

# Start service
sudo systemctl start micfx

echo "Rollback completed"
```

### Database Migration Rollback
```bash
# Rollback to specific migration
dotnet ef database update <PreviousMigrationName> --project src/MicFx.Mvc.Web
```

This comprehensive deployment guide provides multiple strategies for deploying the MicFx starter template across various platforms and environments, ensuring flexibility and reliability in production deployments. 