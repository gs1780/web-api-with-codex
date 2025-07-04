# Builds and runs the EmployeeManagementApi and launches the base URL when ready.

[CmdletBinding()]
param(
    [string]$ProjectPath = "../EmployeeManagementApi/EmployeeManagementApi.csproj",
    [string]$BaseUrl = "http://localhost:5000"
)

function Wait-Port {
    param([string]$Url)
    $uri = [System.Uri]$Url
    $host = $uri.Host
    $port = $uri.Port
    while ($true) {
        try {
            $client = New-Object System.Net.Sockets.TcpClient
            $client.Connect($host, $port)
            $client.Dispose()
            break
        } catch {
            Start-Sleep -Seconds 1
        }
    }
}

Write-Host "Restoring packages..." -ForegroundColor Cyan
& dotnet restore $ProjectPath | Out-Null

Write-Host "Building project..." -ForegroundColor Cyan
& dotnet build $ProjectPath | Out-Null

$env:ASPNETCORE_URLS = $BaseUrl

Write-Host "Starting application..." -ForegroundColor Cyan
$process = Start-Process "dotnet" "run --no-build --project `"$ProjectPath`"" -PassThru -NoNewWindow

Wait-Port $BaseUrl

Write-Host "Server is running at $BaseUrl" -ForegroundColor Green
Start-Process $BaseUrl

$process.WaitForExit()
