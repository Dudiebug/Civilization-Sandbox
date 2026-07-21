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

    Test-Case 'pinned direct package manifest' {
        $manifest = Test-Task001PackageManifest -RepositoryRoot $root
        if ($manifest.status -ne 'PASS') { throw "Expected PASS, got $($manifest.code): $($manifest.mismatches -join '; ')" }
    }

    Test-Case 'copied manifest drift' {
        $copy = Join-Path $temp 'manifest.json'
        $manifest = Get-Content -LiteralPath (Join-Path $root 'Packages\manifest.json') -Raw | ConvertFrom-Json
        $manifest.dependencies.'com.unity.burst' = '0.0.0-tampered'
        $manifest | ConvertTo-Json -Depth 8 | Set-Content -LiteralPath $copy -Encoding UTF8
        $check = Test-Task001PackageManifest -RepositoryRoot $root -ManifestPath $copy
        if ($check.status -ne 'FAIL' -or $check.code -ne 'CIV001-MANIFEST-003') { throw 'Tampered direct manifest did not fail with CIV001-MANIFEST-003.' }
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

    Test-Case 'result Git state uses pinned executable discovery' {
        $state = Get-Task001GitState -RepositoryRoot $root
        if ($state.commit -notmatch '^[0-9a-f]{40}$') { throw 'Result Git commit is absent or invalid.' }
        if ($null -eq $state.dirty) { throw 'Result Git dirty state is null.' }
    }

    Test-Case 'stale command result provenance is rejected' {
        $resultPath = Join-Path $selfTestArtifacts 'stale-result.json'
        Write-Task001CommandResult -Path $resultPath -Command 'self-test' -Status PASS -ExitCode 0 -StartedAt (Get-Date) -RepositoryRoot $root | Out-Null
        $record = Get-Content -LiteralPath $resultPath -Raw | ConvertFrom-Json
        $record.git.commit = '0000000000000000000000000000000000000000'
        $record | ConvertTo-Json -Depth 10 | Set-Content -LiteralPath $resultPath -Encoding UTF8
        $threw = $false
        try { Assert-Task001CommandResultIntegrity -Path $resultPath -ExpectedCommand 'self-test' -RepositoryRoot $root | Out-Null } catch { $threw = $_.Exception.Message -match 'CIV001-RESULT-005' }
        if (-not $threw) { throw 'Stale command result provenance was accepted.' }
    }

    Test-Case 'incomplete build tree hashes are rejected' {
        $tree = Join-Path $selfTestArtifacts 'build-tree'
        New-Item -ItemType Directory -Path $tree -Force | Out-Null
        $first = Join-Path $tree 'launcher.exe'
        $second = Join-Path $tree 'support.data'
        Set-Content -LiteralPath $first -Value 'launcher' -Encoding UTF8
        Set-Content -LiteralPath $second -Value 'support' -Encoding UTF8
        $resultPath = Join-Path $selfTestArtifacts 'incomplete-build-result.json'
        Write-Task001CommandResult -Path $resultPath -Command 'self-test-build' -Status PASS -ExitCode 0 -StartedAt (Get-Date) -ArtifactPaths @($first) -RepositoryRoot $root | Out-Null
        $threw = $false
        try { Assert-Task001CommandResultIntegrity -Path $resultPath -ExpectedCommand 'self-test-build' -ArtifactDirectory $tree -RepositoryRoot $root | Out-Null } catch { $threw = $_.Exception.Message -match 'CIV001-RESULT-012' }
        if (-not $threw) { throw 'Incomplete build output hashes were accepted.' }
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

    Test-Case 'required status-check binding remains a JSON array' {
        $source = Get-Content -LiteralPath (Join-Path $root 'Build\Configure-Repository.ps1') -Raw
        if ($source -notmatch 'checks\s*=\s*\[object\[\]\]@\(') {
            throw 'Configure-Repository.ps1 can collapse the one-item required-check binding into a JSON object.'
        }
        $body = @{ required_status_checks = @{ strict = $true; checks = [object[]]@(@{ context = 'repository-policy'; app_id = 15368 }) } } | ConvertTo-Json -Depth 5 | ConvertFrom-Json
        if ($body.required_status_checks.checks -isnot [array] -or @($body.required_status_checks.checks).Count -ne 1) {
            throw 'Required status-check binding did not round-trip as a one-item JSON array.'
        }
    }

    Test-Case 'artifact-mutating scripts apply path guards' {
        $testSource = Get-Content -LiteralPath (Join-Path $root 'Build\Test.ps1') -Raw
        $packageSource = Get-Content -LiteralPath (Join-Path $root 'Build\Package-Evidence.ps1') -Raw
        if (@([regex]::Matches($testSource, 'Assert-Task001PathWithinRoot')).Count -lt 8 -or $testSource -notmatch 'Assert-Task001CommandResultIntegrity') {
            throw 'Test.ps1 does not guard all fixed artifact paths and player provenance.'
        }
        if ($packageSource -notmatch 'Assert-Task001TreeHasNoReparsePoints -Path \$ArtifactRoot' -or $packageSource -notmatch 'Assert-Task001TreeHasNoReparsePoints -Path \$staging') {
            throw 'Package-Evidence.ps1 does not guard recursive artifact and staging operations.'
        }
        if (@([regex]::Matches($packageSource, '\$_.FullName -ne \$currentResultFull')).Count -lt 2) {
            throw 'Package-Evidence.ps1 does not exclude its current result from both validation and archive enumeration.'
        }
    }

    Test-Case 'governance auditors anchor every CI validation command' {
        $governanceSource = Get-Content -LiteralPath (Join-Path $root 'Build\Configure-Repository.ps1') -Raw
        $validatorSource = Get-Content -LiteralPath (Join-Path $root 'Build\validate_plan.py') -Raw
        foreach ($needle in @('Build/validate_plan.py', 'Build/Configure-Repository.ps1 -Offline', 'Build/Bootstrap.ps1 -RepositoryOnly', 'Tests/Bootstrap/Task001.Bootstrap.Tests.ps1')) {
            if ($governanceSource -notmatch [regex]::Escape($needle) -or $validatorSource -notmatch [regex]::Escape($needle)) {
                throw "Governance auditors do not both anchor required CI command: $needle"
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
