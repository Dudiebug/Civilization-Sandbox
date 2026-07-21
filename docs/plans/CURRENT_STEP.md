# Current Step

**Active task:** TASK-001 - Repository governance and reproducible bootstrap
**Status:** In Progress
**Milestone:** 0.1 - Project Foundation
**Model:** Implementation and verification role

## One action
Commit the creator-approved Linux SDK/toolchain package pins, then rerun bootstrap idempotency, Windows/Linux builds, the bootstrap test, and evidence packaging. Keep acceptance and recovery gates pending until independently observed.

## Decisions recorded
D01 uses Unity `6000.3.20f1` and the exact tool/package pins in `Config/toolchain.json`. D02 supports the pinned Windows host and records the creator reference hardware in `Config/benchmark-reference.json`.

Any future amendment to a locked interface requires an explicit creator question and approval before editing.

## Do not decide or build yet
Do not decide world size, population target, content counts, government models, economy depth, combat depth, power roster, UI layout, or release test duration. Do not create terrain, citizens, buildings, gameplay AI, art, persistence, replay, or player-facing APIs.

## Success for this step
Local governance and Unity checks pass with retained evidence; remote protection, clean-profile reproduction, independent review, and creator acceptance are explicitly recorded.
