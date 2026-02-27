# Project Structure

The HealthManagement application follows a layered architecture pattern for clean code organization and separation of concerns.

## Folder Structure

```
HealthManagement/
├── Controllers/              # MVC Controllers - Handle HTTP requests and responses
├── Views/                    # Razor views - UI templates for web pages
├── Models/                   # Domain models
│   ├── ApplicationUser.cs    # Extended IdentityUser with profile fields
│   ├── FoodLog.cs           # Food intake entity
│   └── ExerciseLog.cs       # Exercise activity entity
├── Data/                     # Data access layer
│   └── ApplicationDbContext.cs  # Entity Framework Core DbContext
├── Repositories/            # Repository pattern for data access
│   ├── IRepository.cs       # Generic repository interface
│   ├── GenericRepository.cs # Base repository implementation
│   ├── IFoodLogRepository.cs
│   ├── FoodLogRepository.cs
│   ├── IExerciseLogRepository.cs
│   └── ExerciseLogRepository.cs
├── Services/                # Business logic services
│   ├── IFoodLogService.cs
│   ├── FoodLogService.cs
│   ├── IExerciseLogService.cs
│   └── ExerciseLogService.cs
├── Settings/                # Configuration classes
│   └── AzureOpenAISettings.cs
├── wwwroot/                 # Static files (CSS, JS, images)
├── Program.cs               # Application startup configuration
└── appsettings.json         # Configuration settings
```

## Architecture Layers

### 1. **Presentation Layer** (Controllers & Views)
- Handles HTTP requests
- Renders Razor views
- No direct database access

### 2. **Service Layer** (Services)
- Contains business logic
- Coordinates repositories
- Performs calculations and validations
- Consumed by controllers

### 3. **Repository Layer** (Repositories)
- Abstracts data access
- Implements CRUD operations
- Uses Entity Framework Core
- Returns domain entities

### 4. **Data Layer** (Models & DbContext)
- Entity definitions
- Database context configuration
- Relationships and constraints

### 5. **Configuration & Settings**
- Azure OpenAI settings
- Identity configuration
- Database connection strings

## Dependency Injection

Services are registered in Program.cs with appropriate lifetimes:
- **Scoped**: Repositories and Services (one per HTTP request)
- **Singleton**: Azure OpenAI Client (thread-safe, reusable)

## Common Development Workflow

1. **Create Entity** → Add to Models/
2. **Create Repository** → Implement IRepository<T> in Repositories/
3. **Create Service** → Implement business logic in Services/
4. **Create Controller** → Use injected services for data operations
5. **Create Views** → Razor views in Views/

## Key Technologies

- **ASP.NET MVC Core 8** - Web framework
- **Entity Framework Core 8** - ORM
- **SQLite** - Database
- **Azure OpenAI** - AI services
- **ASP.NET Identity** - Authentication & authorization
