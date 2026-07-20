# Pre-1.0 Development Roadmap

**Program target:** a lean regional Version 1.0 that expresses the full game identity without attempting the complete Blueprint v2.0 early-modern breadth.
**North-star target:** Version 1.5, which inherits the original Blueprint v2.0 complete early-modern scope.
**Decision policy:** choose only what the active milestone needs; leave later quantities and content catalogs open.

## 1. Roadmap purpose

This roadmap converts the north-star design into a sequence of independently testable products. Each milestone must produce an integrated proof, not merely add isolated systems. The sequence is ordered to answer the highest-risk questions before content production becomes expensive:

1. Can the project be reproduced, tested, saved, replayed, and audited?
2. Can the selected engine and architecture sustain an evidence-based scale tier?
3. Can named people physically found a camp for understandable reasons?
4. Can that camp become a durable autonomous settlement?
5. Can multiple societies diverge without scripted identities?
6. Can physical routes and exchange connect their outcomes?
7. Can conflict create persistent human and physical consequences?
8. Does the omniscient god-sandbox interaction make those systems compelling?
9. Can the result be shipped, supported, and continued safely?

The roadmap deliberately does not choose exact population counts, world dimensions, content counts, government forms, economy depth, conflict depth, power roster, workspace count, or final release test duration. Those decisions belong to the evidence gates listed in `DECISION_QUEUE.md`.

## 2. What is already fixed

The following are release-identity constraints rather than optional features:

- persistent named people whose consequential state survives save/load;
- physical resources, construction, routes, damage, rebuilding, ruins, and scars;
- societies that can develop differently because of geography, resources, institutions, and history;
- autonomous exact implementation rather than ordinary manual placement;
- omniscient inspection with useful causal explanation;
- distinct Binding Order, indirect intervention, and Force Outcome semantics;
- durable major events linked to people, places, actions, and physical remnants;
- camera-independent authoritative simulation, stable identity, semantic persistence, bounded work, and evidence-gated development.

Everything else is subject to milestone scoping unless an accepted ADR says otherwise.

## 3. Program operating model

### 3.1 Evidence before commitment

Each milestone begins with a short scope brief based on the prior gate. The creator approves only the decisions needed for that milestone. Detailed task contracts are then generated for that milestone only. This prevents untested assumptions from becoming permanent architecture or a large backlog.

### 3.2 One integrated proof per milestone

A milestone is not accepted because its individual modules pass separately. Its gate scenario must exercise the modules together, preserve authoritative causality, save and resume correctly, remain camera-independent, and produce an understandable player-visible result.

### 3.3 Architecture reservation

Future-facing schemas may contain stable categories, extension points, explicit units, and migration-safe identifiers. They may not contain dormant implementations of later releases. “Design the doorway” means preserve compatibility, not build unused rooms.

### 3.4 Decision timing

- Open a decision only when the active milestone cannot proceed without it.
- Present one decision at a time.
- Offer evidence-grounded options, tradeoffs, and a defer option.
- Record accepted decisions in an ADR, milestone brief, or release-scope manifest.
- Stop rather than invent a temporary product truth that downstream work could mistake for a creator decision.

### 3.5 Completion evidence

Evidence is proportional to risk, but every milestone normally includes:

- clean bootstrap/build/test output;
- deterministic replay or canonical state comparison;
- persistence and migration results;
- performance and bounded-work measurements;
- integrated behavior scenarios, including failure cases;
- player-visible explanation or inspection evidence where applicable;
- independent adversarial review;
- creator-visible acceptance and a go/conditional/no-go decision.

## 4. Cross-cutting workstreams

The milestones are sequential, but the following workstreams continue across them.

