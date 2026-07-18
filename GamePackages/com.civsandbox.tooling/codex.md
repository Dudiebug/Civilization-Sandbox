# com.civsandbox.tooling

## Owns
Editor/content authoring, validators, previews, fixture builders, benchmark launchers, evidence packaging, and migration tools.

## Forbidden
No production runtime authority, no bypassing validation to write live saves/content, no hidden manual repair steps.

## Dependency rule
Depends on content/schema, persistence tooling APIs, and test/telemetry reports; editor-only dependencies remain isolated.

## Change requirements
- Read the active task and relevant behavior/data contracts.
- Preserve stable IDs, determinism, save compatibility, and explicit units.
- Add positive, negative, boundary, replay/save, and performance evidence appropriate to the change.
- Do not expose mutable internal collections across package boundaries.
- Update package documentation and architecture tests with any public API or dependency change.
