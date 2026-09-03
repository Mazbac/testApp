param(
    [string]$InstallerPath = (Join-Path (Split-Path -Parent $PSScriptRoot) 'dist\QuickShelf-Setup-0.1.0.exe')
)

$ErrorActionPreference = 'Stop'
$installDir = Join-Path $env:LOCALAPPDATA 'Programs\QuickShelf'
$dataDir = Join-Path $env:LOCALAPPDATA 'QuickShelf'
$menuDir = Join-Path $env:APPDATA 'Microsoft\Windows\Start Menu\Programs\QuickShelf'
$uninstallKey = 'HKCU:\Software\Microsoft\Windows\CurrentVersion\Uninstall\QuickShelf'
$installedExe = Join-Path $installDir 'QuickShelf.exe'
$uninstaller = Join-Path $installDir 'Uninstall.exe'

if (-not (Test-Path $InstallerPath)) { throw "Installer not found: $InstallerPath" }
if ((Test-Path $installDir) -or (Test-Path $dataDir) -or (Test-Path $uninstallKey)) {
    throw 'Lifecycle test requires a clean QuickShelf install/data state and will not overwrite existing user state.'
}
if (Get-Process QuickShelf -ErrorAction SilentlyContinue) { throw 'Close running QuickShelf instances before lifecycle testing.' }

Add-Type -AssemblyName UIAutomationClient
Add-Type -AssemblyName UIAutomationTypes
$process = $null

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

function Install-QuickShelf {
    $result = Start-Process -FilePath $InstallerPath -ArgumentList '/S' -Wait -PassThru
    if ($result.ExitCode -ne 0) { throw "Installer exited with code $($result.ExitCode)." }
    if (-not (Test-Path $installedExe)) { throw 'Installed executable is missing.' }
    if (-not (Test-Path $uninstaller)) { throw 'Uninstaller is missing.' }
    if (-not (Test-Path $uninstallKey)) { throw 'Add/Remove Programs registration is missing.' }
    if (-not (Test-Path (Join-Path $menuDir 'QuickShelf.lnk'))) { throw 'Start Menu shortcut is missing.' }
}

function Uninstall-QuickShelf {
    $result = Start-Process -FilePath $uninstaller -ArgumentList '/S' -Wait -PassThru
    if ($result.ExitCode -ne 0) { throw "Uninstaller exited with code $($result.ExitCode)." }
    Start-Sleep -Milliseconds 250
    if (Test-Path $installDir) { throw 'Install directory remained after uninstall.' }
    if (Test-Path $uninstallKey) { throw 'Uninstall registry entry remained after uninstall.' }
    if (Test-Path $menuDir) { throw 'Start Menu folder remained after uninstall.' }
}

try {
    Install-QuickShelf
    $process = Start-Process -FilePath $installedExe -PassThru
    $window = Wait-Window $process.Id
    Invoke-Element (Find-ByName $window 'Create new snippet')
    Start-Sleep -Milliseconds 200
    Set-ElementValue (Find-ByName $window 'Snippet title') 'Lifecycle preservation note'
    Set-ElementValue (Find-ByName $window 'Snippet content') 'Created from the installed release artifact.'
    Start-Sleep -Milliseconds 1200
    Close-Window $window
    $process.WaitForExit(5000) | Out-Null
    if (-not $process.HasExited) { throw 'Installed QuickShelf did not close cleanly.' }

    $statePath = Join-Path $dataDir 'quickshelf.json'
    if (-not (Test-Path $statePath)) { throw 'Installed app did not persist its state.' }
    Uninstall-QuickShelf
    if (-not (Test-Path $statePath)) { throw 'Uninstall deleted user-authored QuickShelf data.' }

    Install-QuickShelf
    $process = Start-Process -FilePath $installedExe -PassThru
    $window = Wait-Window $process.Id
    $title = Get-ElementValue (Find-ByName $window 'Snippet title')
    if ($title -ne 'Lifecycle preservation note') {
        throw "Reinstalled app did not restore preserved data. Title was '$title'."
    }
    Close-Window $window
    $process.WaitForExit(5000) | Out-Null
    if (-not $process.HasExited) { throw 'Reinstalled QuickShelf did not close cleanly.' }

    Uninstall-QuickShelf
    if (-not (Test-Path $statePath)) { throw 'Final uninstall unexpectedly removed user data.' }
    Remove-Item $dataDir -Recurse -Force
    Write-Host 'Installer lifecycle install/use/uninstall/reinstall preservation test OK'
}
catch {
    throw
}
finally {
    if ($process -and -not $process.HasExited) {
        Stop-Process -Id $process.Id -Force -ErrorAction SilentlyContinue
    }
}
