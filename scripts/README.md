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

Restores, builds and runs the EmployeeManagementApi project.

### Usage

```powershell
./Run-WebApi.ps1
```

An optional parameter lets you specify the project path.

## Deploy-WebApi.ps1

Publishes the EmployeeManagementApi project and deploys it to an Azure App Service. The script ensures the web app exists in the **eastasia-rg1** resource group using the **eastasia-app-service-plan**.

### Usage

```powershell
./Deploy-WebApi.ps1
```

Parameters allow overriding the resource group, plan and app name. The default app name is **eastasia-webapi**.

The deployment package is generated using the .NET `ZipFile` API so the entries
within the archive always use forward slashes. This ensures compatibility across
different operating systems.
