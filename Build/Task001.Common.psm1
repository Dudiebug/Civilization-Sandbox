Set-StrictMode -Version Latest

function Get-Task001RepositoryRoot {
    return [System.IO.Path]::GetFullPath((Split-Path -Parent $PSScriptRoot))
}

function Get-Task001Toolchain {
    param([string]$RepositoryRoot = (Get-Task001RepositoryRoot))
    $path = Join-Path $RepositoryRoot 'Config\toolchain.json'
    if (-not (Test-Path -LiteralPath $path -PathType Leaf)) {
        throw "CIV001-CONFIG-001: Missing toolchain contract: $path"
    }
    return Get-Content -LiteralPath $path -Raw | ConvertFrom-Json
}

function Get-Task001Governance {
    param([string]$RepositoryRoot = (Get-Task001RepositoryRoot))
    $path = Join-Path $RepositoryRoot 'Config\repository-governance.json'
    if (-not (Test-Path -LiteralPath $path -PathType Leaf)) {
        throw "CIV001-GOV-001: Missing governance contract: $path"
    }
    return Get-Content -LiteralPath $path -Raw | ConvertFrom-Json
}

function Get-Task001Sha256 {
    param([Parameter(Mandatory = $true)][string]$Path)
    if (-not (Test-Path -LiteralPath $Path -PathType Leaf)) { return $null }
    return (Get-FileHash -LiteralPath $Path -Algorithm SHA256).Hash.ToLowerInvariant()
}

function Test-Task001PathRedirector {
    param([Parameter(Mandatory = $true)][System.IO.FileSystemInfo]$Item)
    if (($Item.Attributes -band [System.IO.FileAttributes]::ReparsePoint) -eq 0) { return $false }

    # OneDrive uses non-redirecting cloud reparse points for ordinary files and
    # directories. PowerShell exposes path-redirecting reparse points (junctions
    # and symbolic links) through LinkType/Target, which are the escape risk.
    $linkType = $Item.PSObject.Properties['LinkType']
    if ($linkType -and -not [string]::IsNullOrWhiteSpace([string]$linkType.Value)) { return $true }
    $target = $Item.PSObject.Properties['Target']
    if ($target -and $null -ne $target.Value -and @($target.Value).Count -gt 0) {
        $targetText = (@($target.Value) | ForEach-Object { [string]$_ }) -join ''
        if (-not [string]::IsNullOrWhiteSpace($targetText)) { return $true }
    }
    return $false
}

