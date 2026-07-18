# com.civsandbox.simulation.core

## Owns
Authoritative clock, stable IDs, units, fixed-point values, scheduler, keyed RNG, deterministic containers, command boundary primitives, and checksums.

## Forbidden
No Unity presentation, gameplay-domain policy, file-format implementation, or unmanaged global mutable state.

## Dependency rule
All domain packages; may depend only on minimal Unity Collections/Entities/runtime abstractions approved by ADR.

## Change requirements
- Read the active task and relevant behavior/data contracts.
- Preserve stable IDs, determinism, save compatibility, and explicit units.
- Add positive, negative, boundary, replay/save, and performance evidence appropriate to the change.
- Do not expose mutable internal collections across package boundaries.
- Update package documentation and architecture tests with any public API or dependency change.
