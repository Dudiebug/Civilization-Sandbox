# TASK-008 — Historical event graph foundation

**Status:** Not Started  
**Phase:** Phase 1  
**Risk:** High  
**Depends on:** TASK-006, TASK-007  
**Evidence folder:** `docs/evidence/TASK-008/`
**Blueprint source:** Section 81, Task 008; Sections 10, 17, 47, 51, 55–57, 66–68, 75.2, 80.2, 81, and technical appendices

## Creator summary
Record causal history before complex systems produce events that cannot later be reconstructed.

## Objective
Persist stable factual event nodes, typed causal edges, participants, locations, significance, perspective hooks, and intervention ancestry.

## In scope
- Event/edge stable IDs and schemas.
- Typed edges such as caused-by, contributed-to, damaged, displaced, rebuilt-from, and intervention ancestry.
- Entity/location indexes with query caps.
- Significance and retention fields.
- Semantic save integration and integrity validation.

## Required outputs
- [ ] Event graph package, DTOs, query API, tests, and sample founding/command events.

## Verification and acceptance
- [ ] Round-trip preserves nodes, edges, and references.
- [ ] Queries enforce depth/time/result caps.
- [ ] Compaction cannot remove referenced canonical nodes.
- [ ] Broken causal/entity references fail validation.
- [ ] Documentation and relevant `codex.md` instructions match the implementation.
- [ ] Independent adversarial review reports no blocking findings.
- [ ] Creator-visible acceptance is recorded.

## Out of scope
- Free-form generated prose, unbounded per-entity event lists, or story systems that own outcomes.

## Required work sequence
- [ ] Planner produces small milestones, exact files, tests, rollback, and stop conditions.
- [ ] Creator approves the plan.
- [ ] Implementer completes one milestone at a time.
- [ ] Verification agent reproduces evidence from a clean worktree.
- [ ] Adversarial reviewer challenges architecture and failure cases.
- [ ] Creator tests the observable result and accepts or requests changes.
- [ ] `Build/validate_plan.py` permits Done status.

## Suggested launch
Use `docs/prompts/01_PLAN_TASK.md` with **5.6 Sol / Extra High** for High-risk planning. After approval, use **5.6 Terra / High** for ordinary implementation and **Terra / Medium** only for exact mechanical work.
