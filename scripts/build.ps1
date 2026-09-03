param(
    [string]$Version = '0.1.0'
)

$ErrorActionPreference = 'Stop'
$root = Split-Path -Parent $PSScriptRoot
$publish = Join-Path $root 'artifacts\publish'
$dist = Join-Path $root 'dist'
Push-Location $root
try {
    Remove-Item $publish -Recurse -Force -ErrorAction SilentlyContinue
    Remove-Item $dist -Recurse -Force -ErrorAction SilentlyContinue
    New-Item -ItemType Directory -Force $publish, $dist | Out-Null

    dotnet restore src/QuickShelf/QuickShelf.csproj -r win-x64 --locked-mode
    if ($LASTEXITCODE -ne 0) { throw 'Release restore failed.' }

    dotnet publish src/QuickShelf/QuickShelf.csproj -c Release -r win-x64 --self-contained true `
        --no-restore -o $publish -p:PublishSingleFile=true -p:IncludeNativeLibrariesForSelfExtract=true `
        -p:DebugType=None -p:DebugSymbols=false -p:Version=$Version
    if ($LASTEXITCODE -ne 0) { throw 'Self-contained publish failed.' }

    $makensisCandidates = @(
        $env:MAKENSIS_PATH,
        "$env:ProgramFiles(x86)\NSIS\makensis.exe",
        "$env:ProgramFiles\NSIS\makensis.exe"
    ) | Where-Object { $_ -and (Test-Path $_) }
    $makensis = $makensisCandidates | Select-Object -First 1
    if (-not $makensis) { throw 'NSIS makensis.exe was not found. Set MAKENSIS_PATH or install NSIS.' }


    & $makensis "/DVERSION=$Version" "/DPUBLISH_DIR=$publish" "/DOUTPUT_DIR=$dist" "/DICON_PATH=$root\src\QuickShelf\Assets\QuickShelf.ico" installer/QuickShelf.nsi
    if ($LASTEXITCODE -ne 0) { throw 'NSIS packaging failed.' }

    $installer = Join-Path $dist "QuickShelf-Setup-$Version.exe"
    if (-not (Test-Path $installer)) { throw "Expected installer was not produced: $installer" }
    $hash = (Get-FileHash $installer -Algorithm SHA256).Hash.ToLowerInvariant()
    Set-Content -Path "$installer.sha256" -Value "$hash  QuickShelf-Setup-$Version.exe" -Encoding ascii

    Write-Host "Built $installer"
    Write-Host "SHA256 $hash"
}
finally {
    Pop-Location
}