| Workstream | Starts | Evolves through | Version 1.0 responsibility |
|---|---:|---:|---|
| Repository, CI, evidence, rollback | 0.1 | 0.9 | Every accepted build is reproducible and diagnosable |
| Deterministic simulation and scheduling | 0.1 | 0.9 | Camera-independent authoritative outcomes |
| Stable identity, events, saves, migrations | 0.1 | 0.9 | Named continuity and recoverable long-running worlds |
| Spatial queries, routing, presentation separation | 0.2 | 0.9 | Bounded movement and readable physical worlds |
| Coded intelligence and decision traces | 0.2 | 0.9 | Important choices are bounded and explainable |
| People, households, labor, construction | 0.3 | 0.9 | Human-scale causal foundation |
| Settlement economy and infrastructure | 0.4 | 0.9 | Physical growth and bottlenecks |
| Society identity and divergence | 0.5 | 0.9 | Distinct outcomes without scripted factions |
| Regional interaction and exchange | 0.6 | 0.9 | Interdependence, shortages, and migration |
| Conflict and recovery | 0.7 | 0.9 | Persistent destruction, displacement, peace, and rebuilding |
| Player powers, UI, and history | 0.8 | 0.9 | Complete god-sandbox loop |
| Release engineering and support | 0.1 | 0.9 | Stable standalone product |

## 5. Milestone 0.1 — Project Foundation

### Intent

Create a safe, engine-aware but gameplay-neutral repository in which authoritative state can advance, be identified, produce a factual event, save, load, replay, and expose a divergence.

### Entry conditions

- Blueprint v2.0 and ADR-001 are present.
- No game implementation is assumed.
- The creator is prepared to resolve only the toolchain and development-host questions required for bootstrap.

### Core workstreams

1. **Repository governance**
   - version control conventions, protected mainline, small changes, rollback, and destructive-operation rules;
   - clean working-tree and evidence expectations;
   - durable repository instructions through scoped `codex.md` files.
2. **Toolchain selection and pinning**
   - evaluate the candidate engine/toolchain against headless execution, deterministic architecture, test automation, 2.5D presentation, save tooling, and creator workflow;
   - record exact versions, dependency locks, reference hosts, and fallback/reconsideration conditions.
3. **Build and CI harness**
   - one local and CI command path for bootstrap, build, tests, evidence, and packaging;
   - structured reports, revision metadata, and failure diagnostics.
4. **Authoritative shell**
   - typed world time, fixed scheduling, due queues, stable ordering, and backlog reporting;
   - no camera or frame input in authoritative state.
5. **Identity, randomness, and state comparison**
   - stable nonreused domain IDs;
   - keyed deterministic random streams;
   - canonical serialization/checksum and actionable state diff.
6. **Minimum persistence and event foundation**
   - versioned semantic save container, atomic replacement, previous-good backup, and migration skeleton;
   - minimal factual event record linked by stable IDs.

### Integrated proof

From a clean supported environment, run a small headless fixture twice. The fixture advances fixed time, creates stable entities, uses keyed variation, records an event, saves, loads, continues, and compares canonical state. A deliberate RNG, dependency, or state mutation defect must fail with a useful diagnosis.

### Evidence required

- clean bootstrap/build/test logs and dependency manifest;
- deterministic repeated-run equality;
- deliberate divergence report;
- save round-trip and interrupted-write behavior;
- architecture negative tests;
- independent review and creator acceptance.

### Decisions opened

- D01 engine/toolchain path and exact pins;
- D02 reference development hardware and supported build hosts;
- repository host/location and protection mechanism.

### Decisions kept closed

World scale, people count, world generation, art direction, resource catalog, economy, government, combat, powers, UI organization, release duration, and post-1.0 allocation.

### Exit and handoff

Milestone 0.2 receives a reproducible engine/toolchain, authoritative shell, stable IDs, deterministic RNG, event/persistence foundation, benchmark harness, and a documented rollback path.

## 6. Milestone 0.2 — Decision and Engine Proof

### Intent

Measure the selected engine architecture and prove the shared decision runtime before building production gameplay.

### Entry conditions

- Milestone 0.1 passes on the accepted development host.
- The engine/toolchain and architecture boundaries are recorded.
- Save, replay, state diff, and evidence tooling are usable by benchmark and AI fixtures.

### Core workstreams

1. **Scale ladder**
   - parameterized synthetic entity/state workloads rather than a single headline count;
   - measure simulation time, memory, scheduling backlog, serialization, load, query costs, and presentation density at several tiers;
   - separate release target, stress tier, and Version 1.5 horizon measurements.
