#requires -Version 5.1
[CmdletBinding()]
param(
    [string]$UnityPath,
    [string]$ResultPath
)

$ErrorActionPreference = 'Stop'
Import-Module (Join-Path $PSScriptRoot 'Task001.Common.psm1') -Force
$root = Get-Task001RepositoryRoot
$started = Get-Date
$stamp = [datetime]::UtcNow.ToString('yyyyMMddTHHmmssZ')
if (-not $ResultPath) { $ResultPath = Join-Path $root "Artifacts\results\verify-world-viewer-$stamp.json" }
$ResultPath = Assert-Task001PathWithinRoot -RepositoryRoot $root -TargetPath $ResultPath
$artifactRoot = Assert-Task001PathWithinRoot -RepositoryRoot $root -TargetPath (Join-Path $root 'Artifacts')
$logRoot = Assert-Task001PathWithinRoot -RepositoryRoot $root -TargetPath (Join-Path $artifactRoot 'logs')
$testRoot = Assert-Task001PathWithinRoot -RepositoryRoot $root -TargetPath (Join-Path $artifactRoot 'tests')
$buildLog = Assert-Task001PathWithinRoot -RepositoryRoot $root -TargetPath (Join-Path $logRoot "world-viewer-build-$stamp.log")
$peopleLog = Assert-Task001PathWithinRoot -RepositoryRoot $root -TargetPath (Join-Path $logRoot "world-viewer-people-tests-$stamp.log")
$presentationLog = Assert-Task001PathWithinRoot -RepositoryRoot $root -TargetPath (Join-Path $logRoot "world-viewer-presentation-tests-$stamp.log")
$smokeLog = Assert-Task001PathWithinRoot -RepositoryRoot $root -TargetPath (Join-Path $logRoot "world-viewer-smoke-$stamp.log")
$peopleXml = Assert-Task001PathWithinRoot -RepositoryRoot $root -TargetPath (Join-Path $testRoot 'world-viewer-people.xml')
$presentationXml = Assert-Task001PathWithinRoot -RepositoryRoot $root -TargetPath (Join-Path $testRoot 'world-viewer-presentation.xml')
$player = Assert-Task001PathWithinRoot -RepositoryRoot $root -TargetPath (Join-Path $artifactRoot 'build\world-viewer\CivilizationSandboxWorldViewer.exe')
$diagnostics = @()
$artifacts = @($buildLog, $peopleLog, $presentationLog, $smokeLog, $peopleXml, $presentationXml, $player)
$exitCode = 1

New-Item -ItemType Directory -Path $logRoot -Force | Out-Null
New-Item -ItemType Directory -Path $testRoot -Force | Out-Null

