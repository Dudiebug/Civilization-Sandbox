# Assets Scope

This folder is the accepted Unity integration boundary under ADR-002. It is active for engine integration, scenes, presentation assets, addressable/editor resources, and minimal composition roots that cannot live in packages.

If active, use `Assets/` only for engine integration, scenes, presentation assets, addressable/editor resources, and minimal composition roots that cannot live in packages. Authoritative domain logic and state belong in runtime domain packages.

Never place game rules in scene scripts, mutable editor assets used as runtime truth, animation events, scene object names, or inspector-only configuration. All production assets require stable descriptor mapping and validation.