2. **Spatial and routing primitives**
   - chunked spatial indices and bounded local queries;
   - strategic/settlement/local route hierarchy;
   - route caching and bounded invalidation rather than global per-person search.
3. **2.5D presentation proof**
   - selected engine equivalent of terrain, simple building instances, camera-facing billboards, selection, and read-only simulation snapshots;
   - configurable density and occlusion experiments;
   - camera operations must not modify authoritative checksums.
4. **Behavior-contract framework**
   - authority owner, knowledge inputs, preconditions, candidate source and cap, score terms, commitments, randomness, invalidation, trace, and performance budget;
   - catalog and versioning rules.
5. **Canonical decision primitives**
   - authority and knowledge filters;
   - bounded candidate queries;
   - reservations, commitment/hysteresis, stable selection, keyed variation, and structured trace output.
6. **Decision Laboratory**
   - deterministic scenario runner with fixture editing, candidate/score inspection, state diff, seed suites, and defect injection;
   - first contracts: founding-site selection, urgent individual need, and settlement labor allocation.

### Integrated proof

Run the three decision contracts at multiple accepted workload tiers. Display the authoritative state through the presentation proof while moving, hiding, and rotating the camera. Save during active commitments, load, continue, and compare traces/checkpoints. Inject hidden knowledge, an illegal authority call, unbounded candidate generation, and nondeterministic tie-breaking; the harness must expose each defect.

### Evidence required

- scale-ladder report with raw commands and environment;
- spatial/routing candidate and invalidation measurements;
- presentation CPU/GPU/instance measurements at tested tiers;
- positive, negative, boundary, replay, save, trace, and performance evidence for each decision contract;
- engine continuation/fallback recommendation;
- D03 decision record and creator gate.

### Decisions opened

- D03 Version 1.0 target scale envelope, development stress tier, and Version 1.5 horizon tier;
- an engine continuation/fallback decision only if the agreed benchmark gate triggers it.

### Decisions kept closed

Production world dimensions, exact person fields, content catalogs, society models, economy, diplomacy, combat, final art, powers, UI workspaces, and release soak target.

### Exit and handoff

Milestone 0.3 receives measured scale limits, spatial/routing primitives, a read-only visual bridge, an accepted decision runtime, and the three behavior contracts needed for a living camp.

## 7. Milestone 0.3 — Living Camp

### Intent

Create the first recognizable game slice: persistent named people physically attempt to found a camp and survive for understandable reasons.

### Entry conditions

- D03 provides the accepted milestone workload envelope.
- Founding-site, urgent-needs, and labor-allocation contracts pass in the laboratory.
- Spatial, routing, presentation, event, save, and replay foundations are available.

### Core workstreams

1. **Regional world envelope**
   - terrain, water, basic fertility/productivity, resources, route costs, and a small hazard set sufficient for meaningful site choice;
   - deterministic generation, validation, and rejected-world diagnostics.
2. **Lean persistent people**
   - stable identity, name, age/life state, position, current action/goal, approved need/health fields, small trait/aptitude set, work relationship, and major experiences;
   - no field is added unless a 0.3 behavior, inspection, persistence, or extension boundary requires it.
3. **Lightweight households or support groups**
   - membership, guardians/dependents where required, shelter/home, pooled survival resources, and joint movement/relocation commitment;
   - coherent disruption when a person dies, leaves, or becomes unable to work.
4. **Physical camp economy**
   - broad survival resources, stockpile, reservations, gathering, hauling, consumption, work, and tool/labor progress at the approved minimum depth.
5. **Construction and paths**
   - temporary shelter, storage, one or more approved durable structures, real footprints/entrances, construction stages, damage, abandonment, salvage, and ruin identity;
   - travel heat, basic paths/road edges, and a representative decision-level project order boundary.
6. **Inspection and factual history**
   - omniscient person/camp inspector, one-line causes, major event timeline, and factual summaries linked to stable evidence;
   - no Story Director or free-form narrative generation.

### Integrated proof

