#requires -Version 5.1
[CmdletBinding()]
param(
    [ValidateSet('Bootstrap')][string]$Suite = 'Bootstrap',
    [string]$UnityPath,
    [switch]$SkipPlayerSmoke,
    [string]$ResultPath
)

$ErrorActionPreference = 'Stop'
Import-Module (Join-Path $PSScriptRoot 'Task001.Common.psm1') -Force
$root = Get-Task001RepositoryRoot
$started = Get-Date
$stamp = [datetime]::UtcNow.ToString('yyyyMMddTHHmmssZ')
$log = Join-Path $root "Artifacts\logs\test-bootstrap-$stamp.log"
$testXml = Join-Path $root 'Artifacts\tests\bootstrap-editmode.xml'
$smokeLog = Join-Path $root "Artifacts\logs\windows-smoke-$stamp.log"
$log = Assert-Task001PathWithinRoot -RepositoryRoot $root -TargetPath $log
$testXml = Assert-Task001PathWithinRoot -RepositoryRoot $root -TargetPath $testXml
$smokeLog = Assert-Task001PathWithinRoot -RepositoryRoot $root -TargetPath $smokeLog
if (-not $ResultPath) { $ResultPath = Join-Path $root "Artifacts\results\test-bootstrap-$stamp.json" }
$ResultPath = Assert-Task001PathWithinRoot -RepositoryRoot $root -TargetPath $ResultPath
$diagnostics = @()
$exitCode = 1
$logDirectory = Assert-Task001PathWithinRoot -RepositoryRoot $root -TargetPath (Split-Path -Parent $log)
$testDirectory = Assert-Task001PathWithinRoot -RepositoryRoot $root -TargetPath (Split-Path -Parent $testXml)
New-Item -ItemType Directory -Path $logDirectory -Force | Out-Null
Set-Content -LiteralPath $log -Value 'CIV001-TEST: launching Bootstrap EditMode test' -Encoding UTF8

try {
    $lock = Test-Task001PackageLock -RepositoryRoot $root
    if ($lock.status -ne 'PASS') { throw "$($lock.code): Package-lock verification failed." }
    $unity = Resolve-Task001Unity -UnityPath $UnityPath -RepositoryRoot $root -RequiredModules @()
    New-Item -ItemType Directory -Path $testDirectory -Force | Out-Null
    if (Test-Path -LiteralPath $testXml) { Remove-Item -LiteralPath $testXml -Force }
    $arguments = "-batchmode -nographics -projectPath `"$root`" -runTests -testPlatform EditMode -assemblyNames CivSandbox.Tooling.Editor.Tests -testResults `"$testXml`" -logFile `"$log`""
    $process = Start-Process -FilePath $unity -ArgumentList $arguments -PassThru -WindowStyle Hidden
    $process.WaitForExit()
    $exitCode = $process.ExitCode
    if ($exitCode -ne 0) { throw "CIV001-TEST-001: Unity test runner failed with exit code $exitCode. See $log" }
    if (-not (Test-Path -LiteralPath $testXml -PathType Leaf)) { throw 'CIV001-TEST-002: NUnit XML was not produced.' }
    [xml]$xml = Get-Content -LiteralPath $testXml -Raw
    $testRun = $xml.'test-run'
    if (-not $testRun -or [int]$testRun.failed -ne 0 -or [int]$testRun.passed -ne 1) {
        throw "CIV001-TEST-003: Expected exactly one passing EditMode test; observed passed=$($testRun.passed), failed=$($testRun.failed)."
    }
    $diagnostics += [pscustomobject]@{ status = 'PASS'; code = 'CIV001-TEST-000'; message = 'Exactly one deterministic EditMode test passed.'; details = @{ report = $testXml } }

    if (-not $SkipPlayerSmoke) {
        $player = Join-Path $root 'Artifacts\build\windows\CivilizationSandboxBootstrap.exe'
        $player = Assert-Task001PathWithinRoot -RepositoryRoot $root -TargetPath $player
        $playerDirectory = Assert-Task001PathWithinRoot -RepositoryRoot $root -TargetPath (Split-Path -Parent $player)
        if (-not (Test-Path -LiteralPath $player -PathType Leaf)) { throw 'CIV001-SMOKE-001: Windows player is required for the headless smoke test.' }
        $buildResultPath = Assert-Task001PathWithinRoot -RepositoryRoot $root -TargetPath (Join-Path $root 'Artifacts\results\build-windows.json')
        $buildResult = Assert-Task001CommandResultIntegrity -Path $buildResultPath -ExpectedCommand 'Build/Build.ps1 -Target Windows' -ArtifactDirectory $playerDirectory -RepositoryRoot $root
        $process = Start-Process -FilePath $player -ArgumentList @('-batchmode', '-nographics', '-logFile', $smokeLog) -PassThru -WindowStyle Hidden
        if (-not $process.WaitForExit(30000)) {
            $process.Kill()
            throw 'CIV001-SMOKE-002: Windows player did not exit within 30 seconds.'
        }
        if ($process.ExitCode -ne 0) { throw "CIV001-SMOKE-003: Windows player exited with $($process.ExitCode)." }
        $diagnostics += [pscustomobject]@{ status = 'PASS'; code = 'CIV001-SMOKE-000'; message = 'Headless Windows player with candidate-build provenance exited successfully.'; details = @{ log = $smokeLog; buildResult = $buildResultPath; buildCommit = $buildResult.git.commit } }
    }
    $exitCode = 0
} catch {
    $diagnostics += [pscustomobject]@{ status = 'FAIL'; code = 'CIV001-TEST-099'; message = $_.Exception.Message; details = @{ log = $log } }
    if ($exitCode -eq 0) { $exitCode = 1 }
}

$status = if ($exitCode -eq 0) { 'PASS' } else { 'FAIL' }
Write-Task001CommandResult -Path $ResultPath -Command "Build/Test.ps1 -Suite $Suite" -Status $status -ExitCode $exitCode -StartedAt $started -Diagnostics $diagnostics -ArtifactPaths @($testXml, $log, $smokeLog) -RepositoryRoot $root | Out-Null
$diagnostics | ForEach-Object { Write-Host "[$($_.status)] $($_.code) $($_.message)" }
Write-Host "Result: $ResultPath"
exit $exitCode
