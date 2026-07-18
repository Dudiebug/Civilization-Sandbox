# com.civsandbox.history

## Owns
Historical event graph, typed causal edges, significance, perspective records, entity/place indexes, retention, compaction, landmark references, and query caps.

## Forbidden
No free-form unsupported facts, unbounded per-entity histories, or deletion of referenced canonical evidence.

## Dependency rule
Depends on simulation.core and stable domain references; consumed by story, persistence, UI, and tests.

## Change requirements
- Read the active task and relevant behavior/data contracts.
- Preserve stable IDs, determinism, save compatibility, and explicit units.
- Add positive, negative, boundary, replay/save, and performance evidence appropriate to the change.
- Do not expose mutable internal collections across package boundaries.
- Update package documentation and architecture tests with any public API or dependency change.