Run a bounded seed suite through site choice, survival, work allocation, material hauling, construction, injury/death or relocation, path formation, and continuation. At least one accepted seed demonstrates a viable camp and one demonstrates an explainable failure or forced move. Save during active construction and a critical need, load, continue, and compare canonical checkpoints. Camera presence and zoom must not alter the outcome.

### Evidence required

- generator validity and rejection report;
- per-person and per-system work/candidate budgets;
- construction material/labor attribution;
- household/support-group conservation and disruption tests;
- multi-year replay/save evidence at the approved envelope;
- player inspection recording or screenshots tied to state evidence;
- identity-guardrail review and creator gate.

### Decisions opened

- D04 minimum person/household detail and regional world-generation envelope;
- exact quantities only to the degree needed to define the 0.3 gate.

### Decisions kept closed

Durable settlement breadth, multiple society contrast model, broad market systems, trade, diplomacy, war, full power roster, final UI, Story Director, ecosystems, and Version 1.0 release test durations.

### Exit and handoff

Milestone 0.4 receives a stable living-camp slice, accepted field/content envelope, physical work and construction loop, basic path infrastructure, factual events, and evidence identifying the real settlement bottlenecks.

## 8. Milestone 0.4 — Physical Settlement

### Intent

Turn the living camp into an autonomous durable settlement through a bounded physical economy, without yet claiming multiple-society divergence.

### Entry conditions

- Milestone 0.3 passes and the creator accepts the Living Camp as readable and causally convincing.
- Performance, save growth, path costs, and decision trace volume from the camp are measured.
- D05 is opened only after reviewing those results.

### Core workstreams

1. **Approved resource and inventory envelope**
   - choose only the resource classes needed to express food/water security, construction, tools, and one meaningful bottleneck;
   - preserve conservation, location, ownership/controller, reservation, consumption/decay, and declared sources/sinks.
2. **Production and workplaces**
   - bounded recipes/lots, roles, capacity, inputs, outputs, maintenance, and blockers;
   - local labor matching and work commitments rather than global job scans.
3. **Building and project breadth**
   - approved housing, storage, production, service, and public/shared archetypes;
   - proposal, authority, site selection, rights, finance/allocation abstraction if needed, reservation, work stages, commissioning, cancellation, repair, demolition, and salvage.
4. **Settlement planning and services**
   - bounded project candidates driven by needs and policy;
   - minimum parcel/right model needed for autonomous location choice;
   - reachable capacity for shelter, water/food distribution, storage, and the approved service set.
5. **Infrastructure and bottleneck proof**
   - path formalization, road condition/capacity, and one accepted bridge, crossing, or corridor bottleneck;
   - route change affects access, hauling, project feasibility, and recovery.
6. **Damage and recovery**
   - capacity loss, displacement, salvage, repair/rebuild choice, abandoned structures, and persistent local ruins.
7. **Settlement information**
   - consolidated inspector and a small set of resource, work, construction, service, and route overlays.

### Integrated proof

A camp autonomously becomes the approved durable settlement form. The settlement constructs housing, production, storage, and a shared/service asset using real deliveries and labor. A key route or neighborhood is then damaged. Capacity, movement, work, shelter, and project priorities change; the settlement either repairs, reroutes, adapts, or declines for traceable reasons. Save/load preserves all active projects, routes, damage, and recovery commitments.

### Evidence required

- resource conservation and reservation tests;
- workplace/recipe and labor-budget results;
- project/site candidate caps and construction attribution;
- reachable-service and route-bottleneck tests;
- damage/displacement/recovery scenario;
- settlement performance/save-growth report;
- creator acceptance and D05 decision record.

### Decisions opened

- D05 resource, building, occupation, and settlement-depth envelope;
- one representative settlement-level Binding Order used to validate player direction without manual placement.

### Decisions kept closed

Final content counts, multiple government catalog, broad markets/credit, inter-settlement trade, full cultural/religious systems, warfare, complete powers, cities, and release content lock.

### Exit and handoff

Milestone 0.5 receives a durable settlement loop, bounded economy and infrastructure primitives, damage/recovery behavior, and a measured base on which contrasting societies can act differently.

