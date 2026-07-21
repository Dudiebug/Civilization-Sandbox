# TASK-001 Windows Commands

Run commands from the repository root. All commands are compatible with Windows PowerShell 5.1; `pwsh` is the pinned shell for normal creator use.

```powershell
# Read-only full audit. Does not install or activate anything.
powershell -NoProfile -ExecutionPolicy Bypass -File Build/Bootstrap.ps1

# Explicitly authorize exact missing prerequisite installs through WinGet.
powershell -NoProfile -ExecutionPolicy Bypass -File Build/Bootstrap.ps1 -InstallPrerequisites

# Repository/package-integrity audit used by minimal CI.
powershell -NoProfile -ExecutionPolicy Bypass -File Build/Bootstrap.ps1 -RepositoryOnly

powershell -NoProfile -ExecutionPolicy Bypass -File Build/Bootstrap.ps1 -ResultPath Artifacts/results/bootstrap-run-1.json
powershell -NoProfile -ExecutionPolicy Bypass -File Build/Bootstrap.ps1 -ResultPath Artifacts/results/bootstrap-run-2.json
powershell -NoProfile -ExecutionPolicy Bypass -File Build/Build.ps1 -Target Windows -ResultPath Artifacts/results/build-windows.json
powershell -NoProfile -ExecutionPolicy Bypass -File Build/Build.ps1 -Target Linux -ResultPath Artifacts/results/build-linux.json
powershell -NoProfile -ExecutionPolicy Bypass -File Build/Test.ps1 -Suite Bootstrap -ResultPath Artifacts/results/test-bootstrap.json
powershell -NoProfile -ExecutionPolicy Bypass -File Tests/Bootstrap/Task001.Bootstrap.Tests.ps1 -ResultPath Artifacts/results/task001-script-self-tests.json
powershell -NoProfile -ExecutionPolicy Bypass -File Build/Package-Evidence.ps1 -TaskId TASK-001
```

Bootstrap never activates a Unity license, downgrades a newer tool, uninstalls software, or repairs package drift. Git and WinGet are pre-clone prerequisites. Unity Hub account sign-in and a valid editor license are human prerequisites.

## Cleanup

`Clean.ps1` only reports its exact whitelist unless `-Execute` is supplied:

```powershell
powershell -NoProfile -ExecutionPolicy Bypass -File Build/Clean.ps1 -Scope Outputs
powershell -NoProfile -ExecutionPolicy Bypass -File Build/Clean.ps1 -Scope Outputs -Execute
powershell -NoProfile -ExecutionPolicy Bypass -File Build/Clean.ps1 -Scope FullGenerated -Execute
```

It can remove only known generated children (`Artifacts`, `Library`, `Temp`, `Obj`, `Logs`, `UserSettings`, and named build/test outputs). It refuses the repository root, outside paths, and a substituted repository.

## Governance and clean reproduction

```powershell
powershell -NoProfile -ExecutionPolicy Bypass -File Build/Configure-Repository.ps1 -Offline
powershell -NoProfile -ExecutionPolicy Bypass -File Build/Configure-Repository.ps1
powershell -NoProfile -ExecutionPolicy Bypass -File Build/Verify-CleanCheckout.ps1
powershell -NoProfile -ExecutionPolicy Bypass -File Tests/Bootstrap/Task001.Bootstrap.Tests.ps1
python Build/validate_plan.py
```

Live governance audit and `-Apply` require authenticated GitHub CLI `2.96.0`. Run `-Apply` without `-RequireCheck` for initial protection; add `-RequireCheck` only after the workflow has reported successfully once.

Command results use schema version 1 JSON under `Artifacts/results/` and include command, status, exit code, UTC timestamps, Git commit/dirty state, Unity version, package-lock hash, stable diagnostics, and artifact hashes. Logs remain under `Artifacts/logs/` after failures.

`validate_plan.py` verifies the milestone-aware first-task registry, required roadmap files, evidence-gated completion, scoped `codex.md` fallback, and the absence of `AGENTS.md`.

`update_status.py` regenerates `docs/plans/STATUS_BOARD.md` from task contracts and the milestone registry. Do not hand-edit completion state.
