# com.civsandbox.settlements

## Owns
Settlement identity, parcels, land rights, projects, buildings, construction, services, neighborhoods, roads/infrastructure proposals, damage, abandonment, salvage, and recovery.

## Forbidden
No ordinary player manual placement, no goods/labor teleportation, no direct personal footstep control.

## Dependency rule
Depends on simulation.core, ai.runtime, world, people/household interfaces, economy/logistics interfaces, and history events.

## Change requirements
- Read the active task and relevant behavior/data contracts.
- Preserve stable IDs, determinism, save compatibility, and explicit units.
- Add positive, negative, boundary, replay/save, and performance evidence appropriate to the change.
- Do not expose mutable internal collections across package boundaries.
- Update package documentation and architecture tests with any public API or dependency change.
