# BUILD-01 Adversarial Review

**Status:** PASS — independent review completed against implementation commit `36218a7cb63000f08be44e41e331147d7c78deee`.

## Review scope

The reviewer challenged deterministic time and speed schedules, stable identity, keyed randomness, camera and selection isolation, package ownership, bounded work/backlog, reset lifecycle, test relevance, early-modern presentation, and exclusion of future systems.

## Findings resolved before the gate

- Different-seed reset could retain stale person presenters. The scene view now removes stale IDs before applying a new snapshot, and a regression verifies exactly 24 active presenters.
- The render-frame runner either capped work with an unreported growing backlog or drained unbounded work. An engine-free fixed-step pump now caps each frame at 8 steps, retains at most 40 queued steps, and reports active/cumulative overload.
- Original camera and selection tests exercised standalone simulation objects rather than the session boundary used by the root. A testable `WorldViewerSession` now owns simulation, selection, speed, snapshots, and clock telemetry; camera and selection tests compare its authoritative checksum.
- Repeated reset created new native sprites and materials. The presentation now shares 12 sprite variants and color-keyed prototype materials; a 200-seed reset test verifies bounded cache counts and presenter count.
- Authoritative clock state and rate initially lived in the people package. `SimulationClock` now owns the 20 Hz rate, world time, and speed-step semantics in `simulation.core`; people compose it.
- Retained old-speed backlog could be replayed under a new speed. Speed changes now discard and report only complete queued old-speed steps while preserving sub-tick wall time. A rapid alternating-speed regression proves unpaused time still reaches complete ticks without loss.
- EditMode reset verification initially used unsafe deferred `Destroy`. Runtime presentation teardown now selects `Destroy` in play and `DestroyImmediate` outside play.

## Final verification

- Clean implementation commit: `36218a7cb63000f08be44e41e331147d7c78deee`.
- Focused verifier: PASS, zero exit status.
- People/simulation: 7 passed, 0 failed.
- World Viewer integration/presentation: 18 passed, 0 failed.
- Windows player build and first-frame smoke: PASS.
- Repository package/editor audit: PASS.

## Non-blocking follow-ups

- P3: improve name-label readability at the widest overview zoom.
- P3: add a PlayMode camera-independence test around the actual root/camera wiring when the pinned Entities Graphics test-runner teardown issue no longer makes that layer unreliable. The current session-boundary test and structural dependency check cover the Build 01 authority invariant.

## Verdict

PASS — the independent reviewer reported: “No blocking P1/P2 findings remain.” Deterministic time, stable IDs, keyed randomness, bounded scheduling, speed equivalence, camera/selection isolation, reset cleanup, package direction, era presentation, and Build 01 scope are acceptable. Creator acceptance remains pending.
