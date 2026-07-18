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

`validate_plan.py` prevents a task from being Done without dependencies, required evidence, independent review, and creator approval. It also verifies the `codex.md` fallback and the absence of `AGENTS.md`.
