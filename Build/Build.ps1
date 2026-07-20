#requires -Version 5.1
[CmdletBinding()]
param(
    [Parameter(Mandatory = $true)][ValidateSet('Windows', 'Linux')][string]$Target,
    [string]$UnityPath,
    [string]$ResultPath
)

$ErrorActionPreference = 'Stop'
Import-Module (Join-Path $PSScriptRoot 'Task001.Common.psm1') -Force
$root = Get-Task001RepositoryRoot
$started = Get-Date
$stamp = [datetime]::UtcNow.ToString('yyyyMMddTHHmmssZ')
$log = Join-Path $root "Artifacts\logs\build-$($Target.ToLowerInvariant())-$stamp.log"
if (-not $ResultPath) { $ResultPath = Join-Path $root "Artifacts\results\build-$($Target.ToLowerInvariant())-$stamp.json" }
$ResultPath = Assert-Task001PathWithinRoot -RepositoryRoot $root -TargetPath $ResultPath
$diagnostics = @()
$exitCode = 1
$artifact = if ($Target -eq 'Windows') {
    Join-Path $root 'Artifacts\build\windows\CivilizationSandboxBootstrap.exe'
} else {
    Join-Path $root 'Artifacts\build\linux\CivilizationSandboxBootstrap.x86_64'
}
$targetDirectory = Split-Path -Parent $artifact
$targetDirectory = Assert-Task001PathWithinRoot -RepositoryRoot $root -TargetPath $targetDirectory
New-Item -ItemType Directory -Path (Split-Path -Parent $log) -Force | Out-Null
Set-Content -LiteralPath $log -Value "CIV001-BUILD: launching $Target build" -Encoding UTF8

try {
    $lock = Test-Task001PackageLock -RepositoryRoot $root
    if ($lock.status -ne 'PASS') { throw "$($lock.code): Package-lock verification failed." }
    $requiredModule = if ($Target -eq 'Windows') { 'windows-mono' } else { 'linux-mono' }
    $unity = Resolve-Task001Unity -UnityPath $UnityPath -RepositoryRoot $root -RequiredModules @($requiredModule)
    if (Test-Path -LiteralPath $targetDirectory) {
        Assert-Task001TreeHasNoReparsePoints -Path $targetDirectory
        Remove-Item -LiteralPath $targetDirectory -Recurse -Force
    }
    $arguments = "-batchmode -nographics -quit -projectPath `"$root`" -executeMethod CivSandbox.Tooling.Editor.BaselineBuild.Run -civSandboxBuildTarget $Target -logFile `"$log`""
    $process = Start-Process -FilePath $unity -ArgumentList $arguments -PassThru -WindowStyle Hidden
    $process.WaitForExit()
    $exitCode = $process.ExitCode
    if ($exitCode -eq 0 -and -not (Test-Path -LiteralPath $artifact -PathType Leaf)) {
        $exitCode = 1
        throw 'CIV001-BUILD-002: Unity returned zero but the expected player executable is absent.'
    }
    if ($exitCode -ne 0) { throw "CIV001-BUILD-001: Unity build failed with exit code $exitCode. See $log" }
    $diagnostics += [pscustomobject]@{ status = 'PASS'; code = 'CIV001-BUILD-000'; message = "$Target player built."; details = @{ log = $log } }
} catch {
    $diagnostics += [pscustomobject]@{ status = 'FAIL'; code = 'CIV001-BUILD-099'; message = $_.Exception.Message; details = @{ log = $log } }
    if ($exitCode -eq 0) { $exitCode = 1 }
}

$status = if ($exitCode -eq 0) { 'PASS' } else { 'FAIL' }
Write-Task001CommandResult -Path $ResultPath -Command "Build/Build.ps1 -Target $Target" -Status $status -ExitCode $exitCode -StartedAt $started -Diagnostics $diagnostics -ArtifactPaths @($artifact, $log) -RepositoryRoot $root | Out-Null
$diagnostics | ForEach-Object { Write-Host "[$($_.status)] $($_.code) $($_.message)" }
Write-Host "Log: $log"
Write-Host "Result: $ResultPath"
exit $exitCode
