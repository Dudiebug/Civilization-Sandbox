# TASK-022 - Lean persistent people and households

**Status:** Not Started
**Milestone:** 0.3 - Living Camp
**Release horizon:** Minimum persistent human substrate now; full Blueprint people/household depth by Version 1.5
**Risk:** Critical
**Depends on:** TASK-006, TASK-010, TASK-019, TASK-021
**Decision dependencies:** D04
**Evidence folder:** `docs/evidence/TASK-022/`
**Blueprint/ADR source:** Blueprint Sections 18-23, 69-75, 81; ADR-001

## Creator summary
Create only the person and household state exercised by the Living Camp: stable identity, basic life needs, home/group support, work, movement, injury/death, and major experiences.

## Objective
Persist named people and lightweight households through the approved Living Camp duration, active needs/work, migration or separation, death, event participation, and save/load.

## In scope
- Stable person identity, name, age/life state, position, current action/goal, and appearance descriptor.
- Approved minimum health/need state.
- Household membership, guardians/dependents where needed, home/shelter, pooled survival resources, and movement commitment.
- A small approved trait/aptitude set that changes declared decision terms.
- Work eligibility/assignment interface and meaningful major experiences.
- Death, household disruption, and event history.

## Required outputs
- [ ] Lean people/household packages and DTOs.
- [ ] Approved D04 field catalog and policy data.
- [ ] Long-enough demographic/lifecycle fixtures for the milestone.
- [ ] Omniscient read models.
- [ ] Version 1.5 extension map for inheritance, attitudes, beliefs, ties, and broader life simulation.

## Verification and acceptance
- [ ] Accepted target population retains stable identity through save/load without reuse.
- [ ] Traits affect only declared terms and cannot bypass feasibility.
- [ ] Household pooled survival resources reconcile.
- [ ] Death/migration updates memberships and history coherently.
- [ ] Camera absence produces identical checkpoints.
- [ ] Independent review and creator acceptance pass.

## Out of scope
The 10,000-person gate, detailed inheritance, romance, exhaustive genealogy, political/religious attitudes, large social graphs, broad education/skills, or organ-level health.
