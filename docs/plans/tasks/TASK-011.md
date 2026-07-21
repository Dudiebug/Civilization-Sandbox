# TASK-011 - Measured scale ladder and engine benchmark

**Status:** Not Started
**Milestone:** 0.2 - Decision and Engine Proof
**Release horizon:** Select Version 1.0 scale from evidence; measure Version 1.5 horizon without making it a 1.0 blocker
**Risk:** Critical
**Depends on:** TASK-005, TASK-006, TASK-007, TASK-009, TASK-010
**Decision dependencies:** D03 closes after evidence
**Evidence folder:** `docs/evidence/TASK-011/`
**Blueprint/ADR source:** Blueprint Sections 55-57, 68, 75.2, 81; ADR-001

## Creator summary
Measure real cost at several tiers before choosing the Version 1.0 world/population target. Do not assume the original 10,000-person goal is a first-release requirement.

## Objective
Run representative synthetic people, households, needs, jobs, locations, structures, and shipments through a configurable scale ladder, then recommend a target and stress tier for Version 1.0.

## In scope
- Stable-seed synthetic data generator.
- Representative component/access/cadence patterns without full gameplay AI.
- Multiple scale tiers named laboratory, candidate 1.0, 1.0 stress, and 1.5 horizon.
- Memory, allocations, system duration, backlog, save/load, and trace-off measurements.
- Reference-machine reports and D03 recommendation.

## Required outputs
- [ ] Configurable benchmark scenarios and runner.
- [ ] Machine-readable and creator-readable scale report.
- [ ] Candidate Version 1.0 budget ADR.
- [ ] Nonblocking Version 1.5 horizon findings, including the Blueprint 10,000-person target where practical.

## Verification and acceptance
- [ ] Each tested tier records comparable p50/p95/max, memory, backlog, save, and load evidence.
- [ ] Steady-state hot loops meet declared allocation rules.
- [ ] Repeated runs have stable variance and checksums.
- [ ] D03 is decided by the creator from evidence; the task does not choose it silently.
- [ ] Failure at the 1.5 horizon does not fail the 1.0 gate if the accepted 1.0 tier passes.
- [ ] Independent review and creator acceptance pass.

## Out of scope
Claiming scale from record count alone, building full AI, or locking world/population numbers before measurement.
