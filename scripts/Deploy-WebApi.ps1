# Deploys the EmployeeManagementApi to Azure App Service

param(
    [string]$ResourceGroup = "eastasia-rg1",
    [string]$PlanName = "eastasia-app-service-plan",
    [string]$WebAppName = "eastasia-webapi",
    [string]$ProjectPath = "../EmployeeManagementApi/EmployeeManagementApi.csproj",
    [string]$Location = "East Asia"
)


# Publish output directory and zip package path
$publishDir = Join-Path $PSScriptRoot "publish"
$zipPath = Join-Path $PSScriptRoot "webapp.zip"

Write-Host "Publishing application..." -ForegroundColor Cyan
& dotnet publish $ProjectPath -c Release -o $publishDir

if (Test-Path $zipPath) { Remove-Item $zipPath }

# Create zip using forward slashes when running on Linux
if (Get-Command zip -ErrorAction SilentlyContinue) {
    Push-Location $publishDir
    & zip -r $zipPath . | Out-Null
    Pop-Location
} else {
    Compress-Archive -Path "$publishDir/*" -DestinationPath $zipPath -Force
}

if (-not (az group exists --name $ResourceGroup)) {
    Write-Host "Creating resource group $ResourceGroup" -ForegroundColor Cyan
    az group create --name $ResourceGroup --location $Location | Out-Null
}

if (-not (az appservice plan show --name $PlanName --resource-group $ResourceGroup 2>$null)) {
    Write-Host "Creating App Service plan $PlanName" -ForegroundColor Cyan
    az appservice plan create --name $PlanName --resource-group $ResourceGroup --location $Location --sku F1 --is-linux | Out-Null
}

if (-not (az webapp show --name $WebAppName --resource-group $ResourceGroup 2>$null)) {
    Write-Host "Creating Web App $WebAppName" -ForegroundColor Cyan
    az webapp create --name $WebAppName --resource-group $ResourceGroup --plan $PlanName --runtime "DOTNETCORE:8.0" | Out-Null
}

Write-Host "Deploying package to $WebAppName" -ForegroundColor Cyan
az webapp deploy --resource-group $ResourceGroup --name $WebAppName --src-path $zipPath --type zip | Out-Null

Write-Host "Deployment complete." -ForegroundColor Green
