param location string
param resourceToken string
param tags object
param appServicePlanName string = ''
param appServiceName string = ''

resource appServicePlan 'Microsoft.Web/serverfarms@2023-12-01' = {
  name: !empty(appServicePlanName) ? appServicePlanName : 'plan-${resourceToken}'
  location: location
  kind: 'linux'
  sku: {
    name: 'B1'
    tier: 'Basic'
    size: 'B1'
    family: 'B'
    capacity: 1
  }
  properties: {
    reserved: true
  }
  tags: tags
}

resource healthManagementApp 'Microsoft.Web/sites@2023-12-01' = {
  name: !empty(appServiceName) ? appServiceName : 'app-health-management-${resourceToken}'
  location: location
  kind: 'app,linux'
  identity: {
    type: 'SystemAssigned'
  }
  properties: {
    serverFarmId: appServicePlan.id
    httpsOnly: true
    siteConfig: {
      linuxFxVersion: 'DOTNETCORE|8.0'
      alwaysOn: true
      minTlsVersion: '1.2'
      ftpsState: 'Disabled'
      appSettings: [
        {
          name: 'ASPNETCORE_ENVIRONMENT'
          value: 'Production'
        }
      ]
    }
  }
  tags: union(tags, {
    'azd-service-name': 'health-management'
  })
}

output AZURE_RESOURCE_HEALTH_MANAGEMENT_ID string = healthManagementApp.id
output SERVICE_HEALTH_MANAGEMENT_NAME string = healthManagementApp.name
output SERVICE_HEALTH_MANAGEMENT_URI string = 'https://${healthManagementApp.properties.defaultHostName}'