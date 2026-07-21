# Planned Package Map

> ADR-002 accepts the Unity-shaped package baseline. This map describes eventual Version 1.5 ownership, not a Version 1.0 feature checklist. The active milestone controls which packages and capabilities may be implemented.


| Package | Primary ownership | Activation boundary |
|---|---|---|
| `com.civsandbox.simulation.core` | Authoritative clock, stable IDs, units, fixed-point values, scheduler, keyed RNG, deterministic containers, command boundary primitives, and checksums. | Milestones 0.1-0.2 foundation. |
| `com.civsandbox.ai.runtime` | Canonical behavior-contract runtime: authority contexts, knowledge filtering, candidates, preconditions, utility curves, commitments, reservations, deterministic selection, budgets, and traces. | Milestone 0.2 first-three-decision proof. |
| `com.civsandbox.world` | World seed/version, authoritative coordinates, terrain fields, hydrology, climate, soil, resources, deposits, hazards, chunks, spatial indexes, and world validation. | Bounded 0.2-0.3 proof; broader systems reserved. |
| `com.civsandbox.people` | Persistent person identity and life state, health, needs, skills, work intent, traits, attitudes, beliefs, ties, experiences, position, and appearance descriptors. | Lean 0.3 subset; Version 1.5 depth reserved. |
| `com.civsandbox.households` | Co-residence, guardians/dependents, shared inventory/money/obligations, housing, migration, inheritance, and household decisions. | Lean 0.3 subset; Version 1.5 depth reserved. |
| `com.civsandbox.settlements` | Settlement identity, parcels, land rights, projects, buildings, construction, services, neighborhoods, roads/infrastructure proposals, damage, abandonment, salvage, and recovery. | 0.2 contracts and 0.3 physical proof only. |
| `com.civsandbox.economy` | Commodities, inventories, reservations, ownership, workplaces, recipes, labor demand, markets, prices, simple credit, shipments, manifests, and conservation diagnostics. | Minimal conservation/labor support when required; market depth reserved. |
| `com.civsandbox.knowledge` | Capability hypergraph, knowledge dimensions, carriers, invention projects, prototypes, adoption, diffusion, loss, rediscovery, and reverse engineering. | Reserved for a later approved milestone. |
| `com.civsandbox.politics` | Culture tendencies, religion modules, patriotism/identity, government composition, law, policy/enforcement, legitimacy, factions, succession, reform, coups, rebellion, and civilization formation. | Reserved for a later approved milestone. |
| `com.civsandbox.diplomacywar` | Contact, believed information, claims, treaties, diplomacy, war goals, mobilization, squads, operations, supply, sector encounters, occupation, refugees, and peace aftermath. | Reserved for a later approved milestone. |
| `com.civsandbox.ecosystems` | Regional wildlife populations, persistent notable/livestock/working animals, habitat, pressure, extinction, recovery, pollution/ecological feedback, and disaster hooks. | Reserved for a later approved milestone. |
| `com.civsandbox.playerpowers` | Validated player command classification, Binding Orders, indirect interventions, Force Outcome mutations, reconciliation, previews, warnings, undo-as-command/branch, and intervention history. | Architecture reserved; runtime waits for its approved milestone. |
| `com.civsandbox.persistence` | Semantic save container, manifests, domain chunks, migrations, atomic write/backup, corruption diagnostics, stable-reference resolution, and cache rebuild orchestration. | Milestone 0.1 semantic skeleton; domains opt in through owning tasks. |
| `com.civsandbox.history` | Historical event graph, typed causal edges, significance, perspective records, entity/place indexes, retention, compaction, landmark references, and query caps. | Minimal causal record in 0.1; graph depth reserved. |
| `com.civsandbox.story` | Incident candidate catalog, Story Director selection/pacing/profiles, factual chronicles, timelines, historical maps, template grammar, provenance, and repetition controls. | Factual contract only; full Story Director reserved. |
| `com.civsandbox.presentation` | Read-only snapshot consumption, 3D terrain/roads/buildings, camera-facing billboards, vehicles, effects, LOD, camera, occlusion, selection visuals, and debug overlays. | Configurable 0.2 proof; broader presentation later. |
| `com.civsandbox.ui` | Immutable query models, HUD, inspectors, search, overlays, comparisons, traces, history, orders, Force Outcome previews, accessibility, and input mapping. | Debug/proof surfaces first; final workspace scope deferred. |
| `com.civsandbox.content` | Versioned immutable definitions, namespaced stable IDs, units, recipes, curves, policies, actions, capabilities, visual descriptors, localization keys, and validators. | Schema foundation now; active content only by milestone. |
| `com.civsandbox.telemetry` | System timing, backlog, allocations, memory, query counts, candidate counts, trace volume, save metrics, performance reports, and diagnostic state diff integration. | Foundation telemetry now; reports expand with systems. |
| `com.civsandbox.tooling` | Editor/content authoring, validators, previews, fixture builders, benchmark launchers, evidence packaging, and migration tools. | Milestone 0.1 bootstrap and validators. |

TASK-001 accepted the engine/package pins. TASK-003 owns enforceable dependency arrows. This map defines intended ownership, not completed implementation or permission to activate reserved Version 1.5 scope.

Build 01 activates only the fixed clock, stable identity, keyed seed/reset, bounded coordinates, named-person movement, read-only snapshot, 2.5D presentation, and World Viewer UI portions of `simulation.core`, `world`, `people`, `presentation`, and `ui`. Every broader ownership area in those packages remains reserved for its named later build.
