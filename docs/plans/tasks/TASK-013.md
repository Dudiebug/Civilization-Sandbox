# TASK-013 — Hierarchical navigation prototype

**Status:** Not Started  
**Phase:** Phase 1  
**Risk:** Critical  
**Depends on:** TASK-012  
**Evidence folder:** `docs/evidence/TASK-013/`
**Blueprint source:** Section 81, Task 013; Sections 10, 17, 47, 51, 55–57, 66–68, 75.2, 80.2, 81, and technical appendices

## Creator summary
Prove long-range travel will not become per-person fine-grid pathfinding.

## Objective
Route through strategic regions, settlement/road graph, and short local traversal with shared corridors and bounded invalidation.

## In scope
- Coarse region graph and settlement graph prototypes.
- Local entrance/traversal interface.
- Corridor cache keyed by route conditions/mode.
- Edge damage/blockage and bounded cache invalidation.
- Route disruption, detour, and throughput benchmark.

## Required outputs
- [ ] Navigation spike package/scenarios.
- [ ] Route trace and cache telemetry.
- [ ] Go/no-go report for production navigation approach.

## Verification and acceptance
- [ ] Long trips do not run global fine-grid A* per person.
- [ ] Blocked edge creates deterministic detour or explicit no-route.
- [ ] Only affected cache segments invalidate.
- [ ] Thousands of route requests fit budget.
- [ ] Save/load preserves route/corridor semantics.
- [ ] Documentation and relevant `codex.md` instructions match the implementation.
- [ ] Independent adversarial review reports no blocking findings.
- [ ] Creator-visible acceptance is recorded.

## Out of scope
- Crowd steering polish, final road geometry, or rigidbody traffic.

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
