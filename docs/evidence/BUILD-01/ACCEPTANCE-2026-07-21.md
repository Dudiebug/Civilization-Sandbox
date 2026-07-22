# Creator Acceptance — BUILD-01 — 2026-07-21

**Creator acceptance:** APPROVED

## Accepted candidate

- Branch: `build/01-world-viewer`.
- Candidate commit at the time of approval: `6eca7a2`.
- The creator explicitly approved Build 01 after requesting that the rejected sprite and clothing revision be reverted.

## Accepted player experience

- One bounded World Viewer scene with 24 deterministic placeholder people.
- Camera pan and zoom, pause and speed controls, deterministic seed reset, selection, and read-only inspection.
- Names remain available through selection and the inspector; they do not float above people.
- Placeholder people may wait and walk. Hunger, resources, construction, households, pathfinding, saves, and society systems remain excluded.

## Rejected direction excluded from approval

- Commits `1949431` and `a94da6c` were reverted by `11032e8` and `2adce73`.
- The modular clothing model, generated wardrobe concept, clothing inspector fields, and revised sprites are not part of the accepted Build 01 candidate.
- Sprite and clothing development remains paused.

## Verification status at acceptance

- The committed pre-art World Viewer evidence records a passing repository audit, 25 focused EditMode tests, deterministic replay and camera-independence checks, Windows player build, and first-frame smoke test at verified commit `36218a7`.
- The repository-only audit passed again after rollback on 2026-07-21.
- Unity automatically reloaded the rollback and reported no C# compilation failure in the active editor.
- A fresh headless verification run could not acquire the project because the creator had the same project open in Unity. No new automated-pass claim is made for commit `6eca7a2`; rerun `Build/Verify-WorldViewer.ps1` after closing the editor to refresh that evidence.

## Creator direction

The creator stated: “revert the sprite changes you did and i aprove build 01”. This file records that explicit decision; it is not inferred acceptance.
