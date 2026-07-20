# Planning-Control Commands

From the repository root:

```powershell
pwsh ./Build/Validate-Plan.ps1
pwsh ./Build/Update-Status.ps1
```

Cross-platform equivalent:

```bash
python Build/validate_plan.py
python Build/update_status.py
```

`validate_plan.py` verifies the milestone-aware first-task registry, required roadmap files, evidence-gated completion, scoped `codex.md` fallback, and the absence of `AGENTS.md`.

`update_status.py` regenerates `docs/plans/STATUS_BOARD.md` from task contracts and the milestone registry. Do not hand-edit completion state.
