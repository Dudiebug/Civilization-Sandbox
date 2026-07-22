# BUILD-01 Evidence Pack

**State:** IMPLEMENTED — CREATOR ACCEPTANCE PENDING

## Scope

- Task contract: `docs/plans/tasks/BUILD-01_WORLD_VIEWER.md`.
- Authority: `docs/plans/VISION_AUTHORITY.md`, `docs/plans/VIBE_WORKFLOW.md`, and the relevant simulation, people, 2.5D presentation, omniscience, and testing Blueprint sections.
- Implemented slices: seeded world frame; deterministic movement and time; camera exploration; read-only person inspection; integrated reliability and verification.
- Player-facing era: fictional early-modern, roughly 1700s-equivalent. Names, restrained cloth colors, timber boundary, and parchment-like UI are placeholders consistent with that era.

## Environment

- Candidate branch: `build/01-world-viewer`.
- Verified implementation commit: `36218a7cb63000f08be44e41e331147d7c78deee`.
- Project: `C:\Users\dudie\Projects\Civilization-Sandbox`.
- Reference machine: Windows 11 Pro, Ryzen 5 7600X, 31.46 GiB RAM, RTX 5070; see `Config/benchmark-reference.json`.
- Unity: `6000.3.20f1` (`c9ba695d4f07`).
- Package-lock SHA-256: `1fd6fdd06973bb8aeb25f8897e01090128be10f4f37d65410293f32e6c55efdc`.
- Reference scenario: seed `170601`, 24 persistent named people, bounded 44 m by 44 m prototype space.

## Required results

- Build: **PASS** — `powershell -NoProfile -ExecutionPolicy Bypass -File Build/Verify-WorldViewer.ps1 -ResultPath Artifacts/results/build01-final-verify.json`; Windows player at `Artifacts/build/world-viewer/CivilizationSandboxWorldViewer.exe`.
- Tests: **PASS** — 25 focused EditMode tests: 7 people/simulation tests and 18 World Viewer clock, camera, selection, reset, presentation-lifecycle, and session-boundary tests. Reports: `Artifacts/tests/world-viewer-people.xml` and `Artifacts/tests/world-viewer-presentation.xml`.
- Player smoke: **PASS** — freshly built Windows player reached its first frame with 24 people and emitted `CIV-BUILD01-SMOKE-000` before a clean headless exit.
- Replay/state diff: **PASS** — same-seed reset and equal-world-time schedules produce equal canonical checksums; retained snapshots remain detached; moving, zooming, or hiding the camera and selecting/switching/clearing people do not change authoritative checksums.
- Persistence/migration: **N/A** — Build 01 explicitly excludes saves and introduces no save schema or migration surface.
- Performance: **PASS for Build 01 scope** — the non-authoritative pump processes at most 8 fixed steps per presentation frame and retains at most 40 queued steps; overload and discarded complete backlog are reported in logs and the HUD. Repeated seed-reset coverage holds presenters at 24 and shared visual assets at 12 sprite variants and four prototype material colors. No Version 1.0 scale budget is claimed.
- Repository audit: **PASS** — `Build/Bootstrap.ps1 -RepositoryOnly` verified the pinned editor revision, package-lock hash, and direct manifest.
- Documentation: **PASS** — Build command documentation, package map, evidence, file manifest, document validation, and plan validation are current.
- Independent review: **PASS** — adversarial findings were corrected and re-checked; see `REVIEW.md`.
- Creator acceptance: **PENDING** — see `ACCEPTANCE.md`. This state is not marked on the creator's behalf.

The clean verification result records commit `36218a7cb63000f08be44e41e331147d7c78deee`, `dirty: false`, Unity `6000.3.20f1`, 7 passing people tests, 18 passing World Viewer tests, the player artifact hash, and zero exit status.

## Decision audit

- Decisions opened: none. Build 01 used the fixed 20 Hz reference architecture and the creator's early-modern era direction.
- Decisions explicitly deferred: final art direction; accepted population/performance scale; save representation; all needs, resources, construction, households, pathfinding, and society systems.
- Assumptions that remain provisional: 24 people, the 44 m prototype boundary, seed `170601`, 8 steps of maximum per-frame catch-up work, and 40 retained backlog steps are Build 01 values, not release-scale commitments.

## Changed surface

- Engine-free simulation core: authoritative clock, bounded wall-time pump, speeds, stable IDs, keyed randomness, and canonical checksums.
- World and people packages: integer millimetre coordinates, bounds, seeded named people, deterministic movement, and detached read-only snapshots.
- Presentation and UI packages: early-modern prototype billboards, bounded field, camera, selection, inspector, time/seed controls, and clock-health status.
- Unity composition: one `WorldViewer` scene, runtime session/root, editor setup/build command, focused tests, and the Build 01 verifier.

## Known limitations and deferred work

- People only wait and walk directly toward deterministic targets; there is no pathfinding or collision avoidance.
- The world is a deliberately small bounded prototype field with generated pixel placeholders, not final terrain, buildings, animation, or art.
- The inspector exposes only name, stable ID, integer position, current action, and world time.
- No hunger, inventory, resources, construction, households, relationships, society simulation, persistence, replay file, or future-release system was added.
- The fixed-step pump preserves responsiveness by explicitly skipping only bounded complete wall-time backlog when necessary; it never claims real-time catch-up after an overload.

## Rollback

Revert the five Build 01 slice commits newest-first (`36218a7`, `8cfa05f`, `928997f`, `de4edfd`, `f728b2e`). Generated `Artifacts/` outputs are ignored and can be removed with the repository's guarded cleanup workflow. The accepted foundation on `main` remains the recovery baseline.

## Final declaration

Build 01 implementation, automated verification, and independent review pass. The build remains open only for the creator's explicit Unity playtest decision in `ACCEPTANCE.md`.
