# Civilization Sandbox - Codex Development Kit

This kit converts the authoritative Game Development Blueprint v2.0 into a staged, auditable production repository. It is optimized for a creator who needs short instructions, explicit stop points, small tasks, durable written memory, and decisions made only when evidence makes them necessary.

## Current release strategy

- **Version 1.0:** lean regional civilization sandbox with a complete causal identity loop.
- **Versions 1.1-1.4:** working early-modern depth releases; exact content is selected at each release gate.
- **Version 1.5:** the original Blueprint v2.0 complete early-modern target.
- **Version 2.0+:** industrial and later possibility spaces.

See `REVISION_SUMMARY.md`, `docs/decisions/ADR-001_RELEASE_SCOPE_REBASELINE.md`, `docs/plans/PRE_1_0_ROADMAP.md`, and `docs/plans/RELEASE_LADDER.md`.

## Included

- Hierarchical `codex.md` instructions, without `AGENTS.md`.
- Hierarchical project guidance through committed `codex.md` files; `.codex/config.toml` is intentionally absent.
- A consolidated in-depth pre-1.0 roadmap plus nine milestone plans from repository foundation through release candidate.
- Detailed contracts for the first 25 implementation tasks, covering Milestones 0.1-0.3.
- A decision queue that deliberately postpones product choices until the responsible milestone.
- Evidence-gated status tracking and validation scripts.
- AI behavior-contract, trace, evaluation, and regression specifications.
- Templates for tasks, milestones, decisions, reviews, evidence, regressions, and creator acceptance.
- Conditional engine/package boundaries that are not considered accepted until Milestone 0.1 resolves the engine path.

## Authority order

1. Blueprint v2.0 for north-star product identity, technical principles, and Version 1.5 completeness.
2. Accepted ADRs, including ADR-001 for release labels and staging.
3. Current approved release and milestone plans.
4. Current behavior contracts and technical specifications.
5. Current approved task contract.
6. Code and content.
7. Chat history.

No lower item may silently contradict a higher item.

## Status convention

- `[ ]` Not started
- `[~]` In progress - written as text, not a Markdown checkbox
- `[!]` Blocked
- `[x] ~~Completed item~~` - allowed only after acceptance evidence exists

Run `python Build/validate_plan.py` before marking work done. TASK-001 bootstrap commands and their safety contract are documented in `Build/README.md`.
