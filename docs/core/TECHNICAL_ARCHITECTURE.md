# Technical Architecture

## Provisional stack
Unity 6.3 LTS, C#, Entities/DOTS, Burst, Job System, Entities Graphics, and URP. Exact patch and package versions are pinned during Task 001. Unity remains provisional until Phase 1 benchmark evidence passes.

## System boundary
```
Player/UI commands -> validated command boundary -> authoritative simulation
Authoritative simulation -> immutable read models -> UI/presentation
Authoritative simulation -> persistence/history/traces/tests
Versioned content -> validators -> runtime definitions
```

## Required properties
- Fixed 20 Hz authoritative wall loop with typed game time and due-time scheduling.
- Headless batch execution and clean-checkout command-line build/test.
- Stable ordering, keyed RNG, canonical serialization, checksums, and state diffs.
- Explicit cadence, candidate cap, query cap, allocation rule, and benchmark for every system.
- Strategic region graph → settlement/road graph → short local traversal.
- Structural changes and cache invalidation are bounded and measured.
- Presentation may interpolate, cluster, or omit; it may not change decisions.

## Package rule
Domain packages expose narrow APIs. Higher authority layers may constrain lower layers but may not directly mutate their internal movement, inventory, or presentation state.
