Param(
    [string]$Configuration = "Release"
)

$publishFolder = Join-Path $PSScriptRoot "publish"
$zipPath = Join-Path $PSScriptRoot "app.zip"
$projectPath = Join-Path $PSScriptRoot "EmployeeManagementApi"

if (Test-Path $publishFolder) { Remove-Item $publishFolder -Recurse -Force }
if (Test-Path $zipPath) { Remove-Item $zipPath -Force }

pushd $projectPath

# Restore and build the project
dotnet restore

dotnet build -c $Configuration

# Publish the project to the publish folder
popd

dotnet publish $projectPath -c $Configuration -o $publishFolder

# Create a zip archive with Linux compatible forward slashes
Add-Type -AssemblyName System.IO.Compression.FileSystem
$zip = [System.IO.Compression.ZipFile]::Open($zipPath, [System.IO.Compression.ZipArchiveMode]::Create)
Get-ChildItem -Recurse -File -Path $publishFolder | ForEach-Object {
    $relativePath = $_.FullName.Substring($publishFolder.Length + 1) -replace '\\','/'
    [System.IO.Compression.ZipFileExtensions]::CreateEntryFromFile($zip, $_.FullName, $relativePath) | Out-Null
}
$zip.Dispose()

Write-Host "Publish directory archived at $zipPath"
