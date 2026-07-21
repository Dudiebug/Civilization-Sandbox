# Performance Budgets

## Fixed architectural contracts
These apply from the first implementation task:
- Fixed authoritative scheduling with no unbounded backlog.
- Camera, frame rate, visibility, and presentation density cannot change authoritative results.
- Headless execution and deterministic checkpoint comparison.
- Bounded candidates, spatial queries, allocations, trace volume, and cache invalidation.
- Semantic saves are atomic, validated, recoverable, and measurable.
- Speed controls report sustainable limits rather than silently dropping simulation work.

The Blueprint's 20 Hz reference loop remains the starting architecture assumption unless a measured ADR changes it.

## Milestone-set budgets
Exact Version 1.0 scale numbers are deliberately not chosen in this planning revision. TASK-011 establishes a measured ladder with four categories:
1. laboratory tier;
2. candidate Version 1.0 target tier;
3. Version 1.0 stress tier;
4. Version 1.5 horizon tier.

The creator accepts the Version 1.0 target only after tick, memory, save/load, routing, and presentation evidence is available. The accepted values are recorded in a budget ADR and release-scope manifest.

## Required Version 1.0 budget categories
Before 0.9 can pass, the approved release envelope must state and verify:
- authoritative loop p50/p95/max and backlog behavior;
- ordinary and stress presentation frame rate;
- persistent-person, building, shipment, and route counts;
- working-set limit;
- save size, snapshot hitch, save time, and load time;
- decision, query, and trace limits;
- accelerated simulation and wall-clock soak durations;
- reference hardware and supported build configurations.

## Version 1.5 horizon reference
The original Blueprint targets remain future evidence goals: 10,000 persistent people, 5,000 moving billboards, 1,000 high-detail buildings, less than 6 GB working set, a representative 100-year save below 100 MB, sub-250 ms snapshot hitch, save below 10 seconds, load below 15 seconds, a 24-hour soak, and a 500-year accelerated run. Missing those targets does not block Version 1.0 unless the creator explicitly promotes one into its approved release envelope.

## Change rule
Budgets change only through profiler evidence and an accepted ADR. A feature may not hide cost by reducing simulation truth when off-screen.
