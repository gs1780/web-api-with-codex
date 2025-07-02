<#
.SYNOPSIS
Deploys the EmployeeManagementApi to an Azure Web App.


.PARAMETER ResourceGroup
Name of the Azure resource group. Defaults to india-app-service-rg.

.PARAMETER WebAppSuffix
Five-digit suffix appended to the Web App name.

.PARAMETER Location
Azure region for the resources. Defaults to Central India.

.PARAMETER Sku
App Service plan pricing tier. Defaults to F1.
#>

param(
    [string]$ResourceGroup = "india-app-service-rg",
    [Parameter(Mandatory=$true)][string]$WebAppSuffix,
    [string]$Location = "Central India",
    [string]$Sku = "F1"
)

$WebAppName = "india-webapi-$WebAppSuffix"
$planName = "india-app-plan-3749"

$ErrorActionPreference = 'Stop'

Write-Host "Creating App Service plan $planName in resource group $ResourceGroup"
az appservice plan create `
    --name $planName `
    --resource-group $ResourceGroup `
    --sku $Sku `
    --is-linux `
    --location $Location

Write-Host "Creating Web App $WebAppName"
az webapp create `
    --resource-group $ResourceGroup `
    --plan $planName `
    --name $WebAppName `
    --runtime "DOTNETCORE|8.0"

$publishFolder = Join-Path $PSScriptRoot 'publish'
$zipPath = Join-Path $PSScriptRoot 'app.zip'

if (Test-Path $publishFolder) { Remove-Item $publishFolder -Recurse -Force }
if (Test-Path $zipPath) { Remove-Item $zipPath -Force }

$projectPath = Join-Path $PSScriptRoot '../EmployeeManagementApi'

Write-Host "Publishing project..."
dotnet publish $projectPath -c Release -o $publishFolder

Write-Host "Creating deployment archive..."
Compress-Archive -Path (Join-Path $publishFolder '*') -DestinationPath $zipPath

Write-Host "Deploying to Azure..."
az webapp deploy `
    --resource-group $ResourceGroup `
    --name $WebAppName `
    --src-path $zipPath `
    --type zip

Write-Host "Deployment completed"
