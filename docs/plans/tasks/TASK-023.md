# TASK-023 — Physical camp and building-construction proof

**Status:** Not Started  
**Phase:** Phase 3  
**Risk:** Critical  
**Depends on:** TASK-013, TASK-014, TASK-021, TASK-022  
**Evidence folder:** `docs/evidence/TASK-023/`
**Blueprint source:** Section 81, Task 023; Sections 16–27, 47–54, 69–75, 80.4–80.5, 81, and technical appendices

## Creator summary
Make founding visibly physical: people carry material and construct real footprints rather than flipping an abstract settlement flag.

## Objective
Implement camp projects, validated footprints/entrances, material/labor reservations, hauling, construction stages, damage, abandonment, salvage, and ruins.

## In scope
- Project lifecycle from proposal through commissioning.
- Temporary shelter, common stockpile, water/food worksite, and one durable structure.
- Validated site/footprint/entrance constraints.
- Commodity batches, hauling jobs, tool/labor progress, cancellation.
- Visual stages mapped to authoritative stages.
- Damage/repair/collapse/ruin identity and event history.

## Required outputs
- [ ] Construction/project runtime.
- [ ] Minimal building archetypes/modules.
- [ ] Founding camp scenario and presentation.
- [ ] Save/replay/performance evidence.

## Verification and acceptance
- [ ] Every completed structure has attributable deliveries/person-hours/authority.
- [ ] No material or labor teleports.
- [ ] Cancellation releases reservations and leaves coherent partial state/salvage.
- [ ] Save/load preserves stages, footprints, entrances, and ruin IDs.
- [ ] Presentation cannot change progress.
- [ ] Documentation and relevant `codex.md` instructions match the implementation.
- [ ] Independent adversarial review reports no blocking findings.
- [ ] Creator-visible acceptance is recorded.

## Out of scope
- Sixteen full archetypes, final art, interiors, ordinary manual placement, or city-scale planning.

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
