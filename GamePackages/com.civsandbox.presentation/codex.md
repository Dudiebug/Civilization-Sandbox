# com.civsandbox.presentation

## Owns
Read-only snapshot consumption, 3D terrain/roads/buildings, camera-facing billboards, vehicles, effects, LOD, camera, occlusion, selection visuals, and debug overlays.

## Forbidden
No authoritative decisions, time ownership, inventory, AI, save truth, or mutation of simulation state.

## Dependency rule
Depends on presentation descriptors/read models and Unity rendering packages; simulation packages never depend on this package.

## Change requirements
- Read the active task and relevant behavior/data contracts.
- Preserve stable IDs, determinism, save compatibility, and explicit units.
- Add positive, negative, boundary, replay/save, and performance evidence appropriate to the change.
- Do not expose mutable internal collections across package boundaries.
- Update package documentation and architecture tests with any public API or dependency change.
