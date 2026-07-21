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
$evidenceRoot = Assert-Task001PathWithinRoot -RepositoryRoot $root -TargetPath $evidenceRoot
$archive = Assert-Task001PathWithinRoot -RepositoryRoot $root -TargetPath $archive
$packageManifest = Assert-Task001PathWithinRoot -RepositoryRoot $root -TargetPath $packageManifest
if (-not $ResultPath) { $ResultPath = Join-Path $ArtifactRoot "results\package-evidence-$stamp.json" }
$ResultPath = Assert-Task001PathWithinRoot -RepositoryRoot $root -TargetPath $ResultPath
$diagnostics = @()
$exitCode = 1

try {
    if (Test-Path -LiteralPath $ArtifactRoot -PathType Container) { Assert-Task001TreeHasNoReparsePoints -Path $ArtifactRoot }
    $required = Get-Task001EvidenceRequirements -RepositoryRoot $root -ArtifactRoot $ArtifactRoot
    $missing = @($required | Where-Object { -not (Test-Path -LiteralPath $_ -PathType Leaf) })
    if ($missing.Count -gt 0) {
        throw "CIV001-EVIDENCE-002: Required evidence is absent: $($missing -join ', ')"
    }

    $resultsDirectory = Assert-Task001PathWithinRoot -RepositoryRoot $root -TargetPath (Join-Path $ArtifactRoot 'results')
    $canonicalCommands = @{
        'bootstrap-run-1.json' = 'Build/Bootstrap.ps1'
        'bootstrap-run-2.json' = 'Build/Bootstrap.ps1'
        'build-windows.json' = 'Build/Build.ps1 -Target Windows'
        'build-linux.json' = 'Build/Build.ps1 -Target Linux'
        'test-bootstrap.json' = 'Build/Test.ps1 -Suite Bootstrap'
        'task001-script-self-tests.json' = 'Tests/Bootstrap/Task001.Bootstrap.Tests.ps1'
    }
    foreach ($name in $canonicalCommands.Keys) {
        $path = Join-Path $resultsDirectory $name
        $artifactDirectory = if ($name -eq 'build-windows.json') { Join-Path $ArtifactRoot 'build\windows' } elseif ($name -eq 'build-linux.json') { Join-Path $ArtifactRoot 'build\linux' } else { $null }
        Assert-Task001CommandResultIntegrity -Path $path -ExpectedCommand $canonicalCommands[$name] -ArtifactDirectory $artifactDirectory -RepositoryRoot $root | Out-Null
    }
    $currentResultFull = [System.IO.Path]::GetFullPath($ResultPath)
    $resultFiles = @(Get-ChildItem -LiteralPath $resultsDirectory -Filter '*.json' -File -ErrorAction SilentlyContinue | Where-Object { $_.FullName -ne $currentResultFull })
    foreach ($resultFile in $resultFiles) {
        $expected = if ($canonicalCommands.ContainsKey($resultFile.Name)) { [string]$canonicalCommands[$resultFile.Name] } else { $null }
        Assert-Task001CommandResultIntegrity -Path $resultFile.FullName -ExpectedCommand $expected -RepositoryRoot $root | Out-Null
    }

    New-Item -ItemType Directory -Path $evidenceRoot -Force | Out-Null
    $files = New-Object System.Collections.Generic.List[string]
    foreach ($path in $required) { $files.Add([System.IO.Path]::GetFullPath($path)) }
    foreach ($directory in @('logs', 'results', 'tests', 'negative')) {
        $dir = Assert-Task001PathWithinRoot -RepositoryRoot $root -TargetPath (Join-Path $ArtifactRoot $directory)
        if (Test-Path -LiteralPath $dir -PathType Container) {
            Assert-Task001TreeHasNoReparsePoints -Path $dir
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

    $staging = Assert-Task001PathWithinRoot -RepositoryRoot $root -TargetPath (Join-Path $evidenceRoot ".staging-$stamp")
    New-Item -ItemType Directory -Path $staging -Force | Out-Null
    try {
        foreach ($file in $uniqueFiles + @($packageManifest)) {
            $full = [System.IO.Path]::GetFullPath($file)
            $relative = if ($full.StartsWith([System.IO.Path]::GetFullPath($root), [System.StringComparison]::OrdinalIgnoreCase)) {
                $full.Substring([System.IO.Path]::GetFullPath($root).TrimEnd('\', '/').Length).TrimStart('\', '/')
            } else { Join-Path 'external-artifacts' (Split-Path -Leaf $full) }
            $destination = Assert-Task001PathWithinRoot -RepositoryRoot $root -TargetPath (Join-Path $staging $relative)
            New-Item -ItemType Directory -Path (Split-Path -Parent $destination) -Force | Out-Null
            Copy-Item -LiteralPath $full -Destination $destination -Force
        }
        Compress-Archive -Path (Join-Path $staging '*') -DestinationPath $archive -CompressionLevel Optimal -Force
    } finally {
        if (Test-Path -LiteralPath $staging) {
            Assert-Task001TreeHasNoReparsePoints -Path $staging
            Remove-Item -LiteralPath $staging -Recurse -Force
        }
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
