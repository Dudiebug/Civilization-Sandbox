#requires -Version 5.1
[CmdletBinding()]
param(
    [ValidateSet('Outputs', 'FullGenerated')][string]$Scope = 'Outputs',
    [switch]$Execute,
    [string]$RepositoryRoot
)

$ErrorActionPreference = 'Stop'
Import-Module (Join-Path $PSScriptRoot 'Task001.Common.psm1') -Force
$expectedRoot = Get-Task001RepositoryRoot
if (-not $RepositoryRoot) { $RepositoryRoot = $expectedRoot }
$root = [System.IO.Path]::GetFullPath($RepositoryRoot).TrimEnd('\', '/')
if (-not $root.Equals([System.IO.Path]::GetFullPath($expectedRoot).TrimEnd('\', '/'), [System.StringComparison]::OrdinalIgnoreCase)) {
    Write-Error "CIV001-CLEAN-002: RepositoryRoot must be the repository containing this script: $expectedRoot"
    exit 1
}

$children = if ($Scope -eq 'Outputs') {
    @('Artifacts', 'Logs', 'TestResults', 'CoverageResults', 'BuildOutput', 'Builds')
} else {
    @('Artifacts', 'Logs', 'TestResults', 'CoverageResults', 'BuildOutput', 'Builds', 'Library', 'Temp', 'Obj', 'UserSettings')
}

foreach ($child in $children) {
    $target = Assert-Task001PathWithinRoot -RepositoryRoot $root -TargetPath (Join-Path $root $child)
    if (-not (Test-Path -LiteralPath $target)) { continue }
    try { Assert-Task001TreeHasNoReparsePoints -Path $target } catch { Write-Error $_.Exception.Message; exit 1 }
    if ($Execute) {
        Write-Host "REMOVE $target"
        Remove-Item -LiteralPath $target -Recurse -Force
    } else {
        Write-Host "DRY-RUN $target"
    }
}

if (-not $Execute) { Write-Host 'Dry-run only. Add -Execute to remove the listed whitelisted paths.' }
exit 0
