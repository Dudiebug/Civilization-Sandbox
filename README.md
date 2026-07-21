# Civilization Sandbox - Codex Development Kit

This repository turns the authoritative Game Development Blueprint v2.0 into a vibe-first sequence of playable Unity builds. It is optimized for a solo creator directing Codex through detailed prompts, frequent play checks, and reliable rollback points.

## Current release strategy

- **Version 1.0:** lean regional civilization sandbox with a complete causal identity loop.
- **Versions 1.1-1.4:** working early-modern depth releases; exact content is selected at each release gate.
- **Version 1.5:** the original Blueprint v2.0 complete early-modern target.
- **Version 2.0+:** industrial and later possibility spaces.

Start with the [source-of-truth index](docs/DOCUMENT_INDEX.md), [vision authority](docs/plans/VISION_AUTHORITY.md), and [full playable-build roadmap](docs/plans/FULL_ROADMAP.md).

## Included

- Hierarchical `codex.md` instructions, manually routed without `AGENTS.md` or `.codex/config.toml` under [ADR-003](docs/decisions/ADR-003_CODEX_INSTRUCTION_DISCOVERY.md).
- Twenty-one detailed copy-paste build prompts covering foundation, playable Version 1.0, complete early-modern Version 1.5, later ages, divergent futures, and distant expansion.
- A scope-coverage matrix that keeps the complete Blueprint vision visible while work is delivered in playable vertical slices.
- Just-in-time architecture, testing, persistence, performance, and documentation inside the build that needs them.
- Lightweight roadmap validation and durable written memory without hundreds of pre-authored microtasks.
- AI behavior-contract, trace, evaluation, and regression specifications.
- Templates for tasks, milestones, decisions, reviews, evidence, regressions, and creator acceptance.
- Accepted Unity/toolchain baseline with conditional domain-package activation.

## Authority order

1. Blueprint v2.0 for north-star product identity, technical principles, and Version 1.5 completeness.
2. Accepted ADRs, including ADR-001 for release labels/staging, ADR-002 for the toolchain baseline, and ADR-003 for manual instruction routing.
3. The current full roadmap and selected playable build prompt.
4. Current behavior contracts and technical specifications.
5. The active playable build prompt.
6. Code and content.
7. Evidence records.
8. Chat history.

No lower item may silently contradict a higher item.

## Status convention

- `[ ]` Not started
- `[~]` In progress - written as text, not a Markdown checkbox
- `[!]` Blocked
- `[x] ~~Completed item~~` - allowed only after acceptance evidence exists

Run `python Build/validate_docs.py` and `python Build/validate_plan.py` before marking work done. TASK-001 bootstrap commands and their safety contract are documented in [Build/README.md](Build/README.md).
