# com.civsandbox.world

## Owns
World seed/version, authoritative coordinates, terrain fields, hydrology, climate, soil, resources, deposits, hazards, chunks, spatial indexes, and world validation.

## Forbidden
No rendering meshes as source of truth, no settlement decisions, no camera-dependent generation.

## Dependency rule
Depends on simulation.core; exposes read-only field/query APIs and domain events.

## Change requirements
- Read the active task and relevant behavior/data contracts.
- Preserve stable IDs, determinism, save compatibility, and explicit units.
- Add positive, negative, boundary, replay/save, and performance evidence appropriate to the change.
- Do not expose mutable internal collections across package boundaries.
- Update package documentation and architecture tests with any public API or dependency change.
