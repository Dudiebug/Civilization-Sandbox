# TASK-011 — Synthetic 10,000-person engine benchmark

**Status:** Not Started  
**Phase:** Phase 1  
**Risk:** Critical  
**Depends on:** TASK-005, TASK-006, TASK-007, TASK-009, TASK-010  
**Evidence folder:** `docs/evidence/TASK-011/`
**Blueprint source:** Section 81, Task 011; Sections 10, 17, 47, 51, 55–57, 66–68, 75.2, 80.2, 81, and technical appendices

## Creator summary
Measure the real cost shape before implementing complex behavior.

## Objective
Run compact synthetic people, households, needs, jobs, locations, and shipments at 10,000-person scale and measure tick, memory, save, and load.

## In scope
- Synthetic data generator with stable seeds.
- Representative component/access patterns without full AI.
- Cadence mixes and structural-change stress.
- Memory, allocations, system duration, backlog, save/load, and trace-off measurements.
- Reference-machine baseline and report.

## Required outputs
- [ ] Benchmark scenarios and runner.
- [ ] Machine-readable and human-readable report.
- [ ] Baseline budget ledger.

## Verification and acceptance
- [ ] 10,000 entities remain below working-set and loop budgets or produce a clear gate failure.
- [ ] Steady-state hot loops allocate zero where required.
- [ ] Save/load meets provisional size/time targets.
- [ ] Repeated runs have stable variance and checksums.
- [ ] Documentation and relevant `codex.md` instructions match the implementation.
- [ ] Independent adversarial review reports no blocking findings.
- [ ] Creator-visible acceptance is recorded.

## Out of scope
- Inflating entity count while omitting representative work, or claiming scalability from one screenshot.

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
