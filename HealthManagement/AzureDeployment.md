# Azure Redeployment (Same Resource Group and App Service)

Use these steps to redeploy the **HealthManagement** app to the **same** Azure Resource Group and App Service.

## 1) Open the project folder

```powershell
cd C:\Users\Admin\source\Fitness_DotNet_App\HealthManagement
```

## 2) Select the same azd environment

```powershell
azd env select healthmanagementapp
azd env get-values
```

## 3) Verify target resources before deploy (optional)

```powershell
azd show --environment healthmanagementapp
```

Expected target:
- Resource Group: `rg-healthmanagementapp`
- Service: `health-management`
- App endpoint pattern: `https://app-health-management-<suffix>.azurewebsites.net/`

## 4) Redeploy application code only (fastest)

Use this when only app code changed:

```powershell
azd deploy --environment healthmanagementapp --service health-management
```

## 5) Redeploy infrastructure + code (if infra changed)

Use this when `infra/` or `azure.yaml` changed:

```powershell
azd up --environment healthmanagementapp --no-prompt
```

## 6) Validate deployment

```powershell
azd show --environment healthmanagementapp
```

## Quick troubleshooting

- If environment is wrong:

```powershell
azd env list
azd env select healthmanagementapp
```

- If login/session expired:

```powershell
azd auth login
```

- If subscription must be explicit:

```powershell
azd deploy --environment healthmanagementapp --service health-management --subscription a1aa0e4d-0915-48a4-8c5a-f1410fc99911
```