## 9. Milestone 0.5 — Divergent Societies

### Intent

Prove that different societies can produce materially different settlements and responses through shared causal primitives rather than scripted faction identities.

### Entry conditions

- The Physical Settlement loop passes for one neutral/reference policy set.
- Common resource, project, route, and service primitives are stable enough to compare different policy/institution settings.
- D06 is opened with matched-world evidence, not selected from the Blueprint catalog by default.

### Core workstreams

1. **Multiple society identity**
   - stable group/civilization identity, membership, territory/settlement association, and leadership or collective authority at the minimum required depth.
2. **Thin institutional/government contrast**
   - select a small number of composable authority/allocation patterns;
   - each changes declared authorization, project, property/allocation, risk, or response terms rather than adding bespoke behavior code.
3. **Culture/policy variables**
   - a small approved set with direct, testable effects on decision weights, project selection, exchange, security, education/knowledge, or land use;
   - no moral score or deterministic national personality.
4. **Alternative problem solutions**
   - at least a few shared pressures with multiple feasible physical responses, such as food security, crossing a barrier, storage, defense, or production;
   - capability and adoption representation only as deep as the selected alternatives require.
5. **Collective decisions and succession continuity**
   - priority selection, leadership/assembly action, blocked orders, and continuity when leadership changes at the approved thin depth.
6. **Matched-world divergence laboratory**
   - same or controlled terrain/resources, changed society parameters, multi-seed distributions, traces, and visual comparison;
   - isolate causal differences from uncontrolled randomness.
7. **Comparison and history**
   - compare layout, production, access, project choices, crisis response, and major events with links to the responsible conditions and decisions.

### Integrated proof

Run two or more societies through matched pressure suites. They use the same authoritative primitives but develop visibly different settlement forms, priorities, or solutions. Introduce the same shortage or route damage and show that their responses differ for traceable reasons. Remove the contrast variables and confirm the difference narrows rather than persisting through hidden scripts.

### Evidence required

- declared variable-to-decision effect map;
- matched-world and multi-seed divergence metrics;
- traces showing geography/resource/institution/history contributions;
- layout and response comparisons;
- save/replay through leadership or collective-priority changes;
- anti-stereotype/anti-script adversarial review;
- creator acceptance and D06 record.

### Decisions opened

- D06 number of societies, contrast variables, and thin institutional/government abstraction.

### Decisions kept closed

Full government composition, large culture catalog, factions, religion, complete technology hypergraph, broad diplomacy, war, full historical perspectives, and final society count for every world preset.

### Exit and handoff

Milestone 0.6 receives multiple distinct societies, comparable state/read models, and stable causal differences that can be connected through physical exchange and migration.

## 10. Milestone 0.6 — Connected Region

### Intent

Make society outcomes interdependent through physical routes, shipments, exchange, shortages, agreements, and migration.

### Entry conditions

- At least two accepted society configurations function independently.
- Route capacity, resource location, inventories, projects, and settlement responses are authoritative and bounded.
- D07 is opened after identifying which exchange abstraction is sufficient to create legible dependence.

### Core workstreams

1. **Contact and recognition**
   - discovery/contact state, communication reach, basic believed information, and sparse relationships only for relevant actors.
2. **Shipments and corridors**
   - stable shipment/manifests, origin/destination inventory, capacity, departure/arrival, current segment, risk/failure, and bounded route reuse;
   - no per-item or per-cargo global pathfinding.
3. **Exchange/allocation abstraction**
   - choose the minimum approved mix of direct exchange, institutional allocation, simple orders, or prices;
   - preserve physical availability, travel delay, ownership/controller, and conservation.
4. **Basic agreements and restrictions**
   - only the actions required for trade/access/pact/restriction and peaceful dispute in the selected scenario;
   - obligations and breaches are executable state, not flavor text.
5. **Migration and displacement precursor**
   - individuals/households evaluate destination opportunity, shelter, route, safety, and ties at a bounded depth;
   - migrants remain persistent people carrying approved skills/knowledge/history.
