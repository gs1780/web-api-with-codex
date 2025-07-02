Param(
    [string]$Configuration = "Release"
)

$projectPath = Join-Path $PSScriptRoot "EmployeeManagementApi"

pushd $projectPath

dotnet restore

dotnet build -c $Configuration

dotnet run --no-build -c $Configuration

popd
