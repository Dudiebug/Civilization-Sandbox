# com.civsandbox.telemetry

## Owns
System timing, backlog, allocations, memory, query counts, candidate counts, trace volume, save metrics, performance reports, and diagnostic state diff integration.

## Forbidden
No gameplay decisions, no retail collection beyond approved policy, no telemetry-driven hidden state mutation.

## Dependency rule
Depends on simulation.core diagnostics and explicit instrumentation interfaces; domains must not depend on report rendering.

## Change requirements
- Read the active task and relevant behavior/data contracts.
- Preserve stable IDs, determinism, save compatibility, and explicit units.
- Add positive, negative, boundary, replay/save, and performance evidence appropriate to the change.
- Do not expose mutable internal collections across package boundaries.
- Update package documentation and architecture tests with any public API or dependency change.
