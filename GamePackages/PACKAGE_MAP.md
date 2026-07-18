# Planned Package Map

| Package | Primary ownership |
|---|---|
| `com.civsandbox.simulation.core` | Authoritative clock, stable IDs, units, fixed-point values, scheduler, keyed RNG, deterministic containers, command boundary primitives, and checksums. |
| `com.civsandbox.ai.runtime` | Canonical behavior-contract runtime: authority contexts, knowledge filtering, candidates, preconditions, utility curves, commitments, reservations, deterministic selection, budgets, and traces. |
| `com.civsandbox.world` | World seed/version, authoritative coordinates, terrain fields, hydrology, climate, soil, resources, deposits, hazards, chunks, spatial indexes, and world validation. |
| `com.civsandbox.people` | Persistent person identity and life state, health, needs, skills, work intent, traits, attitudes, beliefs, ties, experiences, position, and appearance descriptors. |
| `com.civsandbox.households` | Co-residence, guardians/dependents, shared inventory/money/obligations, housing, migration, inheritance, and household decisions. |
| `com.civsandbox.settlements` | Settlement identity, parcels, land rights, projects, buildings, construction, services, neighborhoods, roads/infrastructure proposals, damage, abandonment, salvage, and recovery. |
| `com.civsandbox.economy` | Commodities, inventories, reservations, ownership, workplaces, recipes, labor demand, markets, prices, simple credit, shipments, manifests, and conservation diagnostics. |
| `com.civsandbox.knowledge` | Capability hypergraph, knowledge dimensions, carriers, invention projects, prototypes, adoption, diffusion, loss, rediscovery, and reverse engineering. |
| `com.civsandbox.politics` | Culture tendencies, religion modules, patriotism/identity, government composition, law, policy/enforcement, legitimacy, factions, succession, reform, coups, rebellion, and civilization formation. |
| `com.civsandbox.diplomacywar` | Contact, believed information, claims, treaties, diplomacy, war goals, mobilization, squads, operations, supply, sector encounters, occupation, refugees, and peace aftermath. |
| `com.civsandbox.ecosystems` | Regional wildlife populations, persistent notable/livestock/working animals, habitat, pressure, extinction, recovery, pollution/ecological feedback, and disaster hooks. |
| `com.civsandbox.playerpowers` | Validated player command classification, Binding Orders, indirect interventions, Force Outcome mutations, reconciliation, previews, warnings, undo-as-command/branch, and intervention history. |
| `com.civsandbox.persistence` | Semantic save container, manifests, domain chunks, migrations, atomic write/backup, corruption diagnostics, stable-reference resolution, and cache rebuild orchestration. |
| `com.civsandbox.history` | Historical event graph, typed causal edges, significance, perspective records, entity/place indexes, retention, compaction, landmark references, and query caps. |
| `com.civsandbox.story` | Incident candidate catalog, Story Director selection/pacing/profiles, factual chronicles, timelines, historical maps, template grammar, provenance, and repetition controls. |
| `com.civsandbox.presentation` | Read-only snapshot consumption, 3D terrain/roads/buildings, camera-facing billboards, vehicles, effects, LOD, camera, occlusion, selection visuals, and debug overlays. |
| `com.civsandbox.ui` | Immutable query models, HUD, inspectors, search, overlays, comparisons, traces, history, orders, Force Outcome previews, accessibility, and input mapping. |
| `com.civsandbox.content` | Versioned immutable definitions, namespaced stable IDs, units, recipes, curves, policies, actions, capabilities, visual descriptors, localization keys, and validators. |
| `com.civsandbox.telemetry` | System timing, backlog, allocations, memory, query counts, candidate counts, trace volume, save metrics, performance reports, and diagnostic state diff integration. |
| `com.civsandbox.tooling` | Editor/content authoring, validators, previews, fixture builders, benchmark launchers, evidence packaging, and migration tools. |

Exact manifests, versions, and dependency arrows are accepted during TASK-001/TASK-003. This map defines intended ownership, not completed implementation.
