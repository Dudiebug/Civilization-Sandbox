# com.civsandbox.households

## Owns
Co-residence, guardians/dependents, shared inventory/money/obligations, housing, migration, inheritance, and household decisions.

## Forbidden
No detailed romance/genealogy, no ownership bypass, no direct movement mutation.

## Dependency rule
Depends on simulation.core, ai.runtime, people IDs/interfaces, economy inventory interfaces, and settlement housing interfaces.

## Change requirements
- Read the active task and relevant behavior/data contracts.
- Preserve stable IDs, determinism, save compatibility, and explicit units.
- Add positive, negative, boundary, replay/save, and performance evidence appropriate to the change.
- Do not expose mutable internal collections across package boundaries.
- Update package documentation and architecture tests with any public API or dependency change.
