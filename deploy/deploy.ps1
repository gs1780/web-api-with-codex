<#
.SYNOPSIS
Deploys the EmployeeManagementApi to an Azure Web App.

.PARAMETER ResourceGroup
Name of the Azure resource group.

.PARAMETER WebAppName
Name of the Web App to create.

.PARAMETER Location
Azure region for the resources. Defaults to eastus.

.PARAMETER Sku
App Service plan pricing tier. Defaults to F1.
#>

param(
    [Parameter(Mandatory=$true)][string]$ResourceGroup,
    [Parameter(Mandatory=$true)][string]$WebAppName,
    [string]$Location = "eastus",
    [string]$Sku = "F1"
)

$ErrorActionPreference = 'Stop'

$planName = "$WebAppName-plan"

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
