#requires -Version 5.1
[CmdletBinding()]
param(
    [switch]$RepositoryOnly,
    [string]$UnityPath,
    [string]$WorkingParent
)

$ErrorActionPreference = 'Stop'
Import-Module (Join-Path $PSScriptRoot 'Task001.Common.psm1') -Force
$root = Get-Task001RepositoryRoot
$gitTool = (Get-Task001Toolchain -RepositoryRoot $root).tools | Where-Object { $_.wingetId -eq 'Git.Git' }
$git = Resolve-Task001ToolExecutable -Tool $gitTool
if (-not $git) { throw 'CIV001-CLEANCLONE-001: Pinned Git is required.' }
if (-not $WorkingParent) { $WorkingParent = Join-Path $env:LocalAppData 'CivSandboxTask001Verification' }
$WorkingParent = [System.IO.Path]::GetFullPath($WorkingParent)
New-Item -ItemType Directory -Path $WorkingParent -Force | Out-Null
$clone = Join-Path $WorkingParent ("clone-" + [guid]::NewGuid().ToString('N'))

try {
    & $git clone --no-local --branch 'task/TASK-001-bootstrap' $root $clone
    if ($LASTEXITCODE -ne 0) { throw 'CIV001-CLEANCLONE-002: Local clean clone failed.' }
    $lockBefore = Get-Task001Sha256 -Path (Join-Path $clone 'Packages\packages-lock.json')
    $bootstrap = Join-Path $clone 'Build\Bootstrap.ps1'
    $args = @('-NoProfile', '-ExecutionPolicy', 'Bypass', '-File', $bootstrap)
    if ($RepositoryOnly) { $args += '-RepositoryOnly' }
    if ($UnityPath) { $args += @('-UnityPath', $UnityPath) }
    & powershell.exe @args
    if ($LASTEXITCODE -ne 0) { throw 'CIV001-CLEANCLONE-003: First bootstrap failed.' }
    & powershell.exe @args
    if ($LASTEXITCODE -ne 0) { throw 'CIV001-CLEANCLONE-004: Second bootstrap failed.' }
    $lockAfter = Get-Task001Sha256 -Path (Join-Path $clone 'Packages\packages-lock.json')
    if ($lockBefore -ne $lockAfter) { throw 'CIV001-CLEANCLONE-005: Package lock changed across bootstrap runs.' }
    $tracked = (& $git -C $clone status --porcelain --untracked-files=no | Out-String).Trim()
    if ($tracked) { throw "CIV001-CLEANCLONE-006: Bootstrap changed tracked files: $tracked" }
    Write-Host "PASS: clean clone bootstrap was idempotent at $clone"
} finally {
    if (Test-Path -LiteralPath $clone) {
        $resolvedClone = [System.IO.Path]::GetFullPath($clone)
        if (-not $resolvedClone.StartsWith($WorkingParent.TrimEnd('\', '/') + [System.IO.Path]::DirectorySeparatorChar, [System.StringComparison]::OrdinalIgnoreCase)) {
            throw 'CIV001-CLEANCLONE-099: Refusing to remove an unexpected clone path.'
        }
        Remove-Item -LiteralPath $resolvedClone -Recurse -Force
    }
}
