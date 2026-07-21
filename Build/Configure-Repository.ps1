#requires -Version 5.1
[CmdletBinding()]
param(
    [switch]$Apply,
    [switch]$RequireCheck,
    [string]$ExportPath,
    [string]$RollbackSettings,
    [switch]$Offline
)

$ErrorActionPreference = 'Stop'
Import-Module (Join-Path $PSScriptRoot 'Task001.Common.psm1') -Force
$root = Get-Task001RepositoryRoot
$policy = Get-Task001Governance -RepositoryRoot $root
$githubActionsAppId = 15368

$policyErrors = New-Object System.Collections.Generic.List[string]
if ([string]$policy.visibility -notin @('private', 'public')) { $policyErrors.Add('visibility must be private or public') }
if ([string]$policy.defaultBranch -ne 'main') { $policyErrors.Add('default branch must be main') }
if ([string]$policy.taskBranchPattern -ne 'task/TASK-NNN-description') { $policyErrors.Add('task branch pattern changed') }
if ([string]$policy.worktreePolicy -ne 'sibling') { $policyErrors.Add('worktree policy changed') }
if (-not $policy.protection.requirePullRequest) { $policyErrors.Add('pull requests are not required') }
if ([int]$policy.protection.requiredApprovingReviewCount -ne 0) { $policyErrors.Add('solo approval count must remain zero') }
if ([string]$policy.protection.requireStatusCheckAfterFirstRun -ne 'repository-policy') { $policyErrors.Add('required status-check name changed') }
if (-not $policy.protection.requireBranchesUpToDate) { $policyErrors.Add('strict status checks are disabled') }
if (-not $policy.protection.enforceAdmins) { $policyErrors.Add('administrator enforcement is disabled') }
if ($policy.protection.allowForcePushes) { $policyErrors.Add('force-pushes are enabled') }
if ($policy.protection.allowDeletions) { $policyErrors.Add('branch deletion is enabled') }
if ([string]$policy.workflow.path -ne '.github/workflows/repository-policy.yml') { $policyErrors.Add('workflow path changed') }
if ([string]$policy.workflow.jobName -ne 'repository-policy') { $policyErrors.Add('workflow job name changed') }
if ([string]$policy.workflow.checkoutAction -ne 'actions/checkout@08c6903cd8c0fde910a37f88322edcfb5dd907a8') { $policyErrors.Add('checkout action pin changed') }
$workflowPath = Join-Path $root ([string]$policy.workflow.path).Replace('/', '\')
if (-not (Test-Path -LiteralPath $workflowPath -PathType Leaf)) {
    $policyErrors.Add('repository-policy workflow is absent')
} else {
    $workflowSource = Get-Content -LiteralPath $workflowPath -Raw
    if (@([regex]::Matches($workflowSource, '(?m)^\s*uses:\s*actions/checkout@08c6903cd8c0fde910a37f88322edcfb5dd907a8\s*$')).Count -ne 1) { $policyErrors.Add('workflow checkout pin is absent or duplicated') }
    if ($workflowSource -notmatch '(?m)^\s{2}repository-policy:\s*$' -or $workflowSource -notmatch '(?m)^\s{4}name:\s*repository-policy\s*$') { $policyErrors.Add('workflow job key or check name changed') }
    if ($workflowSource -notmatch '(?ms)^permissions:\s*\r?\n\s{2}contents:\s*read\s*$') { $policyErrors.Add('workflow permissions are not read-only contents') }
    if ($workflowSource -match '(?i)\bsecrets\s*\.' -or $workflowSource -match '(?i)\$\{\{\s*secrets\.') { $policyErrors.Add('workflow references secrets') }
}
if ($policyErrors.Count -gt 0) {
    throw "CIV001-GOV-002: Committed governance policy does not meet TASK-001 safeguards: $($policyErrors -join '; ')"
}
if ($Offline) {
    if ($Apply -or $RollbackSettings -or $ExportPath) { throw 'CIV001-GOV-003: Offline mode is audit-only.' }
    Write-Host 'PASS: committed repository-governance.json satisfies TASK-001 offline policy.'
    exit 0
}

$gh = Get-Command gh -CommandType Application -ErrorAction SilentlyContinue | Select-Object -First 1
if (-not $gh) { throw 'CIV001-GOV-004: GitHub CLI 2.96.0 is required for live governance operations.' }
& $gh.Source auth status
if ($LASTEXITCODE -ne 0) { throw 'CIV001-GOV-005: Authenticate GitHub CLI with gh auth login.' }
$repo = [string]$policy.repository

function Invoke-GhJson([string[]]$Arguments) {
    $output = & $gh.Source @Arguments 2>&1
    if ($LASTEXITCODE -ne 0) { throw "CIV001-GOV-006: gh failed: $($output | Out-String)" }
    return ($output | Out-String).Trim()
}

$repositoryJson = Invoke-GhJson @('api', "repos/$repo") | ConvertFrom-Json
$configuredVisibility = [string]$policy.visibility
if ($configuredVisibility -notin @('private', 'public')) { throw 'CIV001-GOV-007: Configured repository visibility must be private or public.' }
$expectedPrivate = $configuredVisibility -eq 'private'
if ([bool]$repositoryJson.private -ne $expectedPrivate) { throw "CIV001-GOV-007: Live repository visibility does not match configured visibility '$configuredVisibility'." }

if ($ExportPath) {
    $protection = $null
    $protectedBranch = [string]$repositoryJson.default_branch
    try { $protection = Invoke-GhJson @('api', "repos/$repo/branches/$protectedBranch/protection") | ConvertFrom-Json } catch { $protection = @{ unavailable = $_.Exception.Message } }
    [pscustomobject]@{ schemaVersion = 1; capturedAtUtc = [datetime]::UtcNow.ToString('o'); repository = $repositoryJson; protectedBranch = $protectedBranch; branchProtection = $protection } |
        ConvertTo-Json -Depth 30 | Set-Content -LiteralPath $ExportPath -Encoding UTF8
    Write-Host "Exported: $ExportPath"
    exit 0
}

if ($RollbackSettings) {
    if (-not $Apply) { throw 'CIV001-GOV-008: Rollback requires -Apply.' }
    $prior = Get-Content -LiteralPath $RollbackSettings -Raw | ConvertFrom-Json
    $priorDefault = [string]$prior.repository.default_branch
    if (-not $priorDefault) { throw 'CIV001-GOV-009: Rollback export has no prior default branch.' }
    Invoke-GhJson @('api', '--method', 'PATCH', "repos/$repo", '-f', "default_branch=$priorDefault") | Out-Null
    $priorProtection = $prior.branchProtection
    if (-not $priorProtection -or ($priorProtection.PSObject.Properties.Name -contains 'unavailable')) {
        $deleteOutput = & $gh.Source api --method DELETE "repos/$repo/branches/$priorDefault/protection" 2>&1
        if ($LASTEXITCODE -ne 0 -and ($deleteOutput | Out-String) -notmatch 'Branch not protected|404') {
            throw "CIV001-GOV-012: Could not restore the prior unprotected state: $($deleteOutput | Out-String)"
        }
    } else {
        $review = $priorProtection.required_pull_request_reviews
        $statusChecks = $priorProtection.required_status_checks
        $restoreBody = [pscustomobject][ordered]@{
            required_status_checks = if ($statusChecks) { @{ strict = [bool]$statusChecks.strict; checks = [object[]]@($statusChecks.checks | ForEach-Object { @{ context = [string]$_.context; app_id = [int]$_.app_id } }) } } else { $null }
            enforce_admins = [bool]$priorProtection.enforce_admins.enabled
            required_pull_request_reviews = if ($review) {
                @{
                    dismiss_stale_reviews = [bool]$review.dismiss_stale_reviews
                    require_code_owner_reviews = [bool]$review.require_code_owner_reviews
                    required_approving_review_count = [int]$review.required_approving_review_count
                    require_last_push_approval = [bool]$review.require_last_push_approval
                }
            } else { $null }
            restrictions = if ($priorProtection.restrictions) {
                @{
                    users = @($priorProtection.restrictions.users | ForEach-Object { $_.login })
                    teams = @($priorProtection.restrictions.teams | ForEach-Object { $_.slug })
                    apps = @($priorProtection.restrictions.apps | ForEach-Object { $_.slug })
                }
            } else { $null }
            required_linear_history = [bool]$priorProtection.required_linear_history.enabled
            allow_force_pushes = [bool]$priorProtection.allow_force_pushes.enabled
            allow_deletions = [bool]$priorProtection.allow_deletions.enabled
            required_conversation_resolution = [bool]$priorProtection.required_conversation_resolution.enabled
            block_creations = [bool]$priorProtection.block_creations.enabled
            lock_branch = [bool]$priorProtection.lock_branch.enabled
            allow_fork_syncing = [bool]$priorProtection.allow_fork_syncing.enabled
        }
        $restoreTemp = Join-Path ([System.IO.Path]::GetTempPath()) ("task001-restore-protection-" + [guid]::NewGuid().ToString('N') + '.json')
        try {
            $restoreBody | ConvertTo-Json -Depth 10 | Set-Content -LiteralPath $restoreTemp -Encoding UTF8
            $null = Get-Content -LiteralPath $restoreTemp -Raw | & $gh.Source api --method PUT "repos/$repo/branches/$priorDefault/protection" --input -
            if ($LASTEXITCODE -ne 0) { throw 'CIV001-GOV-013: GitHub rejected restoration of prior branch protection.' }
        } finally {
            if (Test-Path -LiteralPath $restoreTemp) { Remove-Item -LiteralPath $restoreTemp -Force }
        }
        $signatureMethod = if ($priorProtection.required_signatures.enabled) { 'POST' } else { 'DELETE' }
        $signatureOutput = & $gh.Source api --method $signatureMethod "repos/$repo/branches/$priorDefault/protection/required_signatures" 2>&1
        if ($LASTEXITCODE -ne 0 -and ($signatureOutput | Out-String) -notmatch '404') {
            throw 'CIV001-GOV-014: Could not restore the prior signed-commit requirement.'
        }
    }
    Write-Host "Restored default branch: $priorDefault"
    Write-Host 'Restored the exported default-branch protection state.'
    exit 0
}

if (-not $Apply) {
    $protectionJson = Invoke-GhJson @('api', "repos/$repo/branches/main/protection") | ConvertFrom-Json
    $errors = @()
    if ($repositoryJson.default_branch -ne 'main') { $errors += 'default branch is not main' }
    if ($protectionJson.allow_force_pushes.enabled) { $errors += 'force-pushes are enabled' }
    if ($protectionJson.allow_deletions.enabled) { $errors += 'deletion is enabled' }
    if (-not $protectionJson.required_pull_request_reviews) { $errors += 'pull requests are not required' }
    if ($protectionJson.required_pull_request_reviews -and [int]$protectionJson.required_pull_request_reviews.required_approving_review_count -ne [int]$policy.protection.requiredApprovingReviewCount) { $errors += 'approval count differs from policy' }
    if (-not $protectionJson.enforce_admins.enabled) { $errors += 'administrator enforcement is disabled' }
    if (-not $protectionJson.required_conversation_resolution.enabled) { $errors += 'conversation resolution is not required' }
    if ($RequireCheck) {
        $expectedContext = [string]$policy.protection.requireStatusCheckAfterFirstRun
        $contexts = @($protectionJson.required_status_checks.contexts)
        $boundChecks = @($protectionJson.required_status_checks.checks | Where-Object { [string]$_.context -eq $expectedContext -and [int]$_.app_id -eq $githubActionsAppId })
        if ($contexts.Count -ne 1 -or $contexts -notcontains $expectedContext) { $errors += 'repository-policy is not the unique required check' }
        if (-not $protectionJson.required_status_checks.strict) { $errors += 'required status checks are not strict' }
        if ($boundChecks.Count -ne 1) { $errors += 'repository-policy is not bound to the GitHub Actions app' }
    }
    if ($errors.Count) { throw "CIV001-GOV-010: Live governance audit failed: $($errors -join '; ')" }
    Write-Host 'PASS: live main branch governance matches TASK-001 safeguards.'
    exit 0
}

if ($repositoryJson.default_branch -ne 'main') {
    Invoke-GhJson @('api', '--method', 'PATCH', "repos/$repo", '-f', 'default_branch=main') | Out-Null
}
$requiredContext = [string]$policy.protection.requireStatusCheckAfterFirstRun
$body = [pscustomobject][ordered]@{
    required_status_checks = if ($RequireCheck) { @{ strict = [bool]$policy.protection.requireBranchesUpToDate; checks = [object[]]@(@{ context = $requiredContext; app_id = $githubActionsAppId }) } } else { $null }
    enforce_admins = [bool]$policy.protection.enforceAdmins
    required_pull_request_reviews = @{ dismiss_stale_reviews = $false; require_code_owner_reviews = $false; required_approving_review_count = [int]$policy.protection.requiredApprovingReviewCount }
    restrictions = $null
    allow_force_pushes = $false
    allow_deletions = $false
    required_conversation_resolution = $true
}
$temp = Join-Path ([System.IO.Path]::GetTempPath()) ("task001-protection-" + [guid]::NewGuid().ToString('N') + '.json')
try {
    $body | ConvertTo-Json -Depth 10 | Set-Content -LiteralPath $temp -Encoding UTF8
    $null = Get-Content -LiteralPath $temp -Raw | & $gh.Source api --method PUT "repos/$repo/branches/main/protection" --input -
    if ($LASTEXITCODE -ne 0) { throw 'CIV001-GOV-011: GitHub rejected branch protection. TASK-001 is blocked.' }
} finally {
    if (Test-Path -LiteralPath $temp) { Remove-Item -LiteralPath $temp -Force }
}
Write-Host "Applied protected main policy. Required check enabled: $([bool]$RequireCheck)"
