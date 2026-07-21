# Prompt - Build 00: Compact Foundation

Implement the compact foundation for Civilization Sandbox using the included Blueprint and `../VISION_AUTHORITY.md` as authority.

Player outcome: the Unity project opens, compiles, launches a test scene, and can be safely changed through future Codex prompts. Preserve any already-accepted repository and documentation work, but do not expand infrastructure merely because the Blueprint describes future needs.

Inspect the current project before planning. Split this build into no more than five independently verifiable slices. Include only: pinned Unity/project dependencies, source control and recovery, one local build/test command, a deterministic seed convention, a minimal authoritative-simulation versus presentation boundary, and concise agent instructions.

After each slice, compile or launch the project and create a working commit. Stop and report if the selected Unity baseline cannot support headless tests, deterministic simulation separation, or the intended 2.5D presentation.

Acceptance: a clean checkout launches the same empty/test scene; a compile failure is actionable; camera/presentation code cannot own authoritative state; the repository can be restored to a known working commit.

Do not implement gameplay, a full package hierarchy, broad analyzers, semantic saves, future-domain schemas, or a large CI/evidence platform in this build.
