# Azure OpenAI Configuration Guide

This application uses Azure OpenAI for AI features like food recognition and nutrient extraction.

## Setup Instructions

### 1. Get Azure OpenAI Credentials
- Visit [Azure Portal](https://portal.azure.com)
- Create or access your Azure OpenAI resource
- Get your: **Endpoint** and **API Key**

### 2. Configure Credentials (Local Development)

**Option A: User Secrets (Recommended for Development)**

Navigate to project directory and initialize user secrets:

```bash
cd ./HealthManagement
dotnet user-secrets init
```

Store your credentials:

```bash
dotnet user-secrets set "AzureOpenAI:Endpoint" "https://your-resource.openai.azure.com/"
dotnet user-secrets set "AzureOpenAI:ApiKey" "your-api-key"
dotnet user-secrets set "AzureOpenAI:DeploymentName" "gpt-4o"
```

**Option B: Environment Variables**

Set environment variables:
- `AzureOpenAI__Endpoint`
- `AzureOpenAI__ApiKey`
- `AzureOpenAI__DeploymentName`

### 3. Deployment to Azure

For production deployment, use:
- **Azure Key Vault** to store API keys
- **Managed Identity** (DefaultAzureCredential) for authentication
- API Key will be empty in configuration; system will use managed identity

## Verification

Test the configuration by:
1. Running the application: `dotnet run`
2. Check for startup errors in console
3. Azure OpenAI client should initialize successfully if credentials are valid

## Models Required

Ensure these models are deployed in your Azure OpenAI resource:
- **gpt-4o** - For text analysis and nutrient extraction
- **gpt-4-vision** - For image analysis of food photos (optional for initial setup)
