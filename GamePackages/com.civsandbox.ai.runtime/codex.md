# com.civsandbox.ai.runtime

## Owns
Canonical behavior-contract runtime: authority contexts, knowledge filtering, candidates, preconditions, utility curves, commitments, reservations, deterministic selection, budgets, and traces.

## Forbidden
No domain-specific omniscient shortcuts, live LLM calls, universal civilization brain, or presentation references.

## Dependency rule
Depends on simulation.core and explicit domain interfaces; domain packages depend on it only through versioned contracts.

## Change requirements
- Read the active task and relevant behavior/data contracts.
- Preserve stable IDs, determinism, save compatibility, and explicit units.
- Add positive, negative, boundary, replay/save, and performance evidence appropriate to the change.
- Do not expose mutable internal collections across package boundaries.
- Update package documentation and architecture tests with any public API or dependency change.
