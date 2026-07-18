# Runtime Package Instructions

Each child folder is a planned Unity package/assembly boundary. Keep authoritative domain state out of `Assets/` scene scripts. A package may depend only on explicitly documented lower-level packages. Cross-domain behavior uses commands, events, stable IDs, read models, or narrow service interfaces—not arbitrary component access.

Before adding a dependency, update the package map and architecture tests. Circular dependencies are forbidden. Package names and exact manifests are finalized during TASK-001/TASK-003.
