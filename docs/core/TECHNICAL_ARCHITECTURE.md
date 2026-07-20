# Technical Architecture

## Provisional engine position
Blueprint v2.0 recommends Unity 6.3 LTS with C#, Entities/DOTS, Burst, Job System, Entities Graphics, and URP. The repository currently contains Unity-shaped placeholder boundaries. **The exact engine path is not accepted by this roadmap revision.** Milestone 0.1 must record the creator's engine decision and pin the selected toolchain before gameplay implementation.

## System boundary
```
Player/UI commands -> validated command boundary -> authoritative simulation
Authoritative simulation -> immutable read models -> UI/presentation
Authoritative simulation -> persistence/history/traces/tests
Versioned content -> validators -> runtime definitions
```

## Required properties
- Fixed authoritative scheduling with typed game time and due-time work.
- Headless batch execution and clean-checkout command-line build/test.
- Stable ordering, keyed RNG, canonical serialization, checksums, and state diffs.
- Explicit cadence, candidate cap, query cap, allocation rule, and benchmark for every system.
- Hierarchical strategic/settlement/local routing rather than per-person global pathfinding.
- Bounded structural changes and cache invalidation.
- Presentation may interpolate, cluster, or omit; it may not change decisions.
- Semantic state and stable IDs remain independent of engine scene/entity handles.

## Release activation rule
Package and schema boundaries may reserve Version 1.5 capabilities, but implementation must follow the active milestone. Do not build full politics, ecosystems, story direction, diplomacy, or large-scale systems merely because a package exists.

## Package rule
Domain packages expose narrow APIs. Higher authority layers may constrain lower layers but may not directly mutate their internal movement, inventory, or presentation state. Exact package names and dependency manifests are finalized only after the engine path is accepted.
