# Fitness DotNet App (HealthManagement)

A full-stack ASP.NET Core MVC health tracking application where users can log food intake, track exercises, and view daily/weekly health insights. The app uses SQLite for persistence, ASP.NET Core Identity for authentication, and integrates Azure OpenAI for nutrient extraction and AI-based recommendations.

## Features

- User authentication and authorization with ASP.NET Core Identity
- Food logging (create, edit, delete, history)
- Exercise logging (create, edit, delete, history)
- Dashboard with calorie intake vs calories burned summary
- Service + repository layered architecture for maintainability
- Azure OpenAI integration for nutrient extraction and health insights
- Azure deployment support using Azure Developer CLI (`azd`)

## Tech Stack

- .NET 8 / ASP.NET Core MVC
- Entity Framework Core 8
- SQLite (`healthmanagement.db`)
- ASP.NET Core Identity
- Azure OpenAI SDK (`Azure.AI.OpenAI`)
- Azure Developer CLI (`azd`) + Bicep (`infra/`)

## Project Structure

```text
HealthManagement/
├── Controllers/         # MVC controllers
├── Data/                # DbContext
├── Models/              # Domain and identity models
├── Repositories/        # Data access abstraction
├── Services/            # Business logic and AI integration
├── Views/               # Razor views
├── Settings/            # App settings classes
├── Migrations/          # EF Core migrations
├── infra/               # Azure IaC (Bicep)
├── Program.cs           # App startup and DI
└── appsettings.json     # Configuration
```

## Prerequisites

- [.NET 8 SDK](https://dotnet.microsoft.com/download)
- (Optional) [Azure Developer CLI](https://learn.microsoft.com/azure/developer/azure-developer-cli/install-azd)
- (Optional) Azure OpenAI resource for AI features

## Local Setup

1. Clone the repository:

```bash
git clone https://github.com/iamrohitkainth2/Fitness_DotNet_App.git
cd Fitness_DotNet_App/HealthManagement
```

2. Restore dependencies:

```bash
dotnet restore
```

3. Configure Azure OpenAI (optional but required for AI features):

```bash
dotnet user-secrets init
dotnet user-secrets set "AzureOpenAI:Endpoint" "https://<your-resource>.openai.azure.com/"
dotnet user-secrets set "AzureOpenAI:ApiKey" "<your-api-key>"
dotnet user-secrets set "AzureOpenAI:DeploymentName" "gpt-4o"
```

4. Apply database migrations:

```bash
dotnet ef database update
```

5. Run the app:

```bash
dotnet run --project HealthManagement.csproj
```

Then open the local URL shown in terminal (typically `https://localhost:xxxx`).

## Configuration

The main config file is `HealthManagement/appsettings.json`:

- `ConnectionStrings:DefaultConnection` (default: SQLite `healthmanagement.db`)
- `AzureOpenAI:Endpoint`
- `AzureOpenAI:ApiKey`
- `AzureOpenAI:DeploymentName`

For local development, prefer `dotnet user-secrets` for sensitive values.

## Deployment (Azure)

This project is already configured for Azure Developer CLI.

From `HealthManagement/`:

```bash
azd env select healthmanagementapp
azd deploy --environment healthmanagementapp --service health-management
```

If infrastructure also changed:

```bash
azd up --environment healthmanagementapp --no-prompt
```

More details: `HealthManagement/AzureDeployment.md`

## Useful Commands

```bash
dotnet build HealthManagement/HealthManagement.csproj
dotnet watch run --project HealthManagement/HealthManagement.csproj
dotnet publish HealthManagement/HealthManagement.csproj
```

## Notes

- Authentication UI is scaffolded through ASP.NET Core Identity.
- AI-related features require valid Azure OpenAI configuration.
- Recent updates include login screen UX and static asset conflict fixes.

## Contributing

1. Fork the repository
2. Create a feature branch
3. Commit your changes
4. Open a pull request

---

If you want, I can also add badges (build status, .NET version, Azure) and a small screenshots section for better GitHub presentation.
