# Creator Acceptance — BUILD-01

**State:** APPROVED

## What I should see

- One angled 2.5D early-modern prototype field containing 24 visible pixel people who wait and walk in varied layered clothing.
- Camera pan and zoom that never change the simulated world.
- Pause, 1x, 2x, 5x, and 10x controls plus deterministic seed reset.
- No names floating above people. A read-only omniscient inspector shows the selected person's name, stable ID, position, current action, world time, and clothing.
- A clock-health line that stays steady in ordinary play and clearly reports bounded catch-up or skipped wall time if the player is overloaded.

## Test steps

1. Open `C:\Users\dudie\Projects\Civilization-Sandbox` with Unity `6000.3.20f1`.
2. Open `Assets/WorldViewer/WorldViewer.unity` and press Play.
3. Pan with WASD, arrow keys, or middle-drag; zoom with the mouse wheel. Confirm people continue independently when the camera moves or is looking elsewhere.
4. Try Pause, 1x, 2x, 5x, and 10x. Confirm world time stops only on Pause and movement accelerates at higher speeds.
5. Click several people. Confirm the inspector switches cleanly and that selection itself does not move or alter anyone.
6. Enter seed `170601`, reset, observe the initial company, change to another seed, then return to `170601`. Confirm the original names and starting arrangement return without ghost people.
7. Confirm clothing uses crisp 32 x 48 pixel layers and plausible early-modern pieces; reset seed `170601` and confirm the same outfits return.

## Result

- [x] Behavior matches the Build 01 task contract.
- [x] The early-modern placeholder presentation is acceptable for this stage.
- [x] Camera, time, selection, inspection, and reset controls are understandable.
- [x] No obvious unrelated regression is visible.
- [x] I approve the tradeoff and player experience for Build 01.

**Decision:** APPROVE

**Notes:** Creator approval recorded. The current character models are visually rough and have no meaningful animation, but this is accepted because Build 01 is the first simulation viewer and character art/animation has not entered production scope yet. Treat improved character models and animation as later presentation work, not a blocker for Build 01.
