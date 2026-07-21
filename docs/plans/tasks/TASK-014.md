# TASK-014 - Configurable 2.5D presentation proof

**Status:** Not Started
**Milestone:** 0.2 - Decision and Engine Proof
**Release horizon:** Prove the locked visual identity at the accepted Version 1.0 tier; retain Version 1.5 stress data separately
**Risk:** High
**Depends on:** TASK-011, TASK-012, TASK-013
**Decision dependencies:** Uses D03 scale tiers; does not set final art direction
**Evidence folder:** `docs/evidence/TASK-014/`
**Blueprint/ADR source:** Blueprint Sections 41-44, 55-57, 68, 81; ADR-001

## Creator summary
Prove the 2.5D visual stack can display authoritative snapshots readably at configurable density without making the original 5,000-billboard/1,000-building stress target a Version 1.0 release requirement.

## Objective
Display real 3D terrain/elevation/water, angled camera, selection, camera-facing billboards, modular structures, occlusion, LOD, and debug overlays at the tiers defined by TASK-011.

## In scope
- Orthographic/near-orthographic camera and zoom bands.
- Read-only simulation snapshot bridge.
- Configurable billboard and structure density tiers.
- Direction/action cues sufficient for later work, not final animation.
- Selection, outlines, roof/foreground fading, basic overlays, and CPU/GPU telemetry.

## Required outputs
- [ ] Presentation stress scene with scale presets.
- [ ] Snapshot contract and visual descriptor mapping.
- [ ] Performance, occlusion, and readability report for the accepted 1.0 tier and optional 1.5 horizon tier.

## Verification and acceptance
- [ ] Camera movement/visibility cannot alter authoritative checksum.
- [ ] Accepted Version 1.0 tier fits its presentation budget.
- [ ] Selected entities remain readable across occlusion and zoom.
- [ ] Movement/work/danger direction is semantically legible in prototype form.
- [ ] Missing Version 1.5 stress targets are reported, not hidden or treated as a 1.0 failure.
- [ ] Independent review and creator acceptance pass.

## Out of scope
Final art, final animation library, full UI, or simulation logic in presentation objects.
