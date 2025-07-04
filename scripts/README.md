# Azure CLI PowerShell Scripts

This folder contains PowerShell utilities for Azure resources.

## Create-AppServicePlan.ps1

Use this script to create a Linux-based Free App Service plan in the **East Asia** region.

### Usage

```powershell
# Example
./Create-AppServicePlan.ps1 -ResourceGroupName MyResourceGroup
```

The script checks if the specified resource group exists and creates it in East Asia if needed. Then it provisions an App Service plan named **eastasia-app-service-plan** using the Free (F1) tier.

## Run-WebApi.ps1

Builds and runs the EmployeeManagementApi project. When the application is ready,
it automatically opens the configured base URL in your default browser.

### Usage

```powershell
./Run-WebApi.ps1
```

Parameters allow overriding the project path and the base URL. The default URL is
`http://localhost:5000`.