6. **Regional information**
   - route, shipment, stockpile, shortage/surplus, contact, and migration views;
   - true state for the player and believed state only where required by implemented AI.
7. **Causal bridge/corridor scenario**
   - a selected critical connection whose failure creates delayed and location-specific effects.

### Integrated proof

Two societies become materially dependent through an actual corridor. Interrupt it through damage, restriction, or environmental pressure. Shipments stop or reroute; local stocks and production change after causal delay; each society selects a response; migration, repair/replacement projects, agreements, coercive pressure, or adaptation follow; the complete chain remains inspectable after save/load.

### Evidence required

- origin/manifest/route/destination conservation tests;
- route failure, detour, and bounded cache invalidation;
- exchange/allocation convergence or stability evidence at the approved depth;
- migration persistence and membership updates;
- agreement/obligation state and breach tests;
- integrated causal-chain timeline and player explanation;
- performance with the accepted number of shipments/relationships;
- creator acceptance and D07 record.

### Decisions opened

- D07 trade, allocation, price, and route complexity.

### Decisions kept closed

Full markets/credit, all diplomatic actions, alliance systems, espionage, warfare, occupation, global logistics, final UI roster, and Story Director.

### Exit and handoff

Milestone 0.7 receives a connected region in which civilian logistics, relationships, migration, and route dependence are mature enough for conflict to have systemic consequences.

## 11. Milestone 0.7 — Conflict and Recovery

### Intent

Add limited physical conflict only after normal civilian life, supply, routes, buildings, and migration are reliable.

### Entry conditions

- The connected-region gate passes, including physical shipments, route failure, and migration.
- People, households, workplaces, and settlements can absorb loss and recover through ordinary systems.
- D08 is opened with performance and tone evidence.

### Core workstreams

1. **Mobilization and organization**
   - approved militia/guard or small combat-group model;
   - recruitment removes persistent people from civilian roles and creates household/workplace consequences.
2. **Equipment and supply**
   - broad weapon/ammunition/equipment/supply classes at the minimum approved depth;
   - route and stockpile dependence, readiness loss, and foraging/requisition only if selected.
3. **Operations and movement**
   - bounded goals such as raid, defend, seize a route, or attack a settlement;
   - strategic route movement and local encounter sectors rather than unrestricted tactical planning.
4. **Encounter resolution and presentation**
   - group/squad-level authoritative resolution, morale, retreat, wounds, deaths, capture only if approved, and allocation back to named people;
   - stylized, readable effects that do not own outcomes.
5. **Civilian response**
   - shelter, flight, displacement, temporary housing, return, and loss of labor/knowledge;
   - no fabricated refugees or deleted people.
6. **Control, peace, and recovery**
   - limited territorial/settlement control or occupation only at the approved depth;
   - peace/demobilization, salvage, repair, rebuilding, and persistent grievances/history.
7. **Conflict inspection and history**
   - goals, authority, supply, encounter cause, casualties, displacement, damage, and peace terms linked in factual records.

### Integrated proof

A traceable political or resource pressure creates an approved conflict. Persistent people mobilize, supplies move, a physical encounter occurs, named casualties and building/route damage are produced, civilians move or shelter, and official peace or withdrawal follows. The world then attempts recovery. Ending combat must not restore workers, homes, stockpiles, trust, or infrastructure automatically.

### Evidence required

- authority/goal and believed-information tests;
- supply/readiness and route dependence;
- deterministic encounter replay and casualty conservation;
- civilian labor, household, displacement, and shelter effects;
- damage, salvage, peace, and rebuilding continuity;
- presentation/accessibility review against D08;
- performance at the accepted active-sector/combatant tier;
- creator acceptance and D08 record.

### Decisions opened

- D08 conflict scale, violence presentation, occupation depth, and recovery boundary.

### Decisions kept closed

Detailed military hierarchy, full projectile physics, artillery/naval/air breadth, deep occupation/insurgency, complex prisoners, all treaty clauses, later-era weapons, and final release combat content count.

### Exit and handoff

Milestone 0.8 receives an integrated regional simulation in which player intervention can meaningfully affect settlement, exchange, diplomacy, conflict, displacement, recovery, and history.

