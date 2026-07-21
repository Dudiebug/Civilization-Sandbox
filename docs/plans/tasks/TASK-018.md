# TASK-018 - Founding-group site-selection contract

**Status:** Not Started
**Milestone:** 0.2 - Decision and Engine Proof
**Release horizon:** Lean Version 1.0; complete Blueprint early-modern breadth is Version 1.5
**Risk:** High
**Depends on:** TASK-012, TASK-015, TASK-016, TASK-017
**Decision dependencies:** None unless the approved task plan identifies an active decision gate
**Evidence folder:** `docs/evidence/TASK-018/`
**Blueprint source:** Section 81, Task 018; Sections 58–64, 67–68, 75.3, 80.3, 81, and Appendix A/B/G

## Creator summary
Prove a group can choose a plausible site using only known, bounded evidence and controlled variation.

## Objective
Implement knowledge-limited site candidates, survival considerations, hazards/claims, stable selection, trace, and scenario suites.

## In scope
- Candidate-region provider with hard cap.
- Water, food, arable land, timber, shelter, route, security, opportunity, preference, hazard, and claim considerations.
- Knowledge confidence/staleness and group-profile data.
- Invalidation when site becomes impossible.
- Positive, negative, boundary, metamorphic, multi-seed, save/replay, and performance fixtures.

## Required outputs
- [ ] AI-SITE-001 contract/implementation/fixtures.
- [ ] Trace explanation and distribution report.

## Verification and acceptance
- [ ] Groups never use concealed deposits or unknown hazards as truth.
- [ ] Improved known water does not reduce site utility absent another modeled cause.
- [ ] No-valid-site case is explicit.
- [ ] Same seed/checkpoint replays exactly; different keyed variation remains bounded.
- [ ] Documentation and relevant `codex.md` instructions match the implementation.
- [ ] Independent adversarial review reports no blocking findings.
- [ ] Creator-visible acceptance is recorded.

## Out of scope
- World generation itself, settlement construction, or exhaustive search of every map cell.

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
