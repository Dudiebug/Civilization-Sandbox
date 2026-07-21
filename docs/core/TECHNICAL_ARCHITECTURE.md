# Technical Architecture

## Accepted engine baseline
ADR-002 accepts Unity `6000.3.20f1`, C#, the pinned package graph, Windows Mono, and Linux Mono for the Milestone 0.1 development baseline. The repository contains Unity package boundaries and a verified bootstrap scene. TASK-011 still owns the measured scale ladder and any evidence-triggered engine continuation decision; package acceptance is not permission to implement future-release systems early.

## System boundary
```
Player/UI commands -> validated command boundary -> authoritative simulation
Authoritative simulation -> immutable read models -> UI/presentation
Authoritative simulation -> persistence/history/traces/tests
Versioned content -> validators -> runtime definitions
```

## Dependency direction

- `simulation.core` supplies stable time, identity, units, deterministic primitives, command boundaries, and checksums; it does not depend on domain or presentation packages.
- Versioned content definitions feed domain packages through validated stable IDs.
- Domain packages may use the minimal simulation and AI foundations and expose commands, semantic persistence DTOs, events, and immutable read models.
- Persistence serializes semantic DTOs rather than engine memory. Telemetry observes declared interfaces and may not become a hidden command path.
- Presentation and UI consume read models and send validated commands. Authoritative packages never reference camera, rendering, scene, audio, editor, or mutable UI state.
- Circular package references and cross-domain store mutation are forbidden. TASK-003 converts these rules into automated gates.

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
Domain packages expose narrow APIs. Higher authority layers may constrain lower layers but may not directly mutate their internal movement, inventory, or presentation state. ADR-002 and the pinned package manifest own the current engine/toolchain graph; TASK-003 owns enforcement of domain dependency arrows.
