# TASK-020 - Settlement labor-allocation contract

**Status:** Not Started
**Milestone:** 0.2 - Decision and Engine Proof
**Release horizon:** Lean Version 1.0; complete Blueprint early-modern breadth is Version 1.5
**Risk:** Critical
**Depends on:** TASK-015, TASK-016, TASK-017, TASK-019
**Decision dependencies:** None unless the approved task plan identifies an active decision gate
**Evidence folder:** `docs/evidence/TASK-020/`
**Blueprint source:** Section 81, Task 020; Sections 58–64, 67–68, 75.3, 80.3, 81, and Appendix A/B/G

## Creator summary
Prove a higher layer can allocate priorities without directly commanding every person.

## Objective
Implement bounded settlement/workplace labor demand, eligibility, skills, tools, travel, priorities, emergency overrides, and stable matching/distribution tests.

## In scope
- Demand and role definitions.
- Prefiltered eligible worker cohorts and hard caps.
- Settlement priority/authority inputs and household/person commitments.
- Travel, qualification, compensation/entitlement, safety, and scarcity considerations.
- Reservation, vacancy, reassignment inertia, and emergency override.

## Required outputs
- [ ] AI-LABOR-001 contract/implementation/fixtures.
- [ ] Allocation trace, conservation, fairness/pathology, distribution, and performance reports.

## Verification and acceptance
- [ ] No settlement issues personal movement commands.
- [ ] Unavailable tools/skills/routes remain blockers.
- [ ] Increasing valid harvest priority shifts labor directionally without teleportation.
- [ ] Stable conditions converge without oscillation.
- [ ] Batch fits declared settlement review budget.
- [ ] Documentation and relevant `codex.md` instructions match the implementation.
- [ ] Independent adversarial review reports no blocking findings.
- [ ] Creator-visible acceptance is recorded.

## Out of scope
- Full labor market, wages/credit depth, production recipes, or settlement planning.

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
