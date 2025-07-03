Param(
    [string]$ResourceGroup = "india-app-service-rg",
    [string]$WebAppName = "webapi-3749",
    [string]$ZipPath = (Join-Path $PSScriptRoot 'app.zip')
)

az webapp deploy `
  --resource-group $ResourceGroup `
  --name $WebAppName `
  --src-path $ZipPath `
  --type zip

