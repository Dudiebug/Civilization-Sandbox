# Full Playable-Build Roadmap

This roadmap covers the complete scoped vision. It preserves the Blueprint and reorganizes delivery around playable Unity builds.

## Delivery principles

- The game remains playable after every accepted implementation slice.
- Every build adds an end-to-end player-observable capability.
- Infrastructure is built inside the first playable behavior that needs it.
- Detailed plans exist only for the current build; later builds retain outcome and scope boundaries.
- Counts and depth are adjusted from measurements and play without deleting the long-term vision.

## Build 00 - Compact Foundation

**Playable result:** the Unity project reliably opens, builds, runs a test scene, and can be safely changed by Codex.

**Scope:** source control and rollback, pinned Unity baseline, one local build/test path, minimal separation between simulation and presentation, deterministic seed convention, and concise repository instructions.

**Gate:** a clean checkout launches the same test scene and a failed compile or forbidden camera-to-simulation dependency produces a useful error.

**Defer:** broad analyzer frameworks, full package scaffolding, gameplay-independent save architecture, exhaustive evidence automation, and future-domain abstractions.

## Build 01 - World Viewer

**Playable result:** move and zoom the camera, pause and change time speed, watch a seeded group of visible named people move, click one, and inspect simple state.

**Scope:** authoritative fixed time, stable person IDs, deterministic seed, minimal position/movement, read-only presentation snapshots, basic camera, time controls, selection, and inspector.

**Gate:** camera movement and rendering do not alter simulation checkpoints; reset with the same seed reproduces the same initial world.

## Build 02 - Founding Camp

**Playable result:** a small group searches for food and water, gathers resources, shares a stockpile, creates shelter, and either stabilizes or fails for understandable reasons.

**Scope:** regional test terrain, resources, hunger/thirst/health, reachable-resource decisions, gathering, hauling, stockpile, simple reservations, shelter construction, action reasons, event log, and minimum save/reload.

**Gate:** one seed produces a viable camp and one produces an explainable failure; save/reload during a critical need preserves subsequent outcomes.

## Build 03 - Living Camp

**Playable result:** named people persist across seasons and meaningful life changes while the camp develops physically.

**Scope:** lightweight households/support groups, age/life state, work aptitude, simple traits, illness/injury, arrivals/deaths, construction stages, storage, camp paths, damage/abandonment, factual timeline, and stronger persistence.

**Gate:** the creator can follow one named person from need through work, movement, construction, injury, migration, or death and inspect the causes.

## Build 04 - Physical Settlement

**Playable result:** the camp autonomously becomes a durable village through real labor, materials, production, services, and routes.

**Scope:** bounded resource and recipe set, occupations, workplaces, ownership/reservations, multiple building archetypes, projects, roads/bridge bottleneck, maintenance, damage, salvage, rebuilding, and settlement summaries.

**Gate:** destroy or block a key route/building and observe shortages, displacement, adaptation, repair, and a persistent scar.

## Build 05 - Divergent Societies

**Playable result:** two societies facing a comparable pressure develop visibly different solutions for causal reasons.

**Scope:** society identity, institutions, thin government, authority, priorities, culture/religion tendencies at approved depth, knowledge/capability states, invention/adoption, leadership, collective decisions, and comparison explanations.

**Gate:** matched-seed scenarios demonstrate different feasible outcomes tied to geography, resources, institutions, knowledge, and history rather than faction scripts.

## Build 06 - Connected Region

**Playable result:** settlements recognize each other, exchange goods and knowledge, depend on routes, make agreements, and experience migration when the network changes.

**Scope:** contact, believed information, shipments, manifests, route capacity, delays/loss, bounded trade/allocation/prices, agreements, migration, and regional overlays.

**Gate:** disrupting one bridge or corridor creates an explainable causal chain through shipment delay, shortage, policy response, and migration without duplicating goods.

## Build 07 - Conflict and Recovery

**Playable result:** disputes can escalate into physical conflict that kills and displaces people, damages settlements, ends through peace, and leaves lasting recovery work.

**Scope:** claims and war goals, mobilization, persistent combat groups, supply, bounded physical encounters, casualties, refugees, occupation at approved depth, peace, demobilization, salvage, rebuilding, ruins, and historical memory.

**Gate:** a complete conflict scenario preserves individual outcomes, logistics, damage, peace terms, displacement, recovery, and save/load continuity.

## Build 08 - God Sandbox

**Playable result:** use omniscient inspection, issue Binding Orders, alter conditions indirectly, force outcomes, and compare the consequences in history.

**Scope:** search, overlays, causal traces, representative high-level orders, material interventions, Force Outcome transaction/reconciliation, warnings, intervention ancestry, history view, accessibility foundation, and consolidated UI.

