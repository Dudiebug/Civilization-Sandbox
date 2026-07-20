# TASK-008 - Causal event record foundation

**Status:** Not Started
**Milestone:** 0.1 - Project Foundation
**Release horizon:** Lean Version 1.0 minimum history; full event graph is Version 1.5 horizon
**Risk:** High
**Depends on:** TASK-006, TASK-007
**Decision dependencies:** None
**Evidence folder:** `docs/evidence/TASK-008/`
**Blueprint/ADR source:** Blueprint Sections 47-54 and 81; ADR-001

## Creator summary
Record stable major events early so later causality is not reconstructed from incomplete final state, without building the full historical graph before gameplay exists.

## Objective
Persist a minimal factual event record with stable identity, participants, location, immediate causes/effects, significance, and intervention ancestry; reserve clean extension to the Version 1.5 graph.

## In scope
- Stable event IDs and versioned minimum schema.
- Event type, time, location, participants, authoritative facts, cause references, state changes, significance, and intervention ancestry.
- Bounded entity/location indexes and query caps.
- Semantic save integration and referential validation.
- Sample command, save, and lifecycle events.

## Required outputs
- [ ] Minimum event package/DTOs/query API/tests.
- [ ] Version 1.5 extension notes for typed edges, perspectives, historical maps, and deeper retention.

## Verification and acceptance
- [ ] Round-trip preserves events and references.
- [ ] Queries enforce caps.
- [ ] Protected major events cannot be removed while referenced by current state or a ruin.
- [ ] Broken event/entity references fail validation.
- [ ] No prose generator or full Story Director is introduced.
- [ ] Independent review and creator acceptance pass.

## Out of scope
Full perspective history, long-form chronicles, historical maps, unbounded causal graph queries, or story systems that own outcomes.
