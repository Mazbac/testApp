$ErrorActionPreference = 'Stop'
$root = Split-Path -Parent $PSScriptRoot
Set-Location $root

node .automation/validate.mjs
node .automation/context-integrity.mjs

dotnet restore QuickShelf.slnx --locked-mode
dotnet format QuickShelf.slnx --verify-no-changes --no-restore
dotnet build QuickShelf.slnx -c Release --no-restore -warnaserror
dotnet test tests/QuickShelf.Tests/QuickShelf.Tests.csproj -c Release --no-build --filter TestCategory=Unit
dotnet test tests/QuickShelf.Tests/QuickShelf.Tests.csproj -c Release --no-build --filter TestCategory=Integration
dotnet list QuickShelf.slnx package --vulnerable --include-transitive

Write-Host "QuickShelf verification OK"
