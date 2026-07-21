# TASK-021 - Regional world generation and validation

**Status:** Not Started
**Milestone:** 0.3 - Living Camp
**Release horizon:** One regional Living Camp envelope now; broader presets and full environmental breadth later
**Risk:** High
**Depends on:** TASK-008, TASK-010, TASK-012, TASK-018
**Decision dependencies:** D04
**Evidence folder:** `docs/evidence/TASK-021/`
**Blueprint/ADR source:** Blueprint Sections 16, 25, 69-75, 81; ADR-001

## Creator summary
Create only the environmental and informational variation needed to test a living founding camp. Do not build the full Small/Standard/Large world suite yet.

## Objective
Generate a versioned bounded region with terrain, water, fertility/productivity, surface resources, limited hidden information, route constraints, and a small hazard set sufficient for site choice and survival.

## In scope
- Seed and generation version.
- Elevation, drainage/water, basic moisture/soil/productivity, vegetation/resource fields.
- Only the deposits/hazards needed by the approved Living Camp slice.
- Viable and difficult founding areas.
- Deterministic regeneration, validity report, and save integration.

## Required outputs
- [ ] Regional world-generation package/content.
- [ ] One approved generation envelope plus test variants.
- [ ] Validator and seed suite.
- [ ] Generation manifest and save fixture.

## Verification and acceptance
- [ ] Valid seeds meet the D04 survival/diversity requirements.
- [ ] Same seed/version reproduces authoritative fields.
- [ ] Founding AI sees only permitted knowledge.
- [ ] Structurally unusable worlds are rejected with reasons.
- [ ] Generation and save fit the accepted Version 1.0 target tier.
- [ ] Independent review and creator acceptance pass.

## Out of scope
Multiple retail presets, full climate/ecosystem simulation, perfect real-world geography, spherical worlds, or final terrain art.
