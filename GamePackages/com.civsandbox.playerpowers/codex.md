# com.civsandbox.playerpowers

## Owns
Validated player command classification, Binding Orders, indirect interventions, Force Outcome mutations, reconciliation, previews, warnings, undo-as-command/branch, and intervention history.

## Forbidden
No balance currency in default mode, no direct ECS mutation from UI, no bypass of invariants/history, no disguising intervention as natural history.

## Dependency rule
Depends on command APIs and reconciliation services from relevant domains; UI depends on read models only.

## Change requirements
- Read the active task and relevant behavior/data contracts.
- Preserve stable IDs, determinism, save compatibility, and explicit units.
- Add positive, negative, boundary, replay/save, and performance evidence appropriate to the change.
- Do not expose mutable internal collections across package boundaries.
- Update package documentation and architecture tests with any public API or dependency change.