**Gate:** the player can observe, direct, intervene, and force different outcomes while the game clearly distinguishes each authority path and records what happened.

## Build 09 - Founding Worlds 1.0

**Playable result:** a coherent lean regional civilization sandbox suitable for real play sessions and distribution.

**Scope:** integrated identity loop, onboarding, accessibility, tuning, content lock, performance, accelerated simulation, stable saves/migrations/recovery, diagnostics, packaging, support information, and release playtesting.

**Gate:** a clean player can create a world, observe founding/divergence/interdependence/conflict/recovery, use each player-control class, understand major causes, save/exit/restore, and continue without developer assistance.

## Build 10 - Living People 1.1

**Playable result:** people feel more individually persistent across family, learning, personality, health, relationships, speech, and life history.

**Preserved scope candidates:** dependents, inheritance, possessions, skill development, education, active traits, attitudes, beliefs, social ties, contextual speech, experiences, and deeper household continuity.

## Build 11 - Roads and Markets 1.2

**Playable result:** settlements and regions develop richer material economies and distinct physical forms.

**Preserved scope candidates:** broader commodities/recipes/occupations/buildings, ownership regimes, prices, credit, workplaces, services, roads, bridges, shipments, route hierarchies, urban form, redevelopment, and infrastructure maintenance.

## Build 12 - States and Beliefs 1.3

**Playable result:** societies develop deeper political, institutional, religious, cultural, and technological identities.

**Preserved scope candidates:** government presets, law/enforcement, legitimacy, factions, succession, reform, coups, rebellion, religion, patriotism, assimilation, capability hypergraph, invention, diffusion, loss, rediscovery, and reverse engineering.

## Build 13 - War and World 1.4

**Playable result:** international politics, warfare, environments, powers, and historical products operate at mature early-modern depth.

**Preserved scope candidates:** full diplomacy set, treaties, alliances, coercion, occupation, military supply, ecosystems, pollution, disasters, extinction/recovery, Story Director profiles, chronicles, historical maps, expanded powers, and history interfaces.

## Build 14 - Complete Early Modern 1.5

**Playable result:** the complete original Blueprint early-modern target.

**Scope completion:** evidence-governed 10,000-person/Large-world target, full approved content catalogs, four government presets, six technology domains, twelve diplomacy actions, twenty-four power target, thirteen workspace target, 24-hour soak, 500-year run, 100-year replay, complete saves/migrations, accessibility, performance, and creator acceptance.

## Build 15 - Industrial Transition 2.0

**Playable result:** societies industrialize through different material and institutional paths.

**Scope:** steam power, machine tools, factories, rail, coal chains, industrial labor, dense pollution, mass education, larger cities, industrial accidents, labor politics, and continuity from early-modern capabilities.

## Build 16 - Electrification 3.0

**Playable result:** grids, communication, utilities, and precision industry reshape settlements and state power.

**Scope:** electricity generation/distribution, telecommunications, precision manufacture, modern utilities, expanded services, dense multicenter cities, grid failure, and infrastructure dependence.

## Build 17 - Mechanization 4.0

**Playable result:** faster transport, aircraft, fuel networks, and mechanized conflict transform regional scale.

**Scope:** automobiles, roads/highways, petroleum logistics, aircraft, mechanized armies, faster civilian/military supply, tactical abstraction changes, and new urban forms.

## Build 18 - Atomic and Information Age 5.0

**Playable result:** strategic weapons, computing, electronic communication, surveillance, and deterrence alter society and international behavior.

**Scope:** radiation, nuclear power/weapons, missiles, computing, electronic networks, intelligence, surveillance, automation foundations, strategic diplomacy, accidents, and long-lived consequences.

## Build 19 - Divergent Futures 6.0+

**Playable result:** civilizations pursue genuinely different technological and social futures rather than one universal endgame.

**Scope:** automation, biotechnology, alternate energy, synthetic environments, institutional futures, divergent development paths, and evidence-driven additions consistent with causal simulation.

## Build 20 - Distant Expansion

**Playable result:** civilization expands beyond the core world while preserving named continuity, physical logistics, autonomy, divergence, and history.

**Scope:** orbital topology, celestial resources, launch/transport infrastructure, interplanetary logistics, scale transitions, remote settlements, communication delays, multi-planet governance, and cross-world historical continuity.

## Release allocation rule

Builds 10-13 are working themes, not irreversible promises. After each release, the creator selects the most valuable preserved scope from `SCOPE_COVERAGE.md`. Build 14 closes every remaining complete-early-modern requirement. Later-age builds begin only after the previous product is stable and enjoyable.
