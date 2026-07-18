# TASK-007 — Keyed deterministic random streams

**Status:** Not Started  
**Phase:** Phase 1  
**Risk:** Critical  
**Depends on:** TASK-005, TASK-006  
**Evidence folder:** `docs/evidence/TASK-007/`
**Blueprint source:** Section 81, Task 007; Sections 10, 17, 47, 51, 55–57, 66–68, 75.2, 80.2, 81, and technical appendices

## Creator summary
Make controlled variation reproducible and independent of execution order.

## Objective
All authoritative randomness is addressed by stable keys and bounded semantics rather than shared mutable generators.

## In scope
- Stream-family catalog.
- World seed, stable ID, decision/event ID, occurrence counter addressing.
- Integer/fixed conversion helpers and bounded distributions.
- Analyzer integration for forbidden RNG.
- Order-independence and collision tests.

## Required outputs
- [ ] Keyed RNG library, catalog, tests, and usage guide.
- [ ] Traceable RNG address format.

## Verification and acceptance
- [ ] Parallel/reordered evaluation yields the same per-key samples.
- [ ] Uncontrolled RNG use fails architecture checks.
- [ ] Save/load resumes occurrence semantics correctly.
- [ ] Random adjustment remains inside contract bounds.
- [ ] Documentation and relevant `codex.md` instructions match the implementation.
- [ ] Independent adversarial review reports no blocking findings.
- [ ] Creator-visible acceptance is recorded.

## Out of scope
- Randomness that grants feasibility, consumes a shared sequence, or depends on render/update order.

## Required work sequence
- [ ] Planner produces small milestones, exact files, tests, rollback, and stop conditions.
- [ ] Creator approves the plan.
- [ ] Implementer completes one milestone at a time.
- [ ] Verification agent reproduces evidence from a clean worktree.
- [ ] Adversarial reviewer challenges architecture and failure cases.
- [ ] Creator tests the observable result and accepts or requests changes.
- [ ] `Build/validate_plan.py` permits Done status.

## Suggested launch
Use `docs/prompts/01_PLAN_TASK.md` with **5.6 Sol / Extra High** for planning. After approval, use **5.6 Terra / High** for ordinary implementation unless the plan identifies a critical architecture or migration change.