## 12. Milestone 0.8 — God Sandbox

### Intent

Complete the defining player interaction loop over the systems that actually exist in the lean Version 1.0.

### Entry conditions

- Conflict and recovery pass without requiring hidden player assistance.
- Major decisions and events already produce factual traces and stable references.
- D09 is opened from usability tests over the integrated systems, not from the full Blueprint UI/power catalogs.

### Core workstreams

1. **Omniscient navigation and inspection**
   - search, select, follow, track, compare, and locate implemented people, households, buildings, routes, settlements, societies, shipments, conflicts, and events;
   - no implemented authoritative category is fundamentally hidden from the default player.
2. **Causal explanation**
   - one-line cause and factor view for the implemented high-value decisions and failures;
   - link blockers, observations, policies, resources, routes, and prior events.
3. **Binding Orders**
   - representative decision-level objectives over implemented settlement, exchange, relationship, migration, and conflict systems;
   - receiving authority is obligated, but exact site/route/staffing/logistics remain simulated and blockers remain visible.
4. **Indirect interventions**
   - modify implemented physical, biological, social, or informational conditions and let ordinary systems propagate consequences.
5. **Force Outcomes**
   - explicit validated mutation paths over approved entity categories;
   - preview affected records, reconcile memberships/ownership/inventories/routes, record bypassed rules, and retain aftermath.
6. **Intervention history and reversal**
   - permanent intervention ancestry;
   - undo as a recorded compensating command or save branch, never silent deletion.
7. **Consolidated UI and accessibility**
   - organize capabilities into the smallest coherent set of workspaces/panels chosen through D09;
   - keyboard/input abstraction, scalable text, non-color encodings, reduced motion/effects, and pause-and-inspect.
8. **Major history and ruins**
   - factual timeline, event links, and protected physical remnants needed to understand player-created and natural consequences;
   - advanced chronicles/maps remain optional post-1.0 depth.

### Integrated proof

Use the same regional crisis three ways: issue a Binding Order, apply an indirect intervention, and invoke a Force Outcome. Show the different validation, implementation, blockers, immediate mutations, downstream consequences, and history records. Save/load after each path and compare branches. The player must be able to explain what changed and why.

### Evidence required

- command-class authority and validation tests;
- reconciliation/invariant tests for each Force Outcome category;
- intervention ancestry and branch/undo behavior;
- explanation coverage for agreed major decisions/events;
- UI task-completion and accessibility review;
- clutter/performance measurements for search, overlays, traces, and history;
- creator acceptance and D09 record.

### Decisions opened

- D09 Version 1.0 power roster, UI organization, and explanation depth.

### Decisions kept closed

The full 24-power catalog, 13-workspace target, every overlay, full belief-comparison UI, Story Director profiles, historical maps, large chronicle library, and post-1.0 content allocation.

### Exit and handoff

Milestone 0.9 receives the complete lean gameplay loop and a measured list of release-critical polish, content, performance, persistence, usability, and support defects.

## 13. Milestone 0.9 — Release Candidate

### Intent

Stabilize, explain, package, and support the integrated god sandbox as a coherent standalone Version 1.0.

### Entry conditions

- Milestone 0.8 passes the identity and interaction gate.
- All implemented systems have explicit owners, save semantics, tests, and known limitations.
- D10 is opened from integrated measurements and external playtest evidence.

### Core workstreams

1. **Release-scope manifest and content freeze**
   - record exact supported world/population scale, content catalogs, powers, UI surfaces, save horizon, hardware, platforms, and known exclusions;
   - remove or clearly label incomplete experimental paths.
2. **Onboarding and information quality**
   - world creation, first observation, selection, explanation, intervention, time control, alerts, tracking, and save/return flow;
   - notification aggregation and failure explanations.
3. **Accessibility and input**
   - complete the approved baseline, test core flows without mouse-only assumptions, and document limitations.
4. **Visual and content consistency**
   - replace release-critical placeholders, verify readability/occlusion/LOD, construction/damage states, and restrained effects;
   - avoid final-art expansion that does not improve the release loop.
