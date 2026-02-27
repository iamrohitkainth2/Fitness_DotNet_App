
# **Health Management Web Application – Requirements Document**

### *(ASP.NET MVC + SQLite + Azure OpenAI)*

***

# **1. Introduction**

## **1.1 Purpose**

This document defines the functional, technical, and architectural requirements for a **Health Management Web Application** built using:

*   **ASP.NET MVC** (C#)
*   **SQLite LocalDB** for storage
*   **Azure OpenAI (GPT‑4o & Vision models)** for all AI features

The system enables users to track:

*   Daily exercises
*   Food intake
*   Automatic calorie & micronutrient extraction

***

# **2. Technology Stack (Updated to .NET)**

## **2.1 Backend**

| Layer          | Technology                          |
| -------------- | ----------------------------------- |
| Framework      | **ASP.NET MVC 8**                   |
| Language       | C#                                  |
| Runtime        | .NET 8                              |
| AI Integration | **Azure OpenAI SDK for .NET**       |
| ORM            | Entity Framework Core 8             |
| Database       | **SQLite LocalDB** (local .db file) |
| Authentication | ASP.NET Identity (SQLite provider)  |

## **2.2 Frontend**

*   Razor Views (MVC)
*   Bootstrap / Tailwind (optional)
*   jQuery or React (optional)

## **2.3 Azure AI**

*   **Azure OpenAI GPT‑4o** → nutritional extraction, NLP
*   **Azure OpenAI Vision** → food recognition from images
*   **Azure APIM (optional)** → secure API gateway

***

# **3. Functional Requirements**

## **3.1 User Management**

| ID   | Requirement                                                       |
| ---- | ----------------------------------------------------------------- |
| FR‑1 | Users can register/login via ASP.NET Identity (stored in SQLite). |
| FR‑2 | Passwords hashed using ASP.NET Identity security.                 |
| FR‑3 | User profile fields: age, weight, height, dietary goal.           |
| FR‑4 | JWT not required unless external API integration needed.          |

***

## **3.2 Food Intake Management**

Core module using Azure OpenAI.

| ID    | Requirement                                                  |
| ----- | ------------------------------------------------------------ |
| FR‑5  | Users enter food items via text into MVC form.               |
| FR‑6  | Users upload meal images (processed by Azure OpenAI Vision). |
| FR‑7  | Azure OpenAI identifies food items from text.                |
| FR‑8  | Azure OpenAI identifies food items from images.              |
| FR‑9  | Azure OpenAI extracts macro & micronutrient estimates.       |
| FR‑10 | System displays nutrients for user review.                   |
| FR‑11 | User saves food log into SQLite DB.                          |

### **Nutrients Extracted**

*   Calories
*   Carbs, Protein, Fat
*   Vitamins A, B1–B12, C, D, E, K
*   Minerals: Iron, Calcium, Magnesium, Potassium, Zinc

***

## **3.3 Exercise Management**

| ID    | Requirement                                            |
| ----- | ------------------------------------------------------ |
| FR‑12 | Users enter exercise name, duration, intensity.        |
| FR‑13 | System calculates calories burned using formula in C#. |
| FR‑14 | Exercise logs stored in SQLite.                        |
| FR‑15 | Azure OpenAI (optional) can estimate MET values.       |

***

## **3.4 Dashboard & Insights**

| Feature                | Description                            |
| ---------------------- | -------------------------------------- |
| Daily summary          | Calories intake vs calories burned     |
| Weekly trends          | Graphs for nutrients, macros, activity |
| AI insights            | Personalized guidance via Azure OpenAI |
| Progress visualization | Charts via Chart.js                    |

***

# **4. Non‑Functional Requirements**

## **4.1 Security**

*   HTTPS enforced
*   ASP.NET Anti-forgery tokens
*   Password hashing via ASP.NET Identity
*   Secrets stored in **User Secrets** or Azure Key Vault (when deployed)

## **4.2 Performance**

*   Azure OpenAI responses < 5 seconds
*   MVC pages load < 2 seconds

## **4.3 Scalability**

*   SQLite supports local use; scalable environments may switch to Azure SQL later
*   Azure OpenAI scales automatically

## **4.4 Reliability**

*   LocalDB auto-backup optional
*   App Service or IIS deployable

***

# **5. System Architecture (Updated)**

    +---------------------+
    |     MVC Views       |
    |  (Razor UI Layer)   |
    +----------+----------+
               |
               v
    +---------------------+
    | ASP.NET MVC Layer   |
    | Controllers         |
    | Services (Business) |
    | Repositories (EF)   |
    +----------+----------+
               |
               v
    +---------------------+
    | SQLite LocalDB      |
    | Entity Framework    |
    +---------------------+

    For AI Tasks →
    +------------------------------------------+
    |       Azure OpenAI (GPT‑4o + Vision)     |
    |  - Food recognition                      |
    |  - Nutrient extraction                   |
    |  - Recommendations                       |
    +------------------------------------------+

***

# **6. Database Schema (SQLite LocalDB)**

### **Users Table** (Identity)

*   Id
*   Username
*   Email
*   PasswordHash
*   Age, Weight, Height, Goal

### **FoodLogs**

*   Id
*   UserId
*   FoodName
*   Calories
*   Carbs
*   Protein
*   Fat
*   Micronutrients (JSON field)
*   DateLogged

### **ExerciseLogs**

*   Id
*   UserId
*   ExerciseName
*   DurationMinutes
*   CaloriesBurned
*   DateLogged

***

# **7. Azure OpenAI Integration (C# Example)**

### **Text Nutrient Extraction Endpoint**

```csharp
var client = new Azure.AI.OpenAI.AzureOpenAIClient(
    new Uri("https://YOUR-RESOURCE-NAME.openai.azure.com/"),
    new DefaultAzureCredential()
);

var response = client.GetChatCompletions(
    deploymentOrModelName: "gpt-4o",
    new ChatCompletionsOptions
    {
        Messages =
        {
            new ChatRequestMessage("user", "1 bowl of rice and chicken curry")
        }
    }
);

var result = response.Value.Choices[0].Message.Content;
```

***

# **8. Future Enhancements**

*   Sync with Apple Health / Google Fit
*   Azure OpenAI meal planning
*   Voice input for food/exercise
*   Offline desktop mode using local SQLite


