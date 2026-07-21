# Source and Change Notes

## Source authority

The included `blueprint/Civilization_Sandbox_Game_Development_Blueprint_v2.0.pdf` is the immutable product-vision source. `VISION_AUTHORITY.md` and `SCOPE_COVERAGE.md` make that authority operational. If this roadmap summary and the Blueprint conflict, the Blueprint wins unless the creator explicitly approves a recorded vision change.

## What changed

The earlier plan organized work mainly as many small governance, architecture, and subsystem tasks. This kit reorganizes delivery into 21 playable builds. Each task file is a direct Codex prompt that tells the agent to inspect the current game, propose a small set of playable implementation slices, implement one slice at a time, verify it in Unity, and preserve rollback points.

Architecture, tests, determinism, persistence, CI, documentation, and performance work still exist. They are implemented just in time inside the playable behavior that needs them instead of becoming long stretches with no game to touch.

## What did not change

- The core identity and authority model.
- Named people and physical founding.
- Durable settlements, regional interdependence, divergence, conflict, recovery, powers, and factual history.
- The complete early-modern Version 1.5 target.
- Industrial, electrified, mechanized, atomic/information, divergent-future, orbital, and multi-planet direction.
- Blueprint non-goals and safety boundaries.

No scoped vision area was intentionally deleted. Later ordering and exact content counts remain evidence- and creator-governed, as the Blueprint already requires.

## Working method

The workflow follows experienced AI-assisted development practice: maintain a working state, make bounded end-to-end changes, run the result, inspect diffs, commit good states, and revert failed directions early. The workflow is adapted for Unity and for a solo creator directing Codex through prompts.