5. **Persistence and support hardening**
   - schema migrations, interrupted-save recovery, corruption diagnostics, backup restore, support bundle, and safe world continuation.
6. **Integrated verification**
   - deterministic command-log replay, multi-seed suites, accelerated simulation, wall-clock soak, save-growth/compaction, performance/memory/routing, trace volume, and clean package install at the D10-approved envelope.
7. **External review**
   - structured new-player playtest, independent architecture/behavior review, issue triage, and regression closure.
8. **Packaging and release operations**
   - build manifest, release notes, licenses/attributions, known limitations, rollback, support process, and creator release decision.

### Integrated proof

A clean user environment installs or runs the release candidate, creates a world, observes autonomous founding and divergence, follows interdependence/conflict/recovery, uses each player-control class, understands major causes, saves, exits, restores, and continues. The approved long-run and failure-recovery suites pass without developer intervention.

### Evidence required

- final scope manifest and decision references;
- onboarding/usability and accessibility results;
- release replay, seed, accelerated-run, soak, save-growth, migration, interruption, corruption, and performance reports;
- clean package/install test and build manifest;
- external playtest findings and closure ledger;
- independent review, creator acceptance, and release/hold decision.

### Decisions opened

- D10 exact supported scale, stress limits, save horizon, test/soak durations, reference/minimum hardware, platform/distribution target, release content lock, and commercial/brand choices that are genuinely required to distribute.

### Decisions kept closed

Version 1.1-1.5 feature allocation, industrial/later ages, and any system not necessary for the accepted Version 1.0 product.

### Exit and handoff

A passing 0.9 build may be tagged Version 1.0. The post-release review then opens D11 and selects the first bounded depth release from the Version 1.5 backlog.

## 14. Critical path and dependency rules

The critical path is:

`0.1 deterministic/persistent shell -> 0.2 measured engine and decisions -> 0.3 living camp -> 0.4 durable settlement -> 0.5 causal divergence -> 0.6 regional interdependence -> 0.7 conflict/recovery -> 0.8 god-sandbox loop -> 0.9 release hardening -> 1.0`

Work may run in parallel only when:

- both tasks depend on an accepted stable interface;
- neither task opens or assumes an unresolved creator decision;
- their authoritative write sets do not overlap without an explicit integration plan;
- each task has independent fixtures and rollback;
- integration is scheduled before either task can be considered complete.

Do not parallelize high-risk authoritative schema, scheduler, save, decision-runtime, route, or command-boundary changes merely to increase activity.

## 15. How later task contracts are created

For Milestones 0.4-0.9:

1. finish the prior milestone evidence;
2. complete a milestone gate review using `docs/templates/MILESTONE_GATE_TEMPLATE.md`;
3. open only the decision IDs listed for the next milestone;
4. approve a bounded milestone scope envelope;
5. decompose work into small contracts with explicit dependencies, tests, rollback, exclusions, and decision references;
6. update `Config/task-registry.json`, `TASK_INDEX.md`, and `STATUS_BOARD.md`;
7. implement one accepted task at a time;
8. integrate and run the milestone gate before planning the following milestone.

Task numbers after TASK-025 are intentionally unassigned until this process occurs.

## 16. Version 1.0 release identity test

Version 1.0 is a coherent standalone game only when the player can observe this full loop:

`named people -> physical founding -> durable settlement -> societal divergence -> regional dependence -> conflict or crisis -> displacement/recovery -> player direction/intervention/force -> inspectable persistent history`

The exact scale and content breadth are not identity tests. A feature belongs in Version 1.0 only when it materially strengthens this loop and fits the accepted release envelope better than the work it displaces.

## 17. Change control

- Changes to release labels or identity guardrails require an accepted ADR.
- Changes to a milestone envelope require a gate amendment with evidence and creator approval.
- A failed gate may reduce scope, repeat a bounded optimization/design pass, replace an implementation path, or stop the project. It may not be hidden by weakening tests after the fact.
- Version 1.5 remains the complete early-modern backlog unless a later creator-approved revision changes it.
