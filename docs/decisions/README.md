# Architecture and Product Decision Records

Create ADRs from `docs/templates/ADR_TEMPLATE.md`. Number them monotonically. Keep superseded records; link the replacement rather than deleting history.

## Accepted records
- [ADR-001 - Release-scope rebaseline](ADR-001_RELEASE_SCOPE_REBASELINE.md) - Version 1.0 becomes the lean regional release; the original complete early-modern target moves to Version 1.5.
- [ADR-002 - Unity and toolchain baseline](ADR-002_UNITY_TOOLCHAIN_BASELINE.md) - accepts the pinned TASK-001 development baseline while preserving later measured gates.
- [ADR-003 - Manual Codex instruction routing](ADR-003_CODEX_INSTRUCTION_DISCOVERY.md) - keeps `AGENTS.md` and `.codex/config.toml` absent and requires explicit prompt routing to `codex.md`.
