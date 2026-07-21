# com.civsandbox.simulation.core

## Owns
Authoritative clock, stable IDs, units, fixed-point values, scheduler, keyed RNG, deterministic containers, command boundary primitives, and checksums.

## Forbidden
No engine presentation layer, gameplay-domain policy, file-format implementation, or unmanaged global mutable state.

## Dependency rule
All domain packages; may depend only on the minimal runtime/data abstractions accepted by the engine/toolchain ADR.

## Change requirements
- Read the active task and relevant behavior/data contracts.
- Preserve stable IDs, determinism, save compatibility, and explicit units.
- Add positive, negative, boundary, replay/save, and performance evidence appropriate to the change.
- Do not expose mutable internal collections across package boundaries.
- Update package documentation and architecture tests with any public API or dependency change.
