# Restores, builds and runs the EmployeeManagementApi.

[CmdletBinding()]
param(
    [string]$ProjectPath
)

if (-not $ProjectPath) {
    $ProjectPath = Join-Path $PSScriptRoot "../EmployeeManagementApi/EmployeeManagementApi.csproj"
}

Write-Host "Restoring packages..." -ForegroundColor Cyan
dotnet restore $ProjectPath

Write-Host "Building project..." -ForegroundColor Cyan
dotnet build $ProjectPath

Write-Host "Running project..." -ForegroundColor Cyan
dotnet run --no-build --project $ProjectPath
