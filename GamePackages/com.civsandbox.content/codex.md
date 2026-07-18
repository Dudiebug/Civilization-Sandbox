# com.civsandbox.content

## Owns
Versioned immutable definitions, namespaced stable IDs, units, recipes, curves, policies, actions, capabilities, visual descriptors, localization keys, and validators.

## Forbidden
No arbitrary executable code in data, no duplicate IDs, no undocumented units, no future-era content in active Version 1.0 milestones.

## Dependency rule
Depends on schema libraries only; runtime packages consume validated compiled definitions.

## Change requirements
- Read the active task and relevant behavior/data contracts.
- Preserve stable IDs, determinism, save compatibility, and explicit units.
- Add positive, negative, boundary, replay/save, and performance evidence appropriate to the change.
- Do not expose mutable internal collections across package boundaries.
- Update package documentation and architecture tests with any public API or dependency change.
