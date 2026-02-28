targetScope = 'subscription'

@minLength(1)
@maxLength(64)
@description('Name of the environment used to generate unique resource names.')
param environmentName string

@minLength(1)
@description('Primary location for all resources.')
param location string

@description('Optional override for resource group name.')
param resourceGroupName string = ''

@description('Optional override for app service plan name.')
param appServicePlanName string = ''

@description('Optional override for web app name.')
param appServiceName string = ''

var resourceToken = toLower(uniqueString(subscription().id, environmentName, location))
var tags = {
  'azd-env-name': environmentName
}

resource rg 'Microsoft.Resources/resourceGroups@2021-04-01' = {
  name: !empty(resourceGroupName) ? resourceGroupName : 'rg-${environmentName}'
  location: location
  tags: tags
}

module app './modules/appservice.bicep' = {
  name: 'health-management-app'
  scope: rg
  params: {
    location: location
    resourceToken: resourceToken
    tags: tags
    appServicePlanName: appServicePlanName
    appServiceName: appServiceName
  }
}

output AZURE_RESOURCE_GROUP string = rg.name
output AZURE_RESOURCE_HEALTH_MANAGEMENT_ID string = app.outputs.AZURE_RESOURCE_HEALTH_MANAGEMENT_ID
output SERVICE_HEALTH_MANAGEMENT_NAME string = app.outputs.SERVICE_HEALTH_MANAGEMENT_NAME
output SERVICE_HEALTH_MANAGEMENT_URI string = app.outputs.SERVICE_HEALTH_MANAGEMENT_URI