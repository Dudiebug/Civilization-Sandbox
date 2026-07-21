# Runtime Package Instructions

Each child folder is an accepted Unity package/assembly boundary under ADR-002. Keep the boundary focused on its registered active or reserved scope; do not create unregistered manifests or gameplay code outside an approved task.

If accepted, keep authoritative domain state out of `Assets/` scene scripts. A package may depend only on explicitly documented lower-level packages. Cross-domain behavior uses commands, events, stable IDs, read models, or narrow service interfaces - not arbitrary component access.

Package ownership descriptions include the Version 1.5 horizon. A package's existence is not permission to implement its full scope during Version 1.0. Before adding a dependency, update the package map and architecture tests. Circular dependencies are forbidden.
