#requires -Version 5.1
[CmdletBinding()]
param(
    [switch]$InstallPrerequisites,
    [switch]$RepositoryOnly,
    [string]$UnityPath,
    [string]$ResultPath,
    [ValidateRange(60, 3600)][int]$InstallTimeoutSeconds = 900
)

$ErrorActionPreference = 'Stop'
Import-Module (Join-Path $PSScriptRoot 'Task001.Common.psm1') -Force
$root = Get-Task001RepositoryRoot
$started = Get-Date
if (-not $ResultPath) {
    $stamp = [datetime]::UtcNow.ToString('yyyyMMddTHHmmssZ')
    $ResultPath = Join-Path $root "Artifacts\results\bootstrap-$stamp.json"
}
$ResultPath = Assert-Task001PathWithinRoot -RepositoryRoot $root -TargetPath $ResultPath
$diagnostics = New-Object System.Collections.Generic.List[object]
$failed = $false

function Add-Diagnostic([string]$Status, [string]$Code, [string]$Message, [object]$Details) {
    $script:diagnostics.Add([pscustomobject][ordered]@{ status = $Status; code = $Code; message = $Message; details = $Details })
    if ($Status -eq 'FAIL') { $script:failed = $true }
}

try {
    $contract = Get-Task001Toolchain -RepositoryRoot $root
    if (-not $RepositoryOnly) {
        $winget = Get-Command winget -CommandType Application -ErrorAction SilentlyContinue | Select-Object -First 1
        if (-not $winget) {
            Add-Diagnostic 'FAIL' 'CIV001-WINGET-001' 'WinGet is required before pinned prerequisites can be audited or installed.' $null
        }
        foreach ($tool in $contract.tools) {
            $check = Test-Task001Tool -Tool $tool
            if ($check.status -eq 'FAIL' -and $InstallPrerequisites -and -not $check.path) {
                if (-not $winget) { continue }
                $wingetVersion = if ($tool.PSObject.Properties.Name -contains 'wingetVersion') { [string]$tool.wingetVersion } else { [string]$tool.version }
                $show = (& $winget.Source show --id $tool.wingetId --version $wingetVersion --exact --accept-source-agreements --disable-interactivity 2>&1 | Out-String)
                if ($LASTEXITCODE -ne 0 -or $show -notmatch [regex]::Escape([string]$tool.installerSha256)) {
                    Add-Diagnostic 'FAIL' 'CIV001-INSTALL-002' "WinGet metadata did not expose the pinned installer SHA-256 for $($tool.name); installation was refused." @{ expectedSha256 = $tool.installerSha256; wingetVersion = $wingetVersion }
                    continue
                }
                $installArguments = "install --id $($tool.wingetId) --version $wingetVersion --exact --silent --accept-package-agreements --accept-source-agreements --disable-interactivity"
                $installProcess = Start-Process -FilePath $winget.Source -ArgumentList $installArguments -PassThru -WindowStyle Hidden
                if (-not $installProcess.WaitForExit($InstallTimeoutSeconds * 1000)) {
                    $installProcess.Kill()
                    Add-Diagnostic 'FAIL' 'CIV001-INSTALL-003' "Timed out installing $($tool.name); the bootstrap-owned WinGet process was stopped." @{ timeoutSeconds = $InstallTimeoutSeconds; wingetId = $tool.wingetId }
                    continue
                }
                if ($installProcess.ExitCode -ne 0) {
                    Add-Diagnostic 'FAIL' 'CIV001-INSTALL-001' "WinGet failed to install $($tool.name)." @{ exitCode = $installProcess.ExitCode; wingetId = $tool.wingetId }
                    continue
                }
                $check = Test-Task001Tool -Tool $tool
            }
            Add-Diagnostic $check.status $check.code $check.message $check
        }
        try {
            try {
                $resolvedUnity = Resolve-Task001Unity -UnityPath $UnityPath -RepositoryRoot $root
            } catch {
                if (-not $InstallPrerequisites -or $UnityPath) { throw }
                $hubTool = $contract.tools | Where-Object { $_.wingetId -eq 'UnityTechnologies.UnityHub' }
                $hub = Resolve-Task001ToolExecutable -Tool $hubTool
                if (-not $hub) { throw 'CIV001-UNITY-005: Pinned Unity Hub is required before editor installation.' }
                $defaultUnity = Join-Path $env:ProgramFiles "Unity\Hub\Editor\$($contract.unity.version)\Editor\Unity.exe"
                if (-not (Test-Path -LiteralPath $defaultUnity -PathType Leaf)) {
                    $hubArguments = "-- --headless install --version $($contract.unity.version) --changeset $($contract.unity.changeset) -m windows-mono linux-mono"
                } elseif ($_.Exception.Message -match 'CIV001-UNITY-004') {
                    $hubArguments = "-- --headless install-modules --version $($contract.unity.version) -m windows-mono linux-mono"
                } else {
                    throw
                }
                $hubProcess = Start-Process -FilePath $hub -ArgumentList $hubArguments -PassThru -WindowStyle Hidden
                if (-not $hubProcess.WaitForExit($InstallTimeoutSeconds * 1000)) {
                    $hubProcess.Kill()
                    throw "CIV001-UNITY-007: Unity Hub CLI timed out after $InstallTimeoutSeconds seconds."
                }
                if ($hubProcess.ExitCode -ne 0) { throw "CIV001-UNITY-006: Unity Hub CLI returned $($hubProcess.ExitCode)." }
                $resolvedUnity = Resolve-Task001Unity -RepositoryRoot $root
            }
            Add-Diagnostic 'PASS' 'CIV001-UNITY-000' 'Pinned Unity editor, executable hash, and build modules verified.' @{ path = $resolvedUnity }
        } catch {
            Add-Diagnostic 'FAIL' 'CIV001-UNITY-099' $_.Exception.Message $null
        }
    }

    $projectVersionPath = Join-Path $root 'ProjectSettings\ProjectVersion.txt'
    $projectVersion = if (Test-Path -LiteralPath $projectVersionPath) { Get-Content -LiteralPath $projectVersionPath -Raw } else { '' }
    $expectedRevision = "$($contract.unity.version) ($($contract.unity.changeset))"
    if ($projectVersion -notmatch [regex]::Escape($expectedRevision)) {
        Add-Diagnostic 'FAIL' 'CIV001-PROJECT-001' 'ProjectVersion.txt does not match the pinned Unity version and changeset.' @{ expected = $expectedRevision }
    } else {
        Add-Diagnostic 'PASS' 'CIV001-PROJECT-000' 'Pinned project editor revision verified.' @{ expected = $expectedRevision }
    }

    $lock = Test-Task001PackageLock -RepositoryRoot $root
    if ($lock.status -eq 'PASS') {
        Add-Diagnostic 'PASS' $lock.code 'Committed package-lock hash verified.' $lock
    } else {
        Add-Diagnostic 'FAIL' $lock.code 'Committed package-lock hash is missing, unresolved, or tampered.' $lock
    }

    $manifest = Test-Task001PackageManifest -RepositoryRoot $root
    if ($manifest.status -eq 'PASS') {
        Add-Diagnostic 'PASS' $manifest.code 'Direct package manifest matches the pinned toolchain contract.' $manifest
    } else {
        Add-Diagnostic 'FAIL' $manifest.code 'Direct package manifest is missing, unreadable, or drifted from the pinned contract.' $manifest
    }
} catch {
    Add-Diagnostic 'FAIL' 'CIV001-BOOTSTRAP-099' $_.Exception.Message $null
}

$exitCode = if ($failed) { 1 } else { 0 }
$status = if ($failed) { 'FAIL' } else { 'PASS' }
Write-Task001CommandResult -Path $ResultPath -Command 'Build/Bootstrap.ps1' -Status $status -ExitCode $exitCode -StartedAt $started -Diagnostics $diagnostics -RepositoryRoot $root | Out-Null
$diagnostics | ForEach-Object { Write-Host "[$($_.status)] $($_.code) $($_.message)" }
Write-Host "Result: $ResultPath"
exit $exitCode
