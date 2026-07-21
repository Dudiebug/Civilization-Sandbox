# TASK-012 - Spatial chunks and bounded queries

**Status:** Not Started
**Milestone:** 0.2 - Decision and Engine Proof
**Release horizon:** Lean Version 1.0; complete Blueprint early-modern breadth is Version 1.5
**Risk:** Critical
**Depends on:** TASK-005, TASK-006, TASK-011
**Decision dependencies:** None unless the approved task plan identifies an active decision gate
**Evidence folder:** `docs/evidence/TASK-012/`
**Blueprint source:** Section 81, Task 012; Sections 10, 17, 47, 51, 55–57, 66–68, 75.2, 80.2, 81, and technical appendices

## Creator summary
Create deterministic spatial ownership and hard limits before every system invents its own scans.

## Objective
Authoritative coordinates, 256 m region chunks, indexes, bounded query APIs, deterministic iteration, and cache rebuilds.

## In scope
- Typed world/local coordinates and units.
- Chunk ownership and border rules.
- Point/radius/region/category queries with caps and truncation telemetry.
- Stable iteration/order and deterministic rebuild.
- Save/load reconstruction and benchmark.

## Required outputs
- [ ] Spatial package, query contracts, validators, tests, and benchmark.

## Verification and acceptance
- [ ] Queries never exceed declared caps.
- [ ] Same state yields same ordered result set.
- [ ] Chunk moves update indexes without stale duplicates.
- [ ] Camera/view does not change index membership.
- [ ] Large benchmark remains within budget.
- [ ] Documentation and relevant `codex.md` instructions match the implementation.
- [ ] Independent adversarial review reports no blocking findings.
- [ ] Creator-visible acceptance is recorded.

## Out of scope
- World scans, hidden unbounded LINQ/allocations, or presentation-owned spatial truth.

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
