# Assets Scope

Use `Assets/` only for Unity project integration, scenes, presentation assets, addressable/editor resources, and minimal composition roots that cannot live in packages. Authoritative domain logic and state belong in `GamePackages/`.

Never place game rules in MonoBehaviours, ScriptableObjects used as mutable runtime truth, animation events, scene object names, or inspector-only configuration. All production assets require stable descriptor mapping and validation.
