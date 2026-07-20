#requires -Version 5.1
[CmdletBinding()]
param(
    [ValidatePattern('^TASK-\d{3}$')][string]$TaskId = 'TASK-001',
    [string]$ArtifactRoot,
    [string]$ResultPath
)

$ErrorActionPreference = 'Stop'
Import-Module (Join-Path $PSScriptRoot 'Task001.Common.psm1') -Force
$root = Get-Task001RepositoryRoot
if ($TaskId -ne 'TASK-001') { throw 'CIV001-EVIDENCE-001: This command packages TASK-001 only.' }
if (-not $ArtifactRoot) { $ArtifactRoot = Join-Path $root 'Artifacts' }
$ArtifactRoot = Assert-Task001PathWithinRoot -RepositoryRoot $root -TargetPath $ArtifactRoot
$started = Get-Date
$stamp = [datetime]::UtcNow.ToString('yyyyMMddTHHmmssZ')
$evidenceRoot = Join-Path $ArtifactRoot 'evidence'
$archive = Join-Path $evidenceRoot "$TaskId-$stamp.zip"
$packageManifest = Join-Path $evidenceRoot "$TaskId-$stamp.manifest.json"
if (-not $ResultPath) { $ResultPath = Join-Path $ArtifactRoot "results\package-evidence-$stamp.json" }
$ResultPath = Assert-Task001PathWithinRoot -RepositoryRoot $root -TargetPath $ResultPath
$diagnostics = @()
$exitCode = 1

try {
    $required = Get-Task001EvidenceRequirements -RepositoryRoot $root -ArtifactRoot $ArtifactRoot
    $missing = @($required | Where-Object { -not (Test-Path -LiteralPath $_ -PathType Leaf) })
    if ($missing.Count -gt 0) {
        throw "CIV001-EVIDENCE-002: Required evidence is absent: $($missing -join ', ')"
    }

    $resultFiles = @(Get-ChildItem -LiteralPath (Join-Path $ArtifactRoot 'results') -Filter '*.json' -File -ErrorAction SilentlyContinue)
    $failedResults = @($resultFiles | Where-Object {
        try { (Get-Content -LiteralPath $_.FullName -Raw | ConvertFrom-Json).status -ne 'PASS' } catch { $true }
    })
    if ($failedResults.Count -gt 0) {
        throw "CIV001-EVIDENCE-003: Evidence contains failed or unreadable command results: $($failedResults.Name -join ', ')"
    }

    New-Item -ItemType Directory -Path $evidenceRoot -Force | Out-Null
    $files = New-Object System.Collections.Generic.List[string]
    foreach ($path in $required) { $files.Add([System.IO.Path]::GetFullPath($path)) }
    foreach ($directory in @('logs', 'results', 'tests', 'negative')) {
        $dir = Join-Path $ArtifactRoot $directory
        if (Test-Path -LiteralPath $dir -PathType Container) {
            Get-ChildItem -LiteralPath $dir -File -Recurse | ForEach-Object { $files.Add($_.FullName) }
        }
    }
    $uniqueFiles = @($files | Select-Object -Unique)
    $records = @($uniqueFiles | ForEach-Object { New-Task001ArtifactRecord -Path $_ -RepositoryRoot $root })
    [pscustomobject][ordered]@{
        schemaVersion = 1
        taskId = $TaskId
        createdAtUtc = [datetime]::UtcNow.ToString('o')
        files = $records
    } | ConvertTo-Json -Depth 8 | Set-Content -LiteralPath $packageManifest -Encoding UTF8

    $staging = Join-Path $evidenceRoot ".staging-$stamp"
    New-Item -ItemType Directory -Path $staging -Force | Out-Null
    try {
        foreach ($file in $uniqueFiles + @($packageManifest)) {
            $full = [System.IO.Path]::GetFullPath($file)
            $relative = if ($full.StartsWith([System.IO.Path]::GetFullPath($root), [System.StringComparison]::OrdinalIgnoreCase)) {
                $full.Substring([System.IO.Path]::GetFullPath($root).TrimEnd('\', '/').Length).TrimStart('\', '/')
            } else { Join-Path 'external-artifacts' (Split-Path -Leaf $full) }
            $destination = Join-Path $staging $relative
            New-Item -ItemType Directory -Path (Split-Path -Parent $destination) -Force | Out-Null
            Copy-Item -LiteralPath $full -Destination $destination -Force
        }
        Compress-Archive -Path (Join-Path $staging '*') -DestinationPath $archive -CompressionLevel Optimal -Force
    } finally {
        if (Test-Path -LiteralPath $staging) { Remove-Item -LiteralPath $staging -Recurse -Force }
    }
    $diagnostics += [pscustomobject]@{ status = 'PASS'; code = 'CIV001-EVIDENCE-000'; message = 'Complete TASK-001 evidence archive created.'; details = @{ archive = $archive; files = $uniqueFiles.Count } }
    $exitCode = 0
} catch {
    $diagnostics += [pscustomobject]@{ status = 'FAIL'; code = 'CIV001-EVIDENCE-099'; message = $_.Exception.Message; details = $null }
}

$status = if ($exitCode -eq 0) { 'PASS' } else { 'FAIL' }
Write-Task001CommandResult -Path $ResultPath -Command "Build/Package-Evidence.ps1 -TaskId $TaskId" -Status $status -ExitCode $exitCode -StartedAt $started -Diagnostics $diagnostics -ArtifactPaths @($archive, $packageManifest) -RepositoryRoot $root | Out-Null
$diagnostics | ForEach-Object { Write-Host "[$($_.status)] $($_.code) $($_.message)" }
if ($exitCode -eq 0) { Write-Host "Archive: $archive" }
exit $exitCode
