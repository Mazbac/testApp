$ErrorActionPreference = 'Stop'
$root = Split-Path -Parent $PSScriptRoot
Set-Location $root

node .automation/bootstrap.mjs
$required = '10.0.400'
$actual = (dotnet --version).Trim()
if ($actual -ne $required) {
    throw "QuickShelf requires .NET SDK $required; found $actual."
}

dotnet restore QuickShelf.slnx --locked-mode
Write-Host "QuickShelf bootstrap OK"
