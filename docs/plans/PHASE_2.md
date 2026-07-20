# Milestone 0.3 - Living Camp

**Status:** Not Started
**Task range:** TASK-021 through TASK-025

**Detailed program context:** `PRE_1_0_ROADMAP.md`, Section 7.

## Purpose
Produce the first recognizable causal simulation: named people survive or fail while physically founding a camp.

## Player-visible outcome
The player can inspect a small founding group as people choose a site, obtain water and food, allocate work, haul materials, construct shelter and storage, form early paths, become injured, migrate, die, or recover. Major causes and events survive save/load.

## Required work
- One bounded regional world-generation envelope with terrain, water, resource, route, and hazard diversity needed by the slice.
- Lean persistent people and households with only fields exercised by the slice.
- Urgent needs, work, movement, reservations, and basic health/life outcomes.
- Physical camp projects, material hauling, construction stages, damage, salvage, and ruins.
- Desire paths/basic road edges and one decision-level project order boundary.
- Omniscient person/camp inspection, one-line causes, and factual major-event summaries.
- Multi-year deterministic continuation and save/load through active work and life changes.

## Decisions opened in this milestone
- D04 person/household detail and initial world-generation envelope.

The exact population count, number of comparison groups, simulation duration, and resource/building list are set in the approved milestone brief from 0.2 evidence. They are not chosen by this document.

## Exit gate
A selected seed suite produces an understandable living camp. People do not teleport goods or labor; the camera does not change outcomes; construction and failure are physical; save/load preserves future checksums; and the creator can answer why the camp survived or failed.

## Stop/redesign triggers
- The simulation is technically active but visually or causally unreadable.
- People require unbounded searches or constant replanning.
- Camp progress is mostly abstract state changes rather than physical work.
- History cannot link major outcomes to people, places, and interventions.

## Explicitly deferred
Durable village/town breadth, multiple society divergence, trade, diplomacy, warfare, full player powers, advanced history, ecosystems, and release polish.
