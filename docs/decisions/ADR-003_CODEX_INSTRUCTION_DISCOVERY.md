# ADR-003 - Manual Codex Instruction Routing

**Status:** Accepted
**Date:** 2026-07-20
**Decision owner:** Creator
**Risk tier:** High
**Blueprint compatibility:** Replaces the Blueprint's `AGENTS.md` example with creator-approved repository conventions while preserving durable, scoped instructions.

## Context

The Blueprint recommends automatically discovered repository instructions. Current Codex discovery automatically reads `AGENTS.md`; alternate names such as `codex.md` require a configured fallback. The creator requires both `AGENTS.md` and project `.codex/config.toml` to remain absent for the rest of development.

## Decision

- Keep the root and scoped `codex.md` suite as the repository's durable instruction convention.
- Do not commit `AGENTS.md` or `.codex/config.toml` anywhere in the repository.
- Start-session and task prompts must explicitly tell the agent to read root `codex.md` and the nearest scoped `codex.md` files before acting.
- Documentation must not claim that Codex automatically discovers `codex.md` under default settings.
- Model and reasoning recommendations remain creator-facing guidance, not committed Codex configuration.

## Consequences

- A fresh default Codex session will not automatically discover the `codex.md` suite.
- Reliable operation depends on using the supplied start/task prompts or manually opening the named files.
- The repository remains free of machine/runtime Codex configuration and does not silently change a creator's personal settings.
- Validation can prove that instructions exist, are linked, and are routed by prompts; it cannot claim host-level automatic loading.

## Validation

- Repository validation rejects any case-insensitive `AGENTS.md` and any `.codex/config.toml`.
- Root instructions, the document index, and creator prompts name the manual read sequence.
- A clean session launched from the supplied prompt can locate the active task and scoped instructions.

## Rollback or supersession

Only a later explicit creator decision may permit `AGENTS.md`, project Codex configuration, or another automatic instruction surface. Keep this ADR and mark it Superseded rather than deleting it.

## Creator disclosure

Manual routing is less automatic than Codex's documented default. The creator accepts that tradeoff in exchange for keeping project Codex configuration absent.
