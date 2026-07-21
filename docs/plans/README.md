# Civilization Sandbox - Vibe-First Full Roadmap Kit

This kit preserves the complete Civilization Sandbox vision while replacing the original infrastructure-heavy execution order with playable vertical builds.

## Authority

1. `../blueprint/Civilization_Sandbox_Game_Development_Blueprint_v2.0.pdf` remains the north-star product specification.
2. `VISION_AUTHORITY.md` records the parts of the vision that may not be silently removed.
3. `FULL_ROADMAP.md` reorganizes delivery without shrinking the intended game.
4. `SCOPE_COVERAGE.md` maps the full scope to builds and later releases.
5. Files in `tasks/` are copy-paste prompts for Codex. They are playable build prompts, not giant one-shot implementation requests.

## How to use this kit

1. Read the Blueprint and `VISION_AUTHORITY.md` once.
2. Work on only the next build prompt.
3. Let Codex split that prompt into a few small implementation slices.
4. After every slice, run the game in Unity and judge the visible result.
5. Keep a working commit after each successful slice.
6. Update the next prompt from what the game teaches you. Do not pre-author hundreds of microtasks.

## What changed

- The full early-modern, later-age, and distant-expansion vision is preserved.
- Architecture, testing, persistence, and performance work is introduced just in time inside playable builds.
- Formal gates occur at playable-build and release boundaries rather than after every small AI change.
- The creator accepts observable behavior in Unity; Codex owns implementation details, automated checks, and rollback.

## Starting point

The existing repository/toolchain and source-of-truth work can remain as completed history. Start with `tasks/BUILD-01_WORLD_VIEWER.md` unless the project still needs the compact foundation prompt in `tasks/BUILD-00_FOUNDATION.md`.
