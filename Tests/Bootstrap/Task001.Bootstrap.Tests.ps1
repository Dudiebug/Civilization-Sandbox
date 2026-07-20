#requires -Version 5.1
[CmdletBinding()]
param([string]$ResultPath)

$ErrorActionPreference = 'Stop'
$root = [System.IO.Path]::GetFullPath((Join-Path $PSScriptRoot '..\..'))
Import-Module (Join-Path $root 'Build\Task001.Common.psm1') -Force
$started = Get-Date
if (-not $ResultPath) { $ResultPath = Join-Path $root 'Artifacts\results\task001-script-self-tests.json' }
$ResultPath = Assert-Task001PathWithinRoot -RepositoryRoot $root -TargetPath $ResultPath
$passed = 0
$passedNames = New-Object System.Collections.Generic.List[string]
$failures = New-Object System.Collections.Generic.List[string]

function Test-Case([string]$Name, [scriptblock]$Body) {
    try {
        & $Body
        $script:passed++
        $script:passedNames.Add($Name)
        Write-Host "PASS $Name"
    } catch {
        $script:failures.Add("$Name`: $($_.Exception.Message)")
        Write-Host "FAIL $Name`: $($_.Exception.Message)"
    }
}

$temp = Join-Path ([System.IO.Path]::GetTempPath()) ("civsandbox-task001-tests-" + [guid]::NewGuid().ToString('N'))
New-Item -ItemType Directory -Path $temp -Force | Out-Null
$selfTestArtifacts = Join-Path $root 'Artifacts\task001-self-test'
New-Item -ItemType Directory -Path $selfTestArtifacts -Force | Out-Null
try {
    Test-Case 'outside-root guard' {
        $threw = $false
        try { Assert-Task001PathWithinRoot -RepositoryRoot $root -TargetPath $temp | Out-Null } catch { $threw = $_.Exception.Message -match 'CIV001-CLEAN-001' }
        if (-not $threw) { throw 'Outside path was accepted.' }
    }

    Test-Case 'repository-root guard' {
        $threw = $false
        try { Assert-Task001PathWithinRoot -RepositoryRoot $root -TargetPath $root | Out-Null } catch { $threw = $_.Exception.Message -match 'CIV001-CLEAN-001' }
        if (-not $threw) { throw 'Repository root was accepted as a cleanup child.' }
    }

    Test-Case 'nested reparse-point guard' {
        $tree = Join-Path $selfTestArtifacts 'reparse-tree'
        $junction = Join-Path $tree 'outside-junction'
        New-Item -ItemType Directory -Path $tree -Force | Out-Null
        New-Item -ItemType Junction -Path $junction -Target $temp | Out-Null
        try {
            $threw = $false
            try { Assert-Task001TreeHasNoReparsePoints -Path $tree } catch { $threw = $_.Exception.Message -match 'CIV001-PATH-003' }
            if (-not $threw) { throw 'Nested junction was accepted for recursive mutation.' }
        } finally {
            if (Test-Path -LiteralPath $junction) { Remove-Item -LiteralPath $junction -Force }
        }
    }

    Test-Case 'direct reparse-point guard' {
        $junction = Join-Path $selfTestArtifacts 'direct-outside-junction'
        New-Item -ItemType Junction -Path $junction -Target $temp | Out-Null
        try {
            $threw = $false
            try { Assert-Task001PathWithinRoot -RepositoryRoot $root -TargetPath $junction | Out-Null } catch { $threw = $_.Exception.Message -match 'CIV001-PATH-002' }
            if (-not $threw) { throw 'Direct junction was accepted for repository mutation.' }
        } finally {
            if (Test-Path -LiteralPath $junction) { Remove-Item -LiteralPath $junction -Force }
        }
    }

    Test-Case 'pinned lock hash' {
        $lock = Test-Task001PackageLock -RepositoryRoot $root
        if ($lock.status -ne 'PASS') { throw "Expected PASS, got $($lock.code)." }
    }

    Test-Case 'copied lock corruption' {
        $copy = Join-Path $temp 'packages-lock.json'
        Copy-Item -LiteralPath (Join-Path $root 'Packages\packages-lock.json') -Destination $copy
        Add-Content -LiteralPath $copy -Value 'tamper'
        $lock = Test-Task001PackageLock -RepositoryRoot $root -LockPath $copy
        if ($lock.status -ne 'FAIL' -or $lock.code -ne 'CIV001-LOCK-003') { throw 'Tampered lock did not fail with CIV001-LOCK-003.' }
    }

    Test-Case 'invalid Unity path' {
        $threw = $false
        try { Resolve-Task001Unity -RepositoryRoot $root -UnityPath (Join-Path $temp 'missing-unity.exe') | Out-Null } catch { $threw = $_.Exception.Message -match 'CIV001-UNITY-001' }
        if (-not $threw) { throw 'Invalid Unity path did not fail stably.' }
    }

    Test-Case 'missing prerequisite diagnostic' {
        $fake = [pscustomobject]@{ name = 'Missing Tool'; command = 'definitely-not-a-civsandbox-command'; wingetId = 'Example.Missing'; version = '1.0.0' }
        $check = Test-Task001Tool -Tool $fake
        if ($check.status -ne 'FAIL' -or $check.code -ne 'CIV001-TOOL-001') { throw 'Missing tool diagnostic was unstable.' }
    }

    Test-Case 'versioned result schema' {
        $resultPath = Join-Path $temp 'result.json'
        Write-Task001CommandResult -Path $resultPath -Command 'self-test' -Status PASS -ExitCode 0 -StartedAt (Get-Date) -RepositoryRoot $root | Out-Null
        $result = Get-Content -LiteralPath $resultPath -Raw | ConvertFrom-Json
        if ($result.schemaVersion -ne 1 -or $result.status -ne 'PASS' -or $result.exitCode -ne 0 -or -not $result.finishedAtUtc) { throw 'Result schema fields are incomplete.' }
    }

    Test-Case 'cleanup defaults to dry-run' {
        $sentinel = Join-Path $root 'Artifacts\task001-clean-dry-run\sentinel.txt'
        New-Item -ItemType Directory -Path (Split-Path -Parent $sentinel) -Force | Out-Null
        Set-Content -LiteralPath $sentinel -Value 'keep' -Encoding UTF8
        & powershell.exe -NoProfile -ExecutionPolicy Bypass -File (Join-Path $root 'Build\Clean.ps1') -Scope Outputs | Out-Null
        if ($LASTEXITCODE -ne 0 -or -not (Test-Path -LiteralPath $sentinel)) { throw 'Dry-run removed an output or failed.' }
        Remove-Item -LiteralPath (Split-Path -Parent $sentinel) -Recurse -Force
    }

    Test-Case 'cleanup refuses substituted root' {
        $stderr = Join-Path $temp 'clean-stderr.txt'
        $arguments = "-NoProfile -ExecutionPolicy Bypass -File `"$(Join-Path $root 'Build\Clean.ps1')`" -Scope Outputs -RepositoryRoot `"$temp`""
        $process = Start-Process -FilePath powershell.exe -ArgumentList $arguments -Wait -PassThru -WindowStyle Hidden -RedirectStandardError $stderr
        if ($process.ExitCode -eq 0) { throw 'Substituted cleanup root returned zero.' }
        if ((Get-Content -LiteralPath $stderr -Raw) -notmatch 'CIV001-CLEAN-002') { throw 'Substituted root did not produce the stable diagnostic.' }
    }

    Test-Case 'evidence omissions fail' {
        $missingArtifacts = Join-Path $selfTestArtifacts 'missing-artifacts'
        $result = Join-Path $selfTestArtifacts 'missing-evidence-result.json'
        & powershell.exe -NoProfile -ExecutionPolicy Bypass -File (Join-Path $root 'Build\Package-Evidence.ps1') -TaskId TASK-001 -ArtifactRoot $missingArtifacts -ResultPath $result | Out-Null
        if ($LASTEXITCODE -eq 0) { throw 'Missing evidence returned zero.' }
        $record = Get-Content -LiteralPath $result -Raw | ConvertFrom-Json
        if ($record.status -ne 'FAIL') { throw 'Missing evidence was not recorded as FAIL.' }
    }

    Test-Case 'build failure retains log and nonzero exit' {
        $before = @(Get-ChildItem -LiteralPath (Join-Path $root 'Artifacts\logs') -Filter 'build-windows-*.log' -File -ErrorAction SilentlyContinue).Count
        $result = Join-Path $selfTestArtifacts 'invalid-unity-build-result.json'
        & powershell.exe -NoProfile -ExecutionPolicy Bypass -File (Join-Path $root 'Build\Build.ps1') -Target Windows -UnityPath (Join-Path $temp 'invalid-unity.exe') -ResultPath $result | Out-Null
        if ($LASTEXITCODE -eq 0) { throw 'Invalid Unity build returned zero.' }
        $after = @(Get-ChildItem -LiteralPath (Join-Path $root 'Artifacts\logs') -Filter 'build-windows-*.log' -File -ErrorAction SilentlyContinue).Count
        if ($after -le $before) { throw 'Failure log was not retained.' }
    }

    Test-Case 'Unity wrappers wait only for the editor process' {
        foreach ($scriptName in @('Build.ps1', 'Test.ps1')) {
            $source = Get-Content -LiteralPath (Join-Path $root "Build\$scriptName") -Raw
            if ($source -match 'Start-Process[^\r\n]+-Wait') {
                throw "$scriptName uses Start-Process -Wait, which can hang on Unity compiler-server descendants."
            }
            if ($source -notmatch '\$process\.WaitForExit\(\)') {
                throw "$scriptName does not wait for the direct Unity process handle."
            }
        }
    }
} finally {
    if (Test-Path -LiteralPath $temp) { Remove-Item -LiteralPath $temp -Recurse -Force }
    if (Test-Path -LiteralPath $selfTestArtifacts) { Remove-Item -LiteralPath $selfTestArtifacts -Recurse -Force }
}

if ($failures.Count -gt 0) {
    Write-Task001CommandResult -Path $ResultPath -Command 'Tests/Bootstrap/Task001.Bootstrap.Tests.ps1' -Status FAIL -ExitCode 1 -StartedAt $started -Diagnostics @(
        [pscustomobject]@{ status = 'FAIL'; code = 'CIV001-SELFTEST-001'; message = 'One or more command self-tests failed.'; details = @{ passed = @($passedNames); failures = @($failures) } }
    ) -RepositoryRoot $root | Out-Null
    Write-Host "`nSELF-TEST FAILED ($passed passed, $($failures.Count) failed)"
    $failures | ForEach-Object { Write-Host "- $_" }
    exit 1
}
Write-Task001CommandResult -Path $ResultPath -Command 'Tests/Bootstrap/Task001.Bootstrap.Tests.ps1' -Status PASS -ExitCode 0 -StartedAt $started -Diagnostics @(
    [pscustomobject]@{ status = 'PASS'; code = 'CIV001-SELFTEST-000'; message = 'All TASK-001 command self-tests passed.'; details = @{ passed = @($passedNames) } }
) -RepositoryRoot $root | Out-Null
Write-Host "`nSELF-TEST PASSED ($passed tests)"
exit 0
