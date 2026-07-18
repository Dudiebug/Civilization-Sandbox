# TASK-021 — Procedural world generation and validation

**Status:** Not Started  
**Phase:** Phase 3  
**Risk:** High  
**Depends on:** TASK-008, TASK-010, TASK-012, TASK-018  
**Evidence folder:** `docs/evidence/TASK-021/`
**Blueprint source:** Section 81, Task 021; Sections 16–27, 47–54, 69–75, 80.4–80.5, 81, and technical appendices

## Creator summary
Create bounded worlds with enough physical and informational diversity for contrasting societies.

## Objective
Generate versioned terrain, hydrology, climate, soils, ecosystems/resources, hidden deposits, hazards, and viable contrasting founding regions.

## In scope
- 128-bit seed and generation version.
- Elevation, drainage, watersheds, rivers/lakes/coasts.
- Temperature, moisture, seasonality, soil, vegetation productivity, habitat, surface resources.
- Hidden deposits with quantity/grade/depth/access cost.
- Flood/fire/drought/storm/slope/disease pressure.
- Validity report and deterministic regeneration.

## Required outputs
- [ ] World-generation package and content.
- [ ] Small/Standard/Large generation presets.
- [ ] World validator and seed suite.
- [ ] Generation manifest and save integration.

## Verification and acceptance
- [ ] Standard valid seeds meet minimum river/watershed/region/site/corridor/diversity rules.
- [ ] Same seed/version reproduces authoritative fields.
- [ ] AI site choice sees only knowledge.
- [ ] Invalid structurally unusable worlds are rejected with reasons.
- [ ] Generation and save fit budgets.
- [ ] Documentation and relevant `codex.md` instructions match the implementation.
- [ ] Independent adversarial review reports no blocking findings.
- [ ] Creator-visible acceptance is recorded.

## Out of scope
- Perfect real-world climatology, spherical worlds, final terrain art, or eliminating all difficult sites.

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
