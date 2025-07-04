# This script creates a Linux-based Free App Service plan using Azure CLI.
# Resource group is required. Plan name and location are predefined for East Asia.

param(
    
    [string]$ResourceGroupName="eastasia-rg1" # Default resource group name, can be overridden
)

$planName = "eastasia-app-service-plan"
$location = "East Asia"
$sku = "F1"  # Free tier

# Ensure the resource group exists in the target location
if (-not (az group exists --name $ResourceGroupName)) {
    Write-Host "Creating resource group '$ResourceGroupName' in '$location'..." -ForegroundColor Cyan
    az group create --name $ResourceGroupName --location $location | Out-Null
}

Write-Host "Creating App Service Plan '$planName' in '$location' for resource group '$ResourceGroupName'..." -ForegroundColor Cyan

az appservice plan create `
    --name $planName `
    --resource-group $ResourceGroupName `
    --location $location `
    --sku $sku `
    --is-linux

Write-Host "App Service Plan creation command executed." -ForegroundColor Green
