# com.civsandbox.ui

## Owns
Immutable query models, HUD, inspectors, search, overlays, comparisons, traces, history, orders, Force Outcome previews, accessibility, and input mapping.

## Forbidden
No direct ECS writes, no hidden authoritative state, no color-only meaning, no command bypass.

## Dependency rule
Depends on read-model/query services and validated command boundary; may depend on presentation for selection/camera interfaces.

## Change requirements
- Read the active task and relevant behavior/data contracts.
- Preserve stable IDs, determinism, save compatibility, and explicit units.
- Add positive, negative, boundary, replay/save, and performance evidence appropriate to the change.
- Do not expose mutable internal collections across package boundaries.
- Update package documentation and architecture tests with any public API or dependency change.
