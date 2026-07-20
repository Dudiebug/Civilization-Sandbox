# TASK-014 — 2.5D presentation proof

**Status:** Not Started  
**Phase:** Phase 1  
**Risk:** High  
**Depends on:** TASK-011, TASK-012, TASK-013  
**Evidence folder:** `docs/evidence/TASK-014/`
**Blueprint source:** Section 81, Task 014; Sections 10, 17, 47, 51, 55–57, 66–68, 75.2, 80.2, 81, and technical appendices

## Creator summary
Prove the locked visual direction and density can render authoritative snapshots without affecting simulation.

## Objective
Display real 3D terrain/elevation/water, angled camera, selection, camera-facing billboards, modular structures, occlusion, LOD, and debug overlays at stress scale.

## In scope
- Orthographic/near-orthographic camera and zoom bands.
- Read-only simulation snapshot bridge.
- 5,000 moving billboards with direction/action cues.
- 1,000 modular building instances and LOD.
- Selection, outlines, roof/foreground fading, basic overlays, and CPU/GPU telemetry.

## Required outputs
- [ ] Presentation stress scene.
- [ ] Snapshot contract and visual descriptor mapping.
- [ ] Performance/occlusion/readability report and screenshots/video.

## Verification and acceptance
- [ ] Camera movement/visibility cannot alter authoritative checksum.
- [ ] Stress scene fits provisional presentation budget.
- [ ] Selected entities remain readable across occlusion/zoom.
- [ ] Billboard movement/work/combat direction is semantically legible in prototype.
- [ ] Documentation and relevant `codex.md` instructions match the implementation.
- [ ] Independent adversarial review reports no blocking findings.
- [ ] Creator-visible acceptance is recorded.

## Out of scope
- Final art, final animation library, full UI, or simulation logic in GameObjects/render systems.

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
