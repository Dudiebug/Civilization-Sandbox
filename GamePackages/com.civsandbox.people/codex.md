# com.civsandbox.people

## Owns
Persistent person identity and life state, health, needs, skills, work intent, traits, attitudes, beliefs, ties, experiences, position, and appearance descriptors.

## Forbidden
No exhaustive relationship graph, romance simulator, global searches, government decisions, or direct rendering assets.

## Dependency rule
Depends on simulation.core, ai.runtime, world query interfaces, and household membership interfaces.

## Change requirements
- Read the active task and relevant behavior/data contracts.
- Preserve stable IDs, determinism, save compatibility, and explicit units.
- Add positive, negative, boundary, replay/save, and performance evidence appropriate to the change.
- Do not expose mutable internal collections across package boundaries.
- Update package documentation and architecture tests with any public API or dependency change.
