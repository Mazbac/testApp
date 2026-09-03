param(
    [string]$ExePath
)

$ErrorActionPreference = 'Stop'
$root = Split-Path -Parent $PSScriptRoot
if (-not $ExePath) {
    $ExePath = Join-Path $root 'src\QuickShelf\bin\Release\net10.0-windows\win-x64\QuickShelf.exe'
}
if (-not (Test-Path $ExePath)) { throw "QuickShelf executable not found: $ExePath" }
if (Get-Process QuickShelf -ErrorAction SilentlyContinue) { throw 'Close running QuickShelf instances before E2E testing.' }

Add-Type -AssemblyName UIAutomationClient
Add-Type -AssemblyName UIAutomationTypes
$dataDir = Join-Path $env:LOCALAPPDATA 'QuickShelf'
$backupDir = Join-Path $env:TEMP ("QuickShelf-e2e-backup-" + [guid]::NewGuid().ToString('N'))
$process = $null
$hadData = Test-Path $dataDir

function Wait-Window([int]$processId) {
    $condition = [System.Windows.Automation.PropertyCondition]::new(
        [System.Windows.Automation.AutomationElement]::ProcessIdProperty, $processId)
    for ($i = 0; $i -lt 50; $i++) {
        $window = [System.Windows.Automation.AutomationElement]::RootElement.FindFirst(
            [System.Windows.Automation.TreeScope]::Children, $condition)
        if ($window) { return $window }
        Start-Sleep -Milliseconds 100
    }
    throw "QuickShelf window did not appear for process $processId."
}

function Find-ByName($rootElement, [string]$name) {
    $condition = [System.Windows.Automation.PropertyCondition]::new(
        [System.Windows.Automation.AutomationElement]::NameProperty, $name)
    for ($i = 0; $i -lt 30; $i++) {
        $element = $rootElement.FindFirst([System.Windows.Automation.TreeScope]::Descendants, $condition)
        if ($element) { return $element }
        Start-Sleep -Milliseconds 100
    }
    throw "Automation element not found: $name"
}

function Invoke-Element($element) {
    $pattern = $element.GetCurrentPattern([System.Windows.Automation.InvokePattern]::Pattern)
    $pattern.Invoke()
}

function Set-ElementValue($element, [string]$value) {
    $pattern = $element.GetCurrentPattern([System.Windows.Automation.ValuePattern]::Pattern)
    $pattern.SetValue($value)
}

function Get-ElementValue($element) {
    $pattern = $element.GetCurrentPattern([System.Windows.Automation.ValuePattern]::Pattern)
    return $pattern.Current.Value
}

function Close-Window($window) {
    $pattern = $window.GetCurrentPattern([System.Windows.Automation.WindowPattern]::Pattern)
    $pattern.Close()
}

try {
    if ($hadData) { Move-Item $dataDir $backupDir }

    $process = Start-Process -FilePath $ExePath -PassThru
    $window = Wait-Window $process.Id
    Invoke-Element (Find-ByName $window 'Create new snippet')
    Start-Sleep -Milliseconds 200
    Set-ElementValue (Find-ByName $window 'Snippet title') 'E2E persistence note'
    Set-ElementValue (Find-ByName $window 'Snippet content') 'Created by the QuickShelf lifecycle smoke test.'
    Start-Sleep -Milliseconds 1200
    Close-Window $window
    $process.WaitForExit(5000) | Out-Null
    if (-not $process.HasExited) { throw 'QuickShelf did not close after the save smoke test.' }

    $statePath = Join-Path $dataDir 'quickshelf.json'
    if (-not (Test-Path $statePath)) { throw 'Autosave did not create the local state file.' }
    if ((Get-Content $statePath -Raw) -notmatch 'E2E persistence note') { throw 'Saved state does not contain the E2E note.' }

    $process = Start-Process -FilePath $ExePath -PassThru
    $window = Wait-Window $process.Id
    $title = Get-ElementValue (Find-ByName $window 'Snippet title')
    if ($title -ne 'E2E persistence note') { throw "Persistence check failed after restart. Title was '$title'." }
    Close-Window $window
    $process.WaitForExit(5000) | Out-Null
    if (-not $process.HasExited) { throw 'QuickShelf did not close after restart verification.' }

    Write-Host 'E2E first-value/persistence smoke test OK'
}
finally {
    if ($process -and -not $process.HasExited) {
        Stop-Process -Id $process.Id -Force -ErrorAction SilentlyContinue
    }
    if (Test-Path $dataDir) { Remove-Item $dataDir -Recurse -Force }
    if ($hadData -and (Test-Path $backupDir)) { Move-Item $backupDir $dataDir }
    elseif (Test-Path $backupDir) { Remove-Item $backupDir -Recurse -Force }
}
