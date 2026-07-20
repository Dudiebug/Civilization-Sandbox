# Repository Governance

`main` is the only long-lived integration branch. Work begins from an accepted `main` commit on a branch named `task/TASK-NNN-description` in a sibling Git worktree. Direct pushes, force-pushes, and branch deletion are prohibited. Every change reaches `main` through a pull request.

The repository is intentionally configured for a solo creator: a pull request is required, but no approving GitHub review count is required. Independent adversarial review is recorded as task evidence and remains mandatory before a task can be accepted.

## TASK-001 transition

- Remote baseline: `aec44fcb4285767716fbd22715fd96233459ab8e`.
- Recovery tag: `task-001-start-aec44fc`.
- Preserved creator edits: `prep/task001-approved-docs`.
- Implementation branch: `task/TASK-001-bootstrap`.
- Required check after its first successful report: `repository-policy`.

`Build/Configure-Repository.ps1` audits by default. `-Apply` is required for GitHub mutations, and the script refuses to apply protection to a non-private repository or when the configured baseline is not present. GitHub CLI authentication is a human prerequisite.

## Recovery

Export the live repository settings before applying a change:

```powershell
pwsh Build/Configure-Repository.ps1 -ExportPath Artifacts/repository-settings-before.json
```

To restore the pre-TASK-001 branch shape without rewriting history:

```powershell
git branch master task-001-start-aec44fc
git push origin refs/tags/task-001-start-aec44fc refs/heads/master
gh api --method PATCH repos/Dudiebug/Civilization-Sandbox -f default_branch=master
pwsh Build/Configure-Repository.ps1 -RollbackSettings Artifacts/repository-settings-before.json -Apply
```

Remove the required check before reverting the workflow. Never delete `main`, `master`, or either recovery tag until the restored default branch and protection response have been inspected.

## Protection availability

The repository must remain private and its GitHub plan must support protected private branches. If GitHub rejects protection, TASK-001 is blocked; the script must not weaken the policy or make the repository public as a workaround.
