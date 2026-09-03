param()

$ErrorActionPreference = 'Stop'
$root = Split-Path -Parent $PSScriptRoot
Push-Location $root
try {
    $patterns = @(
        '-----BEGIN (RSA |EC |OPENSSH )?PRIVATE KEY-----',
        'ghp_[A-Za-z0-9]{20,}',
        'github_pat_[A-Za-z0-9_]{20,}',
        'sk-[A-Za-z0-9]{20,}'
    )

    foreach ($pattern in $patterns) {
        $matches = & git grep -n -I -E --untracked --exclude-standard -- $pattern -- . ':(exclude)scripts/security-scan.ps1' 2>$null
        if ($LASTEXITCODE -eq 0) {
            throw "Potential secret pattern found:`n$($matches -join "`n")"
        }
        if ($LASTEXITCODE -ne 1) {
            throw 'git grep failed while scanning tracked and untracked candidate files.'
        }
    }

    $report = (& dotnet list QuickShelf.slnx package --vulnerable --include-transitive 2>&1 | Out-String)
    if ($LASTEXITCODE -ne 0) { throw "NuGet vulnerability query failed.`n$report" }
    if ($report -match 'has the following vulnerable packages') {
        throw "Vulnerable NuGet package detected.`n$report"
    }

    Write-Host 'Security scan OK (tracked + untracked candidate files)'
}
finally {
    Pop-Location
}
