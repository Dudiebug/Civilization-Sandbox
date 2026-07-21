# Vibe-First Roadmap Instructions

- Treat `../blueprint/Civilization_Sandbox_Game_Development_Blueprint_v2.0.pdf` as the immutable north-star vision.
- Read `VISION_AUTHORITY.md`, `SCOPE_COVERAGE.md`, and the selected build prompt before planning implementation.
- Work on one build prompt at a time. Split it into 3-8 playable vertical slices and implement one approved slice at a time.
- Keep Unity runnable after every accepted slice. Run the game, inspect the diff, test proportionally, and preserve a rollback commit.
- Architecture, tests, persistence, tooling, and documentation are just-in-time parts of the playable slice that needs them.
- Never silently delete, narrow, or reinterpret Blueprint scope. Record deferrals in the coverage matrix and ask the creator before a material vision change.
- The creator judges player-visible behavior and product direction. Codex owns implementation detail, automated verification, and concise handoff notes.
