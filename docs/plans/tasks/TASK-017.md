# TASK-017 — AI Decision Laboratory runner

**Status:** Not Started  
**Phase:** Phase 2  
**Risk:** Critical  
**Depends on:** TASK-009, TASK-010, TASK-015, TASK-016  
**Evidence folder:** `docs/evidence/TASK-017/`
**Blueprint source:** Section 81, Task 017; Sections 58–64, 67–68, 75.3, 80.3, 81, and Appendix A/B/G

## Creator summary
Create the reproducible lab that can challenge behavior before it enters the world simulation.

## Objective
Load fixtures, run scenarios/properties/metamorphic/multi-seed suites, compare traces/state/distributions, and measure per-decision performance.

## In scope
- Fixture schema/loader and headless batch runner.
- Golden and invariant assertions.
- Metamorphic transformation definitions.
- Multi-seed aggregation and envelope checks.
- Before/after trace/state/distribution reports.
- Save/load-at-decision and deliberate-defect modes.

## Required outputs
- [ ] Decision Lab CLI and reports.
- [ ] Sample fixtures and CI integration.
- [ ] Defect-injection mechanism.

## Verification and acceptance
- [ ] A known behavioral change produces a focused comparison.
- [ ] Save/load mid-commitment preserves later selection.
- [ ] At least one seeded defect in each primitive family is detected.
- [ ] Batch performance reports p50/p95/max and allocations.
- [ ] Documentation and relevant `codex.md` instructions match the implementation.
- [ ] Independent adversarial review reports no blocking findings.
- [ ] Creator-visible acceptance is recorded.

## Out of scope
- Tuning gameplay content or accepting “looks smart” as evidence.

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
