# Status Board

**Generated:** 2026-07-21 03:21 UTC
**Roadmap revision:** ADR-001-lean-1.0
**Active implementation task:** TASK-002
**Completed implementation tasks:** 1 / 25
**Current milestone:** 0.1

## Repository-control setup
- [x] ~~Blueprint copied into the repository kit unchanged.~~
- [x] ~~Scoped `codex.md` suite created; no `AGENTS.md` file.~~
- [x] ~~Evidence-gated task, review, prompt, and status controls created.~~
- [x] ~~Release plan rebaselined: lean Version 1.0, original complete early-modern target at Version 1.5.~~
- [x] ~~Milestone-timed decision queue created to avoid premature product choices.~~

## Pre-1.0 milestone summary
| Milestone | Working name | Done | Active | Review | Blocked | Contracts | Contract state |
|---:|---|---:|---:|---:|---:|---:|---|
| 0.1 | Project Foundation | 1 | 0 | 1 | 0 | 10 | Active |
| 0.2 | Decision and Engine Proof | 0 | 0 | 0 | 0 | 10 | Defined; not started |
| 0.3 | Living Camp | 0 | 0 | 0 | 0 | 5 | Defined; not started |
| 0.4 | Physical Settlement | 0 | 0 | 0 | 0 | 0 | Deferred until prior gate |
| 0.5 | Divergent Societies | 0 | 0 | 0 | 0 | 0 | Deferred until prior gate |
| 0.6 | Connected Region | 0 | 0 | 0 | 0 | 0 | Deferred until prior gate |
| 0.7 | Conflict and Recovery | 0 | 0 | 0 | 0 | 0 | Deferred until prior gate |
| 0.8 | God Sandbox | 0 | 0 | 0 | 0 | 0 | Deferred until prior gate |
| 0.9 | Release Candidate | 0 | 0 | 0 | 0 | 0 | Deferred until prior gate |

## Currently defined implementation contracts
The first 25 contracts cover Milestones 0.1-0.3. Milestones 0.4-0.9 receive detailed contracts only after the prior gate and required creator decisions pass.

- [x] ~~**TASK-001** — Repository governance and reproducible bootstrap~~ — Milestone 0.1
- [?] **TASK-002** — Source-of-truth, release-scope, story, and AI document set — **IN REVIEW** — Milestone 0.1
- [ ] **TASK-003** — Architecture and code-quality gates — Milestone 0.1
- [ ] **TASK-004** — Command-line and CI harness — Milestone 0.1
- [ ] **TASK-005** — Authoritative world clock and scheduler shell — Milestone 0.1
- [ ] **TASK-006** — Stable identity registry — Milestone 0.1
- [ ] **TASK-007** — Keyed deterministic random streams — Milestone 0.1
- [ ] **TASK-008** — Causal event record foundation — Milestone 0.1
- [ ] **TASK-009** — Canonical checksum and state diff — Milestone 0.1
- [ ] **TASK-010** — Semantic persistence skeleton — Milestone 0.1
- [ ] **TASK-011** — Measured scale ladder and engine benchmark — Milestone 0.2
- [ ] **TASK-012** — Spatial chunks and bounded queries — Milestone 0.2
- [ ] **TASK-013** — Hierarchical navigation prototype — Milestone 0.2
- [ ] **TASK-014** — Configurable 2.5D presentation proof — Milestone 0.2
- [ ] **TASK-015** — AI Behavior Contract schema and decision catalog — Milestone 0.2
- [ ] **TASK-016** — Canonical AI primitives — Milestone 0.2
- [ ] **TASK-017** — AI Decision Laboratory runner — Milestone 0.2
- [ ] **TASK-018** — Founding-group site-selection contract — Milestone 0.2
- [ ] **TASK-019** — Urgent individual-needs contract — Milestone 0.2
- [ ] **TASK-020** — Settlement labor-allocation contract — Milestone 0.2
- [ ] **TASK-021** — Regional world generation and validation — Milestone 0.3
- [ ] **TASK-022** — Lean persistent people and households — Milestone 0.3
- [ ] **TASK-023** — Physical camp and building-construction proof — Milestone 0.3
- [ ] **TASK-024** — Paths, road edges, parcels, and project-order boundary — Milestone 0.3
- [ ] **TASK-025** — Integrated Living Camp gate — Milestone 0.3

## Status notation
- `[ ]` Not started
- `[~]` In progress
- `[?]` In review
- `[!]` Blocked
- `[x] ~~Done~~` only after evidence, independent review, and creator acceptance

## Rule
Run `python Build/validate_plan.py` before committing any Done status. A milestone is not complete merely because all of its task contracts are Done; its integrated gate review must also be approved.
