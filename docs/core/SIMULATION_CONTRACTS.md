# Simulation Contracts

## Time and determinism
- World time: signed 64-bit simulation seconds from epoch.
- Base loop: 20 fixed wall ticks per second.
- Canonical event order: due time, priority, stable target ID, sequence key.
- Random address: world seed, stream family, stable ID, decision ID, occurrence counter.
- Camera, frame rate, visibility, and UI queries never affect authoritative results.

## Authority hierarchy
Alliance/international → civilization → government → region → settlement → institution/workplace → household → person.
Higher layers set strategy, law, budgets, and constraints. Lower layers own exact local execution.

## Decision sequence
Validate commitment → bounded candidates → knowledge-filtered observations → preconditions → canonical normalized considerations → traits/attitudes/policy → inertia/cooldown → keyed variation → stable selection → reservations/commitment → trace.

## Conservation and location
Goods have origins, owners, inventories, reservations, manifests, routes, travel time, and destinations. No teleportation or duplicate spending outside a declared Force Outcome source/sink.
