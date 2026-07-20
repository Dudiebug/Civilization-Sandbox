# Civilization Sandbox — Codex Development Kit

This kit converts the authoritative Game Development Blueprint v2.0 into a staged, auditable production repository. It is optimized for a creator who needs short instructions, explicit stop points, small tasks, and durable written memory.

## Included

- Hierarchical `codex.md` instructions, without `AGENTS.md`.
- Hierarchical project guidance through committed `codex.md` files; project-local Codex configuration is deferred to TASK-002.
- Eight phase plans from repository bootstrap through first-playable stabilization.
- Detailed contracts for the blueprint's first 25 implementation tasks.
- Evidence-gated status tracking and validation scripts.
- Model-selection and prompt guides.
- AI behavior-contract, trace, evaluation, and regression specifications.
- Templates for tasks, decisions, reviews, evidence, regressions, and creator acceptance.
- Placeholder Unity package boundaries with scoped Codex instructions.

## Authoritative order

1. `docs/blueprint/Civilization_Sandbox_Game_Development_Blueprint_v2.0.pdf`
2. Approved ADRs in `docs/decisions/`
3. Current behavior contracts and technical specifications
4. Current approved task contract
5. Code and content
6. Chat history

No lower item may silently contradict a higher item.

## Status convention

- `[ ]` Not started
- `[~]` In progress — written as text, not a Markdown checkbox
- `[!]` Blocked
- `[x] ~~Completed item~~` — allowed only after acceptance evidence exists

Run `python Build/validate_plan.py` before marking work done. TASK-001 bootstrap commands and their safety contract are documented in `Build/README.md`.
