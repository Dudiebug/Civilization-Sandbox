# START HERE

This folder is the repository control system for building **Civilization Sandbox** with Codex. It deliberately contains **no `AGENTS.md` or `.codex/config.toml` file**. Under [ADR-003](docs/decisions/ADR-003_CODEX_INSTRUCTION_DISCOVERY.md), prompts must explicitly route agents to the committed `codex.md` files; automatic discovery is not claimed.

## Roadmap status

The release plan has been rebaselined:
- Version 1.0 is the lean regional game.
- The original Blueprint v2.0 complete early-modern target is Version 1.5.
- Pre-1.0 work uses Milestones 0.1 through 0.9.
- Exact product counts and detailed content choices are intentionally deferred to `docs/plans/DECISION_QUEUE.md`.

## Your next action

1. Read the [source-of-truth index](docs/DOCUMENT_INDEX.md), [revision summary](REVISION_SUMMARY.md), root `codex.md`, and [current step](docs/plans/CURRENT_STEP.md).
2. Run `powershell -NoProfile -File Build/Bootstrap.ps1 -RepositoryOnly` for a read-only repository audit.
3. Work only in the authoritative project at `C:\Users\dudie\Projects\Civilization-Sandbox`; do not create or retain another project checkout or Unity project folder.

## Files to use most often

- [Creator quick reference](docs/creator/QUICK_REFERENCE.md) - one-screen operating guide.
- [Status board](docs/plans/STATUS_BOARD.md) - what is done, active, blocked, or next.
- [Master plan](docs/plans/MASTER_PLAN.md) - the concise milestone route.
- [Pre-1.0 roadmap](docs/plans/PRE_1_0_ROADMAP.md) - detailed gates, evidence, and decision boundaries.
- [Decision queue](docs/plans/DECISION_QUEUE.md) - choices deliberately not made yet.
- [Model and prompt guide](docs/creator/MODEL_AND_PROMPT_GUIDE.md) - model selection and prompting method.

## Safety rule

A box is crossed off only after automated checks, independent review, evidence, and creator-visible acceptance. The planning system is complete; **game-development tasks are not yet complete**.
