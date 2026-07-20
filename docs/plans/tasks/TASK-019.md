# TASK-019 - Urgent individual-needs contract

**Status:** Not Started
**Milestone:** 0.2 - Decision and Engine Proof
**Release horizon:** Lean Version 1.0; complete Blueprint early-modern breadth is Version 1.5
**Risk:** High
**Depends on:** TASK-015, TASK-016, TASK-017
**Decision dependencies:** None unless the approved task plan identifies an active decision gate
**Evidence folder:** `docs/evidence/TASK-019/`
**Blueprint source:** Section 81, Task 019; Sections 58–64, 67–68, 75.3, 80.3, 81, and Appendix A/B/G

## Creator summary
Prove personal emergency action is feasible, reserved, explainable, and does not scan or cheat.

## Objective
Implement bounded emergency food, shelter, water, and care choices with reachability, reservations, current commitments, and failure handling.

## In scope
- Urgency trigger and role/state candidate set.
- Knowledge-filtered reachable sources.
- Preconditions for access, capacity, law, and reservations.
- Commitment interruption and critical-emergency override.
- No-target, stale-target, double-reservation, save/replay, and performance scenarios.

## Required outputs
- [ ] AI-NEED-001 contract/implementation/fixtures.
- [ ] Trace and regression report.

## Verification and acceptance
- [ ] A person cannot consume/reserve the same entitlement as another.
- [ ] Destroyed or unreachable targets invalidate cleanly.
- [ ] Greater hunger increases urgency without creating food.
- [ ] No unexplained idle loop when a valid emergency action exists.
- [ ] Documentation and relevant `codex.md` instructions match the implementation.
- [ ] Independent adversarial review reports no blocking findings.
- [ ] Creator-visible acceptance is recorded.

## Out of scope
- Full health/demography, social behavior, or global search.

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
