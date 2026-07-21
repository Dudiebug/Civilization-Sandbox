# Master Development Plan

**Program target:** Version 1.0 - a lean regional civilization sandbox that preserves the full identity.
**North-star early-modern target:** Version 1.5, matching the original Blueprint v2.0 Version 1.0 breadth.
**Current program status:** roadmap/control kit revised; implementation not started.

## Program controls
- [x] ~~Authoritative Blueprint v2.0 stored in the repository kit.~~
- [x] ~~Scoped `codex.md` instruction system created without `AGENTS.md`.~~
- [x] ~~Evidence-gated task, review, prompt, and status templates created.~~
- [x] ~~Release scope rebaselined through accepted ADR-001.~~
- [x] ~~Milestone-timed decision queue created.~~
- [ ] Complete TASK-001 and establish the real repository/toolchain baseline.

## Pre-1.0 milestone sequence

| Milestone | Working name | Player-visible result | Task-contract state |
|---|---|---|---|
| 0.1 | Project Foundation | Reproducible deterministic project shell; no gameplay required | TASK-001 through TASK-010 defined |
| 0.2 | Decision and Engine Proof | Measured scale/presentation path and verified decision runtime | TASK-011 through TASK-020 defined |
| 0.3 | Living Camp | Named people survive or fail while physically founding a camp | TASK-021 through TASK-025 defined |
| 0.4 | Physical Settlement | A camp autonomously becomes a durable settlement | Candidate work packages only |
| 0.5 | Divergent Societies | Multiple societies visibly develop differently | Candidate work packages only |
| 0.6 | Connected Region | Trade, route failure, shortage, and migration connect societies | Candidate work packages only |
| 0.7 | Conflict and Recovery | Physical conflict causes death, displacement, peace, and rebuilding | Candidate work packages only |
| 0.8 | God Sandbox | Omniscience, orders, interventions, force powers, and history form the player loop | Candidate work packages only |
| 0.9 | Release Candidate | Integrated, accessible, stable, supported build | Candidate work packages only |
| 1.0 | Founding Worlds | Complete lean regional release | Gate after 0.9 |

## Gate rule
A later milestone begins only after the prior milestone's gate evidence and creator decision are accepted. Task contracts for Milestones 0.4-0.9 are intentionally not fully authored now; they are created from measured evidence immediately before that milestone.

## Decision rule
Only decisions required by the active task or next milestone may be opened. `DECISION_QUEUE.md` controls timing. Planning agents stop rather than filling unknowns with assumptions.

## Identity rule
Every milestone must preserve or advance `IDENTITY_GUARDRAILS.md`. Scope is reduced through fewer systems and smaller counts, not by replacing physical causality, divergence, autonomy, omniscience, layered powers, or durable consequences.

## Release rule
Do not tag Version 1.0 until the approved release-scope manifest, identity review, integrated tests, save/recovery, performance envelope, accessibility baseline, and creator acceptance pass. The Blueprint's full Version 1.5 targets do not block Version 1.0 unless promoted by an explicit creator decision.

## Detailed roadmap
The complete pre-1.0 workstream, gate, evidence, decision, and handoff plan is in `PRE_1_0_ROADMAP.md`.

## Where detail lives
- Milestone plans: `PHASE_0.md` through `PHASE_8.md` (legacy filenames retained for tooling compatibility)
- First 25 contracts: `tasks/TASK-001.md` through `docs/plans/tasks/TASK-025.md`
- Release ladder: `RELEASE_LADDER.md`
- Deferred decisions: `DECISION_QUEUE.md`
- Version 1.0 coverage: `VERSION_1_PRODUCTION.md`
- Version 1.5 horizon: `VERSION_1_5_PRODUCTION.md`
- Current status: `STATUS_BOARD.md`
