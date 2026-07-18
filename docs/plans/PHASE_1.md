# Phase 1 — Deterministic Simulation Core and Engine Proof

**Status:** Not Started  
**Estimated duration:** 5–7 weeks

## Objective
Prove the engine and architecture can support deterministic headless simulation, semantic persistence, spatial work, and representative scale.

## Entry condition
Phase 0 clean-checkout gate accepted.

## Work checklist
- [ ] Implement typed world time, 20 Hz fixed loop, due queues, staggered cadences, and backlog watchdog.
- [ ] Implement stable nonreused domain IDs and tombstone policy.
- [ ] Implement keyed RNG streams and deterministic reductions.
- [ ] Implement integer/fixed-point authoritative primitives and explicit units.
- [ ] Implement canonical state serialization, checksum, and diff diagnostics.
- [ ] Implement semantic save skeleton, atomic replacement, backup, load, and migration fixture.
- [ ] Implement headless runner and command-log replay.
- [ ] Implement synthetic 10,000-person/workplace/shipment load.
- [ ] Implement 256 m chunk indexing and bounded spatial queries.
- [ ] Spike strategic/settlement/local hierarchical routing and bounded cache invalidation.
- [ ] Create read-only presentation bridge.
- [ ] Run 5,000-billboard and 1,000-building stress scene.
- [ ] Measure memory, allocations, backlog, save/load, replay, and presentation budgets.
- [ ] Run at most two evidence-led optimization passes before engine fallback decision.

## Exit gate
The Section 57/68 engine benchmark passes. Otherwise document whether architecture correction or a bounded Bevy comparison spike is required.

## Do not build in this phase
Real civilization AI, politics, broad content, advanced rendering, or final assets.

## Completion record
Evidence index: Pending  
Independent phase review: Pending  
Creator gate decision: Pending
