# com.civsandbox.diplomacywar

## Owns
Contact, believed information, claims, treaties, diplomacy, war goals, mobilization, squads, operations, supply, sector encounters, occupation, refugees, and peace aftermath.

## Forbidden
No omniscient AI, per-projectile authoritative physics, tactical player micromanagement, or fabricated off-screen outcomes.

## Dependency rule
Depends on simulation.core, ai.runtime, politics, economy/logistics, settlements/routes, people, ecosystems/environment, and history.

## Change requirements
- Read the active task and relevant behavior/data contracts.
- Preserve stable IDs, determinism, save compatibility, and explicit units.
- Add positive, negative, boundary, replay/save, and performance evidence appropriate to the change.
- Do not expose mutable internal collections across package boundaries.
- Update package documentation and architecture tests with any public API or dependency change.
