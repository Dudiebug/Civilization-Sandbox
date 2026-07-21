# Creator Acceptance — BUILD-01

**State:** PENDING

## What I should see

- One angled 2.5D early-modern prototype field containing 24 visible, named pixel people who wait and walk.
- Camera pan and zoom that never change the simulated world.
- Pause, 1x, 2x, 5x, and 10x controls plus deterministic seed reset.
- A read-only omniscient inspector showing the selected person's name, stable ID, position, current action, and world time.
- A clock-health line that stays steady in ordinary play and clearly reports bounded catch-up or skipped wall time if the player is overloaded.

## Test steps

1. Open `C:\Users\dudie\Projects\Civilization-Sandbox` with Unity `6000.3.20f1`.
2. Open `Assets/WorldViewer/WorldViewer.unity` and press Play.
3. Pan with WASD, arrow keys, or middle-drag; zoom with the mouse wheel. Confirm people continue independently when the camera moves or is looking elsewhere.
4. Try Pause, 1x, 2x, 5x, and 10x. Confirm world time stops only on Pause and movement accelerates at higher speeds.
5. Click several people. Confirm the inspector switches cleanly and that selection itself does not move or alter anyone.
6. Enter seed `170601`, reset, observe the initial company, change to another seed, then return to `170601`. Confirm the original names and starting arrangement return without ghost people.

## Result

- [ ] Behavior matches the Build 01 task contract.
- [ ] The early-modern placeholder presentation feels directionally correct.
- [ ] Camera, time, selection, inspection, and reset controls are understandable.
- [ ] No obvious unrelated regression is visible.
- [ ] I approve the tradeoff and player experience.

**Decision:** PENDING

**Notes:** Reply `APPROVE`, `CHANGES`, `STOP`, or `ROLL BACK` after the playtest.
