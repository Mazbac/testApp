param(
    [switch]$SkipRestore,
    [switch]$SkipE2E
)

$ErrorActionPreference = 'Stop'
$root = Split-Path -Parent $PSScriptRoot
Push-Location $root
try {
    if (-not $SkipRestore) {
        dotnet restore QuickShelf.slnx --locked-mode
        if ($LASTEXITCODE -ne 0) { throw 'Restore failed.' }
    }

    node .automation/validate.mjs
    if ($LASTEXITCODE -ne 0) { throw 'Repository validation failed.' }
    node .automation/context-integrity.mjs
    if ($LASTEXITCODE -ne 0) { throw 'Context integrity failed.' }

    dotnet format QuickShelf.slnx --verify-no-changes --no-restore
    if ($LASTEXITCODE -ne 0) { throw 'Formatting gate failed.' }

    dotnet build QuickShelf.slnx -c Release --no-restore -warnaserror
    if ($LASTEXITCODE -ne 0) { throw 'Release build failed.' }

    dotnet test tests/QuickShelf.Tests/QuickShelf.Tests.csproj -c Release --no-build
    if ($LASTEXITCODE -ne 0) { throw 'Tests failed.' }


    & "$PSScriptRoot\security-scan.ps1"
    if ($LASTEXITCODE -ne 0) { throw 'Security scan failed.' }

    if (-not $SkipE2E) {
        & "$PSScriptRoot\e2e.ps1"
        if ($LASTEXITCODE -ne 0) { throw 'E2E smoke test failed.' }
    }

    Write-Host 'QuickShelf verification OK'
}
finally {
    Pop-Location
}
