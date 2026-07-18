# com.civsandbox.economy

## Owns
Commodities, inventories, reservations, ownership, workplaces, recipes, labor demand, markets, prices, simple credit, shipments, manifests, and conservation diagnostics.

## Forbidden
No equilibrium fabrication of unavailable goods, microscopic coin/item simulation, or per-cargo global routing.

## Dependency rule
Depends on simulation.core, ai.runtime, world/route interfaces, people/work interfaces, and settlement asset interfaces.

## Change requirements
- Read the active task and relevant behavior/data contracts.
- Preserve stable IDs, determinism, save compatibility, and explicit units.
- Add positive, negative, boundary, replay/save, and performance evidence appropriate to the change.
- Do not expose mutable internal collections across package boundaries.
- Update package documentation and architecture tests with any public API or dependency change.
