# START HERE

This folder is the repository control system for building **Civilization Sandbox** with Codex. It deliberately contains **no `AGENTS.md` or `.codex/config.toml` file**. Under [ADR-003](docs/decisions/ADR-003_CODEX_INSTRUCTION_DISCOVERY.md), prompts must explicitly route agents to the committed `codex.md` files; automatic discovery is not claimed.

## Roadmap status

The release plan has been rebaselined:
- Version 1.0 is the lean regional game.
- The original Blueprint v2.0 complete early-modern target is Version 1.5.
- Work is organized as 21 playable builds rather than hundreds of microtasks.
- The complete Blueprint scope remains visible in `docs/plans/SCOPE_COVERAGE.md`.

## Your next action

1. Read the [source-of-truth index](docs/DOCUMENT_INDEX.md), [vision authority](docs/plans/VISION_AUTHORITY.md), root `codex.md`, and the next selected file in `docs/plans/tasks/`.
2. Run `powershell -NoProfile -File Build/Bootstrap.ps1 -RepositoryOnly` for a read-only repository audit.
3. Work only in the authoritative project at `C:\Users\dudie\Projects\Civilization-Sandbox`; do not create or retain another project checkout or Unity project folder.

## Files to use most often

- [Creator quick reference](docs/creator/QUICK_REFERENCE.md) - one-screen operating guide.
- [Full roadmap](docs/plans/FULL_ROADMAP.md) - the complete route from foundation to distant expansion.
- [Playable build prompts](docs/plans/tasks/README.md) - direct prompts to use one build at a time.
- [Scope coverage](docs/plans/SCOPE_COVERAGE.md) - where every major vision area is preserved.
- [Vibe workflow](docs/plans/VIBE_WORKFLOW.md) - prompt, implement, run, judge, commit, or revert.
- [Model and prompt guide](docs/creator/MODEL_AND_PROMPT_GUIDE.md) - model selection and prompting method.

## Safety rule

Do not ask Codex to implement all 21 prompts at once. Select one build, let Codex split it into 3-8 playable slices, and judge each slice in Unity before continuing.
