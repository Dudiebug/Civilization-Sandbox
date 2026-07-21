# Assets Scope

This folder is a provisional Unity integration boundary and becomes active only if Milestone 0.1 accepts the Unity path.

If active, use `Assets/` only for engine integration, scenes, presentation assets, addressable/editor resources, and minimal composition roots that cannot live in packages. Authoritative domain logic and state belong in runtime domain packages.

Never place game rules in scene scripts, mutable editor assets used as runtime truth, animation events, scene object names, or inspector-only configuration. All production assets require stable descriptor mapping and validation.
