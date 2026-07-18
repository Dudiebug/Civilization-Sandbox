# TASK-006 — Stable identity registry

**Status:** Not Started  
**Phase:** Phase 1  
**Risk:** Critical  
**Depends on:** TASK-005  
**Evidence folder:** `docs/evidence/TASK-006/`
**Blueprint source:** Section 81, Task 006; Sections 10, 17, 47, 51, 55–57, 66–68, 75.2, 80.2, 81, and technical appendices

## Creator summary
Give every durable world object a nonreused identity independent of Unity ECS handles.

## Objective
Create typed stable IDs, reference resolution, tombstones, and migration fixtures that survive entity rebuild and long history.

## In scope
- World identity plus monotonic local identity or equivalent.
- Typed IDs/references for initial domain families.
- Registry/index ownership and lifecycle.
- Tombstone semantics and nonreuse tests.
- Serialization/migration representation.

## Required outputs
- [ ] Stable ID library and registry.
- [ ] Reference-validation utilities.
- [ ] Tombstone fixtures.
- [ ] ID diagnostics and tests.

## Verification and acceptance
- [ ] IDs never reuse after deletion/death.
- [ ] ECS reconstruction resolves the same domain references.
- [ ] Dangling/incorrect-type references fail validation.
- [ ] Round-trip preserves identity and tombstones.
- [ ] Documentation and relevant `codex.md` instructions match the implementation.
- [ ] Independent adversarial review reports no blocking findings.
- [ ] Creator-visible acceptance is recorded.

## Out of scope
- Using ECS entity indices, object instance IDs, names, or array positions as authoritative identity.

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
