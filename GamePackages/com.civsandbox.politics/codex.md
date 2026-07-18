# com.civsandbox.politics

## Owns
Culture tendencies, religion modules, patriotism/identity, government composition, law, policy/enforcement, legitimacy, factions, succession, reform, coups, rebellion, and civilization formation.

## Forbidden
No static moral scores, inherited ideology, unbounded faction planning, or direct personal movement/inventory mutation.

## Dependency rule
Depends on simulation.core, ai.runtime, people/households, settlements, economy, knowledge, and history through explicit interfaces.

## Change requirements
- Read the active task and relevant behavior/data contracts.
- Preserve stable IDs, determinism, save compatibility, and explicit units.
- Add positive, negative, boundary, replay/save, and performance evidence appropriate to the change.
- Do not expose mutable internal collections across package boundaries.
- Update package documentation and architecture tests with any public API or dependency change.