try {
    $lock = Test-Task001PackageLock -RepositoryRoot $root
    if ($lock.status -ne 'PASS') { throw "$($lock.code): Package-lock verification failed." }
    $manifest = Test-Task001PackageManifest -RepositoryRoot $root
    if ($manifest.status -ne 'PASS') { throw "$($manifest.code): Package-manifest verification failed." }
    $unity = Resolve-Task001Unity -UnityPath $UnityPath -RepositoryRoot $root -RequiredModules @('windows-mono')

    function Invoke-UnityWorldViewer {
        param([string[]]$Arguments, [string]$FailureCode, [string]$FailureMessage)
        $process = Start-Process -FilePath $unity -ArgumentList $Arguments -PassThru -WindowStyle Hidden
        $process.WaitForExit()
        if ($process.ExitCode -ne 0) { throw "$FailureCode`: $FailureMessage (exit $($process.ExitCode))." }
    }

    Invoke-UnityWorldViewer -FailureCode 'CIV-BUILD01-TEST-001' -FailureMessage "People tests failed; see $peopleLog" -Arguments @(
        '-batchmode', '-nographics', '-projectPath', $root, '-runTests', '-testPlatform', 'EditMode',
        '-assemblyNames', 'CivSandbox.People.Tests', '-testResults', $peopleXml, '-logFile', $peopleLog)
    Invoke-UnityWorldViewer -FailureCode 'CIV-BUILD01-TEST-002' -FailureMessage "Presentation tests failed; see $presentationLog" -Arguments @(
        '-batchmode', '-nographics', '-projectPath', $root, '-runTests', '-testPlatform', 'EditMode',
        '-assemblyNames', 'CivSandbox.WorldViewer.Editor.Tests', '-testResults', $presentationXml, '-logFile', $presentationLog)

    $minimumPassingTests = @{}
    $minimumPassingTests[$peopleXml] = 7
    $minimumPassingTests[$presentationXml] = 29
    foreach ($reportPath in @($peopleXml, $presentationXml)) {
        if (-not (Test-Path -LiteralPath $reportPath -PathType Leaf)) { throw "CIV-BUILD01-TEST-003: Missing NUnit report $reportPath." }
        [xml]$report = Get-Content -LiteralPath $reportPath -Raw
        if (-not $report.'test-run' -or
            [int]$report.'test-run'.failed -ne 0 -or
            [int]$report.'test-run'.passed -lt $minimumPassingTests[$reportPath]) {
            throw "CIV-BUILD01-TEST-004: Test failures or fewer than $($minimumPassingTests[$reportPath]) passing tests were reported in $reportPath."
        }
    }

    Invoke-UnityWorldViewer -FailureCode 'CIV-BUILD01-BUILD-001' -FailureMessage "Player build failed; see $buildLog" -Arguments @(
        '-batchmode', '-nographics', '-quit', '-projectPath', $root,
        '-executeMethod', 'CivSandbox.WorldViewer.Editor.WorldViewerBuild.Run', '-logFile', $buildLog)
    if (-not (Test-Path -LiteralPath $player -PathType Leaf)) { throw 'CIV-BUILD01-BUILD-002: Expected World Viewer player is absent.' }

    $smoke = Start-Process -FilePath $player -ArgumentList @('-batchmode', '-nographics', '-logFile', $smokeLog) -PassThru -WindowStyle Hidden
    if (-not $smoke.WaitForExit(30000)) {
        $smoke.Kill()
        throw 'CIV-BUILD01-SMOKE-001: World Viewer player did not exit within 30 seconds.'
    }
    if ($smoke.ExitCode -ne 0) { throw "CIV-BUILD01-SMOKE-002: World Viewer player exited with $($smoke.ExitCode)." }
    if (-not (Select-String -LiteralPath $smokeLog -SimpleMatch 'CIV-BUILD01-SMOKE-000' -Quiet)) {
        throw 'CIV-BUILD01-SMOKE-003: First-frame success marker is absent from the player log.'
    }
    if (-not (Select-String -LiteralPath $smokeLog -SimpleMatch 'CIV-BUILD02-WORLDGEN-SMOKE-000' -Quiet)) {
        throw 'CIV-BUILD02-SMOKE-003: Generated-world and founding-region success marker is absent from the player log.'
    }

    [xml]$peopleReport = Get-Content -LiteralPath $peopleXml -Raw
    [xml]$presentationReport = Get-Content -LiteralPath $presentationXml -Raw
    $diagnostics += [pscustomobject]@{
        status = 'PASS'
        code = 'CIV-BUILD01-VERIFY-000'
        message = 'World Viewer tests, player build, and first-frame smoke passed.'
        details = @{
            peopleTests = [int]$peopleReport.'test-run'.passed
            presentationTests = [int]$presentationReport.'test-run'.passed
            player = $player
        }
    }
    $exitCode = 0
} catch {
    $diagnostics += [pscustomobject]@{ status = 'FAIL'; code = 'CIV-BUILD01-VERIFY-099'; message = $_.Exception.Message; details = @{} }
}

$status = if ($exitCode -eq 0) { 'PASS' } else { 'FAIL' }
Write-Task001CommandResult -Path $ResultPath -Command 'Build/Verify-WorldViewer.ps1' -Status $status -ExitCode $exitCode -StartedAt $started -Diagnostics $diagnostics -ArtifactPaths $artifacts -RepositoryRoot $root | Out-Null
$diagnostics | ForEach-Object { Write-Host "[$($_.status)] $($_.code) $($_.message)" }
Write-Host "Result: $ResultPath"
exit $exitCode