function Assert-Task001PathWithinRoot {
    param(
        [Parameter(Mandatory = $true)][string]$RepositoryRoot,
        [Parameter(Mandatory = $true)][string]$TargetPath
    )
    $root = [System.IO.Path]::GetFullPath($RepositoryRoot).TrimEnd('\', '/')
    $target = [System.IO.Path]::GetFullPath($TargetPath).TrimEnd('\', '/')
    if ($target.Equals($root, [System.StringComparison]::OrdinalIgnoreCase) -or
        -not $target.StartsWith($root + [System.IO.Path]::DirectorySeparatorChar, [System.StringComparison]::OrdinalIgnoreCase)) {
        throw "CIV001-CLEAN-001: Refusing path outside the repository or the repository root itself: $target"
    }
    $probe = $target
    while (-not $probe.Equals($root, [System.StringComparison]::OrdinalIgnoreCase)) {
        if (Test-Path -LiteralPath $probe) {
            $probeItem = Get-Item -LiteralPath $probe -Force
            if (Test-Task001PathRedirector -Item $probeItem) {
                throw "CIV001-PATH-002: Refusing a repository mutation through a reparse point: $probe"
            }
        }
        $parent = Split-Path -Parent $probe
        if (-not $parent -or $parent -eq $probe) { break }
        $probe = $parent
    }
    return $target
}

function Assert-Task001TreeHasNoReparsePoints {
    param([Parameter(Mandatory = $true)][string]$Path)
    if (-not (Test-Path -LiteralPath $Path -PathType Container)) { return }
    $reparsePoints = @(Get-ChildItem -LiteralPath $Path -Recurse -Force -ErrorAction Stop | Where-Object {
        Test-Task001PathRedirector -Item $_
    })
    if ($reparsePoints.Count -gt 0) {
        throw "CIV001-PATH-003: Refusing recursive mutation because the target tree contains a reparse point: $($reparsePoints[0].FullName)"
    }
}

function Resolve-Task001ToolExecutable {
    param([Parameter(Mandatory = $true)][pscustomobject]$Tool)
    $candidates = New-Object System.Collections.Generic.List[string]
    try {
        $command = Get-Command $Tool.command -CommandType Application -ErrorAction Stop | Select-Object -First 1
        if ($command -and $command.Source) { $candidates.Add($command.Source) }
    } catch { }

    switch ($Tool.wingetId) {
        'Git.Git' {
            $candidates.Add((Join-Path $env:ProgramFiles 'Git\cmd\git.exe'))
            if ($env:LocalAppData) { $candidates.Add((Join-Path $env:LocalAppData 'Programs\Git\cmd\git.exe')) }
        }
        'Microsoft.PowerShell' { $candidates.Add((Join-Path $env:ProgramFiles 'PowerShell\7\pwsh.exe')) }
        'Python.Python.3.13' {
            $candidates.Add((Join-Path $env:ProgramFiles 'Python313\python.exe'))
            if ($env:LocalAppData) { $candidates.Add((Join-Path $env:LocalAppData 'Programs\Python\Python313\python.exe')) }
        }
        'GitHub.cli' {
            $candidates.Add((Join-Path $env:ProgramFiles 'GitHub CLI\gh.exe'))
            if ($env:LocalAppData) { $candidates.Add((Join-Path $env:LocalAppData 'Programs\GitHub CLI\gh.exe')) }
        }
        'UnityTechnologies.UnityHub' { $candidates.Add((Join-Path $env:ProgramFiles 'Unity Hub\Unity Hub.exe')) }
    }

    foreach ($candidate in $candidates | Select-Object -Unique) {
        if (-not (Test-Path -LiteralPath $candidate -PathType Leaf)) { continue }
        if ($Tool.wingetId -eq 'Python.Python.3.13' -and $candidate -match '\\WindowsApps\\') {
            continue
        }
        return [System.IO.Path]::GetFullPath($candidate)
    }
    return $null
}

function Get-Task001ToolVersion {
    param(
        [Parameter(Mandatory = $true)][pscustomobject]$Tool,
        [Parameter(Mandatory = $true)][string]$Executable
    )
    if ($Tool.wingetId -eq 'UnityTechnologies.UnityHub') {
        return [System.Diagnostics.FileVersionInfo]::GetVersionInfo($Executable).FileVersion
    }
    $output = switch ($Tool.wingetId) {
        'Git.Git' { & $Executable --version 2>&1 }
        'Microsoft.PowerShell' { & $Executable -NoLogo -NoProfile -Command '$PSVersionTable.PSVersion.ToString()' 2>&1 }
        'Python.Python.3.13' { & $Executable --version 2>&1 }
        'GitHub.cli' { & $Executable --version 2>&1 | Select-Object -First 1 }
    }
    $text = ($output | Out-String).Trim()
    if ($Tool.wingetId -eq 'Git.Git' -and $text -match '(\d+\.\d+\.\d+)\.windows\.(\d+)') {
        return "$($Matches[1]).$($Matches[2])"
    }
    if ($text -match '(\d+\.\d+\.\d+(?:\.\d+)?)') { return $Matches[1] }
    return $text
}

function Test-Task001Tool {
    param([Parameter(Mandatory = $true)][pscustomobject]$Tool)
    $executable = Resolve-Task001ToolExecutable -Tool $Tool
    if (-not $executable) {
        return [pscustomobject][ordered]@{
            name = $Tool.name; status = 'FAIL'; code = 'CIV001-TOOL-001'; expected = $Tool.version
            actual = $null; path = $null; message = "Pinned prerequisite is not installed or discoverable: $($Tool.name) $($Tool.version)."
        }
    }
    try { $actual = Get-Task001ToolVersion -Tool $Tool -Executable $executable }
    catch {
        return [pscustomobject][ordered]@{
            name = $Tool.name; status = 'FAIL'; code = 'CIV001-TOOL-002'; expected = $Tool.version
            actual = $null; path = $executable; message = "Could not read tool version: $($_.Exception.Message)"
        }
    }
    $expectedValues = @([string]$Tool.version)
    if ($Tool.PSObject.Properties.Name -contains 'displayVersion') { $expectedValues += [string]$Tool.displayVersion }
    $status = if ($expectedValues -contains $actual) { 'PASS' } else { 'FAIL' }
    $code = if ($status -eq 'PASS') { 'CIV001-TOOL-000' } else { 'CIV001-TOOL-003' }
    $message = if ($status -eq 'PASS') { 'Pinned version verified.' } else { 'Installed version differs from the pin; no downgrade or replacement was attempted.' }
    return [pscustomobject][ordered]@{
        name = $Tool.name; status = $status; code = $code; expected = $Tool.version
        actual = $actual; path = $executable; message = $message
    }
}

function Resolve-Task001Unity {
    param(
        [string]$UnityPath,
        [string]$RepositoryRoot = (Get-Task001RepositoryRoot),
        [string[]]$RequiredModules,
        [switch]$SkipHash
    )
    $contract = Get-Task001Toolchain -RepositoryRoot $RepositoryRoot
    if (-not $UnityPath) {
        $UnityPath = Join-Path $env:ProgramFiles "Unity\Hub\Editor\$($contract.unity.version)\Editor\Unity.exe"
    }
    if (-not (Test-Path -LiteralPath $UnityPath -PathType Leaf)) {
        throw "CIV001-UNITY-001: Unity editor was not found at the resolved path: $UnityPath"
    }
    $full = [System.IO.Path]::GetFullPath($UnityPath)
    $version = [System.Diagnostics.FileVersionInfo]::GetVersionInfo($full).ProductVersion
    $expectedPrefix = "$($contract.unity.version)_$($contract.unity.changeset)"
    if (-not $version.StartsWith($expectedPrefix, [System.StringComparison]::OrdinalIgnoreCase)) {
        throw "CIV001-UNITY-002: Unity product version '$version' does not match '$expectedPrefix'."
    }
    if (-not $SkipHash) {
        $hash = Get-Task001Sha256 -Path $full
        if ($hash -ne ([string]$contract.unity.editorSha256).ToLowerInvariant()) {
            throw "CIV001-UNITY-003: Unity executable SHA-256 mismatch."
        }
    }
    $playbackRoot = Join-Path (Split-Path -Parent $full) 'Data\PlaybackEngines'
    $moduleFolders = @{
        'windows-mono' = 'windowsstandalonesupport\Variations\win64_player_nondevelopment_mono'
        'linux-mono' = 'LinuxStandaloneSupport\Variations\linux64_player_nondevelopment_mono'
    }
    $modulesToCheck = if ($PSBoundParameters.ContainsKey('RequiredModules')) { @($RequiredModules) } else { @($contract.unity.installModules) }
    foreach ($module in $modulesToCheck) {
        $folder = $moduleFolders[[string]$module]
        if (-not $folder -or -not (Test-Path -LiteralPath (Join-Path $playbackRoot $folder) -PathType Container)) {
            throw "CIV001-UNITY-004: Required Unity module is missing: $module"
        }
    }
    return $full
}

function Test-Task001PackageLock {
    param(
        [string]$RepositoryRoot = (Get-Task001RepositoryRoot),
        [string]$LockPath
    )
    $contract = Get-Task001Toolchain -RepositoryRoot $RepositoryRoot
    if (-not $LockPath) { $LockPath = Join-Path $RepositoryRoot 'Packages\packages-lock.json' }
    if (-not (Test-Path -LiteralPath $LockPath -PathType Leaf)) {
        return [pscustomobject]@{ status = 'FAIL'; code = 'CIV001-LOCK-001'; expected = $contract.unity.packageLockSha256; actual = $null; path = $LockPath }
    }
    $actual = Get-Task001Sha256 -Path $LockPath
    $expected = ([string]$contract.unity.packageLockSha256).ToLowerInvariant()
    if ($expected -eq 'pending_unity_resolution') {
        return [pscustomobject]@{ status = 'FAIL'; code = 'CIV001-LOCK-002'; expected = $expected; actual = $actual; path = $LockPath }
    }
    $status = if ($actual -eq $expected) { 'PASS' } else { 'FAIL' }
    $code = if ($status -eq 'PASS') { 'CIV001-LOCK-000' } else { 'CIV001-LOCK-003' }
    return [pscustomobject]@{ status = $status; code = $code; expected = $expected; actual = $actual; path = $LockPath }
}

function Test-Task001PackageManifest {
    param(
        [string]$RepositoryRoot = (Get-Task001RepositoryRoot),
        [string]$ManifestPath
    )
    $contract = Get-Task001Toolchain -RepositoryRoot $RepositoryRoot
    if (-not $ManifestPath) { $ManifestPath = Join-Path $RepositoryRoot 'Packages\manifest.json' }
    if (-not (Test-Path -LiteralPath $ManifestPath -PathType Leaf)) {
        return [pscustomobject]@{ status = 'FAIL'; code = 'CIV001-MANIFEST-001'; path = $ManifestPath; mismatches = @('manifest is absent') }
    }
    try {
        $manifest = Get-Content -LiteralPath $ManifestPath -Raw | ConvertFrom-Json
        $expectedNames = @($contract.packages.PSObject.Properties.Name | Sort-Object)
        $actualNames = @($manifest.dependencies.PSObject.Properties.Name | Sort-Object)
        $mismatches = New-Object System.Collections.Generic.List[string]
        $nameDiff = @(Compare-Object -ReferenceObject $expectedNames -DifferenceObject $actualNames)
        if ($nameDiff.Count -gt 0) { $mismatches.Add('direct dependency names differ from Config/toolchain.json') }
        foreach ($name in $expectedNames) {
            $expected = [string]$contract.packages.$name
            $actualProperty = $manifest.dependencies.PSObject.Properties[$name]
            $actual = if ($actualProperty) { [string]$actualProperty.Value } else { $null }
            if ($actual -ne $expected) { $mismatches.Add("$name expected '$expected' but found '$actual'") }
        }
        $expectedTestables = if ($contract.PSObject.Properties['testables']) {
            @($contract.testables | ForEach-Object { [string]$_ } | Sort-Object)
        } else {
            @('com.civsandbox.tooling')
        }
        $actualTestables = @($manifest.testables | ForEach-Object { [string]$_ } | Sort-Object)
        $testableDiff = @(Compare-Object -ReferenceObject $expectedTestables -DifferenceObject $actualTestables)
        if ($testableDiff.Count -gt 0) { $mismatches.Add('testables differ from Config/toolchain.json') }
        $status = if ($mismatches.Count -eq 0) { 'PASS' } else { 'FAIL' }
        $code = if ($status -eq 'PASS') { 'CIV001-MANIFEST-000' } else { 'CIV001-MANIFEST-003' }
        return [pscustomobject]@{ status = $status; code = $code; path = $ManifestPath; mismatches = @($mismatches) }
    } catch {
        return [pscustomobject]@{ status = 'FAIL'; code = 'CIV001-MANIFEST-002'; path = $ManifestPath; mismatches = @($_.Exception.Message) }
    }
}

function Get-Task001GitState {
    param([string]$RepositoryRoot = (Get-Task001RepositoryRoot))
    try {
        $contract = Get-Task001Toolchain -RepositoryRoot $RepositoryRoot
        $gitTool = $contract.tools | Where-Object { $_.wingetId -eq 'Git.Git' } | Select-Object -First 1
        $git = if ($gitTool) { Resolve-Task001ToolExecutable -Tool $gitTool } else { $null }
        if (-not $git) { throw 'Pinned Git executable was not found.' }
        $commit = (& $git -C $RepositoryRoot rev-parse HEAD 2>$null | Out-String).Trim()
        $dirty = [bool]((& $git -C $RepositoryRoot status --porcelain 2>$null | Out-String).Trim())
        return [pscustomobject]@{ commit = $commit; dirty = $dirty }
    } catch {
        return [pscustomobject]@{ commit = $null; dirty = $null }
    }
}

function New-Task001ArtifactRecord {
    param(
        [Parameter(Mandatory = $true)][string]$Path,
        [string]$RepositoryRoot = (Get-Task001RepositoryRoot)
    )
    if (-not (Test-Path -LiteralPath $Path -PathType Leaf)) { return $null }
    $full = Assert-Task001PathWithinRoot -RepositoryRoot $RepositoryRoot -TargetPath $Path
    $relative = $full
    $rootPrefix = [System.IO.Path]::GetFullPath($RepositoryRoot).TrimEnd('\', '/') + [System.IO.Path]::DirectorySeparatorChar
    if ($full.StartsWith($rootPrefix, [System.StringComparison]::OrdinalIgnoreCase)) { $relative = $full.Substring($rootPrefix.Length).Replace('\', '/') }
    $item = Get-Item -LiteralPath $full
    return [pscustomobject][ordered]@{ path = $relative; sha256 = Get-Task001Sha256 -Path $full; bytes = $item.Length }
}

function Assert-Task001CommandResultIntegrity {
    param(
        [Parameter(Mandatory = $true)][string]$Path,
        [string]$ExpectedCommand,
        [string]$ArtifactDirectory,
        [string]$RepositoryRoot = (Get-Task001RepositoryRoot)
    )
    $Path = Assert-Task001PathWithinRoot -RepositoryRoot $RepositoryRoot -TargetPath $Path
    if (-not (Test-Path -LiteralPath $Path -PathType Leaf)) { throw "CIV001-RESULT-001: Required command result is absent: $Path" }
    try { $result = Get-Content -LiteralPath $Path -Raw | ConvertFrom-Json } catch { throw "CIV001-RESULT-002: Command result is unreadable: $Path" }
    $git = Get-Task001GitState -RepositoryRoot $RepositoryRoot
    $contract = Get-Task001Toolchain -RepositoryRoot $RepositoryRoot
    $lock = Test-Task001PackageLock -RepositoryRoot $RepositoryRoot
    if ($result.schemaVersion -ne 1 -or $result.status -ne 'PASS' -or [int]$result.exitCode -ne 0) {
        throw "CIV001-RESULT-003: Command result is not a versioned zero-exit PASS: $Path"
    }
    if ($ExpectedCommand -and [string]$result.command -ne $ExpectedCommand) {
        throw "CIV001-RESULT-004: Command result '$Path' does not record '$ExpectedCommand'."
    }
    if ($git.commit -notmatch '^[0-9a-f]{40}$' -or $git.dirty -ne $false -or [string]$result.git.commit -ne [string]$git.commit -or $result.git.dirty -ne $false) {
        throw "CIV001-RESULT-005: Command result is stale, dirty, or not tied to the clean candidate commit: $Path"
    }
    if ([string]$result.unityVersion -ne [string]$contract.unity.version -or [string]$result.packageLockSha256 -ne [string]$lock.expected -or $lock.status -ne 'PASS') {
        throw "CIV001-RESULT-006: Command result toolchain or package-lock provenance does not match the candidate: $Path"
    }
    $seen = @{}
    foreach ($artifact in @($result.artifacts)) {
        $relative = [string]$artifact.path
        if ([string]::IsNullOrWhiteSpace($relative) -or [System.IO.Path]::IsPathRooted($relative)) { throw "CIV001-RESULT-007: Artifact path is not repository-relative in $Path" }
        $full = Assert-Task001PathWithinRoot -RepositoryRoot $RepositoryRoot -TargetPath (Join-Path $RepositoryRoot $relative.Replace('/', '\'))
        if (-not (Test-Path -LiteralPath $full -PathType Leaf)) { throw "CIV001-RESULT-008: Recorded artifact is absent: $relative" }
        $normalized = $relative.Replace('\', '/').ToLowerInvariant()
        if ($seen.ContainsKey($normalized)) { throw "CIV001-RESULT-009: Duplicate artifact record: $relative" }
        $seen[$normalized] = $true
        $item = Get-Item -LiteralPath $full
        if ([string]$artifact.sha256 -ne (Get-Task001Sha256 -Path $full) -or [int64]$artifact.bytes -ne [int64]$item.Length) {
            throw "CIV001-RESULT-010: Recorded artifact hash or size does not match: $relative"
        }
    }
    if ($ArtifactDirectory) {
        $ArtifactDirectory = Assert-Task001PathWithinRoot -RepositoryRoot $RepositoryRoot -TargetPath $ArtifactDirectory
        if (-not (Test-Path -LiteralPath $ArtifactDirectory -PathType Container)) { throw "CIV001-RESULT-011: Artifact directory is absent: $ArtifactDirectory" }
        Assert-Task001TreeHasNoReparsePoints -Path $ArtifactDirectory
        $actual = @(Get-ChildItem -LiteralPath $ArtifactDirectory -File -Recurse | ForEach-Object {
            $_.FullName.Substring([System.IO.Path]::GetFullPath($RepositoryRoot).TrimEnd('\', '/').Length + 1).Replace('\', '/').ToLowerInvariant()
        } | Sort-Object)
        $directoryPrefix = ([System.IO.Path]::GetFullPath($ArtifactDirectory).Substring([System.IO.Path]::GetFullPath($RepositoryRoot).TrimEnd('\', '/').Length + 1).Replace('\', '/').TrimEnd('/') + '/').ToLowerInvariant()
        $recorded = @(@($result.artifacts) | ForEach-Object { ([string]$_.path).Replace('\', '/').ToLowerInvariant() } | Where-Object { $_.StartsWith($directoryPrefix) } | Sort-Object)
        if (@(Compare-Object -ReferenceObject $actual -DifferenceObject $recorded).Count -gt 0) {
            throw "CIV001-RESULT-012: Result does not hash the complete build output tree: $ArtifactDirectory"
        }
    }
    return $result
}

function Write-Task001CommandResult {
    param(
        [Parameter(Mandatory = $true)][string]$Path,
        [Parameter(Mandatory = $true)][string]$Command,
        [Parameter(Mandatory = $true)][ValidateSet('PASS', 'FAIL')][string]$Status,
        [Parameter(Mandatory = $true)][int]$ExitCode,
        [Parameter(Mandatory = $true)][datetime]$StartedAt,
        [object[]]$Diagnostics = @(),
        [string[]]$ArtifactPaths = @(),
        [string]$RepositoryRoot = (Get-Task001RepositoryRoot)
    )
    $directory = Split-Path -Parent $Path
    if ($directory) { New-Item -ItemType Directory -Path $directory -Force | Out-Null }
    $contract = Get-Task001Toolchain -RepositoryRoot $RepositoryRoot
    $git = Get-Task001GitState -RepositoryRoot $RepositoryRoot
    $lockPath = Join-Path $RepositoryRoot 'Packages\packages-lock.json'
    $artifacts = @($ArtifactPaths | ForEach-Object { New-Task001ArtifactRecord -Path $_ -RepositoryRoot $RepositoryRoot } | Where-Object { $_ })
    $result = [pscustomobject][ordered]@{
        schemaVersion = 1
        command = $Command
        status = $Status
        exitCode = $ExitCode
        startedAtUtc = $StartedAt.ToUniversalTime().ToString('o')
        finishedAtUtc = [datetime]::UtcNow.ToString('o')
        git = $git
        unityVersion = $contract.unity.version
        packageLockSha256 = Get-Task001Sha256 -Path $lockPath
        diagnostics = @($Diagnostics)
        artifacts = $artifacts
    }
    $result | ConvertTo-Json -Depth 10 | Set-Content -LiteralPath $Path -Encoding UTF8
    return $result
}

function Get-Task001EvidenceRequirements {
    param(
        [string]$RepositoryRoot = (Get-Task001RepositoryRoot),
        [string]$ArtifactRoot = (Join-Path (Get-Task001RepositoryRoot) 'Artifacts')
    )
    return @(
        (Join-Path $RepositoryRoot 'docs\evidence\TASK-001\EVIDENCE.md'),
        (Join-Path $RepositoryRoot 'docs\evidence\TASK-001\MANIFEST.json'),
        (Join-Path $RepositoryRoot 'docs\evidence\TASK-001\REVIEW.md'),
        (Join-Path $RepositoryRoot 'docs\evidence\TASK-001\ACCEPTANCE.md'),
        (Join-Path $ArtifactRoot 'build\windows\CivilizationSandboxBootstrap.exe'),
        (Join-Path $ArtifactRoot 'build\linux\CivilizationSandboxBootstrap.x86_64'),
        (Join-Path $ArtifactRoot 'tests\bootstrap-editmode.xml'),
        (Join-Path $ArtifactRoot 'results\bootstrap-run-1.json'),
        (Join-Path $ArtifactRoot 'results\bootstrap-run-2.json'),
        (Join-Path $ArtifactRoot 'results\build-windows.json'),
        (Join-Path $ArtifactRoot 'results\build-linux.json'),
        (Join-Path $ArtifactRoot 'results\test-bootstrap.json'),
        (Join-Path $ArtifactRoot 'results\task001-script-self-tests.json')
    )
}

Export-ModuleMember -Function @(
    'Get-Task001RepositoryRoot', 'Get-Task001Toolchain', 'Get-Task001Governance', 'Get-Task001Sha256',
    'Assert-Task001PathWithinRoot', 'Assert-Task001TreeHasNoReparsePoints', 'Resolve-Task001ToolExecutable', 'Get-Task001ToolVersion', 'Test-Task001Tool',
    'Resolve-Task001Unity', 'Test-Task001PackageLock', 'Test-Task001PackageManifest', 'Get-Task001GitState', 'New-Task001ArtifactRecord',
    'Assert-Task001CommandResultIntegrity', 'Write-Task001CommandResult', 'Get-Task001EvidenceRequirements'
)
