# TASK-024 — Paths, road edges, parcels, and project-order boundary

**Status:** Not Started  
**Phase:** Phase 3  
**Risk:** Critical  
**Depends on:** TASK-013, TASK-020, TASK-023  
**Evidence folder:** `docs/evidence/TASK-024/`
**Blueprint source:** Section 81, Task 024; Sections 16–27, 47–54, 69–75, 80.4–80.5, 81, and technical appendices

## Creator summary
Connect movement to durable infrastructure and prove the player can order an objective without drawing the exact road.

## Objective
Turn repeated travel into path pressure, formal bounded route proposals, authoritative road edges, parcels/rights, and decision-level player orders.

## In scope
- Decaying travel heat by movement class and desire-path candidates.
- Project actors and bounded route candidates.
- Road edge geometry/state: capacity, condition, owner, maintainer, access, lineage.
- Parcel derivation and basic land-right bundles.
- Binding road/housing project order boundary.
- Damage/blockage, detour, maintenance, abandonment, and history.

## Required outputs
- [ ] Path/road/parcel packages and scenarios.
- [ ] Order validation/progress read model.
- [ ] Route/land-use state and trace evidence.

## Verification and acceptance
- [ ] Travel creates a candidate, not an instant completed road.
- [ ] Player normal order selects objective/region while simulated authority chooses route/site.
- [ ] Blocked road causes bounded detour/shortage pressure.
- [ ] Network edit invalidates only affected caches.
- [ ] Save/load preserves route/project/rights state.
- [ ] Documentation and relevant `codex.md` instructions match the implementation.
- [ ] Independent adversarial review reports no blocking findings.
- [ ] Creator-visible acceptance is recorded.

## Out of scope
- Manual road drawing as normal control, full land market, bridges/ports breadth, or decorative roads without authoritative capacity.

## Required work sequence
- [ ] Planner produces small milestones, exact files, tests, rollback, and stop conditions.
- [ ] Creator approves the plan.
- [ ] Implementer completes one milestone at a time.
- [ ] Verification agent reproduces evidence from a clean worktree.
- [ ] Adversarial reviewer challenges architecture and failure cases.
- [ ] Creator tests the observable result and accepts or requests changes.
- [ ] `Build/validate_plan.py` permits Done status.

## Suggested launch
Use `docs/prompts/01_PLAN_TASK.md` with **5.6 Sol / Max** for Critical planning. After approval, use **5.6 Terra / High** for ordinary implementation and **Terra / Medium** only for exact mechanical work.
