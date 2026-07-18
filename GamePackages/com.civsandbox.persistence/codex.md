# com.civsandbox.persistence

## Owns
Semantic save container, manifests, domain chunks, migrations, atomic write/backup, corruption diagnostics, stable-reference resolution, and cache rebuild orchestration.

## Forbidden
No raw ECS memory, scene objects, transient handles, presentation caches, or silent lossy migration.

## Dependency rule
Depends on simulation.core DTO contracts and registered domain serializers; runtime domains must not depend on concrete file I/O.

## Change requirements
- Read the active task and relevant behavior/data contracts.
- Preserve stable IDs, determinism, save compatibility, and explicit units.
- Add positive, negative, boundary, replay/save, and performance evidence appropriate to the change.
- Do not expose mutable internal collections across package boundaries.
- Update package documentation and architecture tests with any public API or dependency change.
