# Prompt - Build 01: World Viewer

Build the first playable Unity simulation toy for Civilization Sandbox. Read the Blueprint sections relevant to authoritative time, stable identity, camera independence, 2.5D presentation, people, omniscience, and testing, plus `../VISION_AUTHORITY.md`.

Player outcome: I can open one scene, move and zoom the camera, pause and change simulation speed, watch a seeded group of 20-30 visible named people move through a simple bounded space, click one person, and inspect name, position, current action, and simulation time.

Plan 3-6 vertical slices and implement them one at a time. Add only the minimum fixed simulation clock, stable person identity, deterministic seed/reset, simple movement, read-only presentation snapshot, camera/time controls, selection, and inspector required for that outcome. Placeholder visuals are expected.

After every slice, compile, run the scene, and keep a working commit. Add focused tests for deterministic reset and camera independence; avoid a general test platform.

Acceptance: identical seed/reset produces identical initial people and checkpoints; moving or hiding the camera does not change authoritative state; selection never mutates the simulation; the scene remains responsive and understandable.

Do not add hunger, resources, construction, households, complex pathfinding, final art, broad saves, or future society systems.
