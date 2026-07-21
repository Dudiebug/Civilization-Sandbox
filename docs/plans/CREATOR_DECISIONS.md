# Creator Decisions

This file is not a questionnaire. Most decisions are intentionally unopened. Use `DECISION_QUEUE.md` for timing; ask only one decision at a time when its gate arrives.

## Existing preference decisions - no action required now
| ID | Decision area | Current planning posture | Decide no later than |
|---|---|---|---|
| C01 | Final title and brand | “Civilization Sandbox” and “Founding Worlds” are working labels only | Public announcement |
| C02 | Tone | Preserve serious systemic history with restrained humor unless changed later | Large speech/content production |
| C03 | Audio/music | Deferred | Release audio pass |
| C04 | Commercial model/price | Deferred | Store planning |
| C05 | Platform scope | Development host and first player builds decided separately | Milestone 0.9 |
| C06 | Modding/UGC | Deferred until schemas stabilize | Post-1.0 planning |
| C07 | Localization order | Localization-safe architecture now; final languages deferred | Release content lock |
| C08 | Accessibility beyond baseline | Baseline required; formal audit scope deferred | Milestone 0.9 |
| C09 | Violence ceiling | Stylized, low-detail direction retained; exact presentation decided at 0.7 | Milestone 0.7 |
| C10 | Release/community strategy | Deferred | Milestone 0.9 |

## Decisions recorded during TASK-001
- D01: Unity `6000.3.20f1` with the exact editor, module, package, and dependency-lock pins in `Config/toolchain.json`.
- D02: pinned Windows development host; creator reference hardware is recorded in `Config/benchmark-reference.json` and is informational rather than the later performance-gate machine.
- Repository: public GitHub repository `Dudiebug/Civilization-Sandbox`, protected `main`, task branches, and one authoritative project folder at `C:\Users\dudie\Projects\Civilization-Sandbox` as specified in `Config/repository-governance.json`. The creator approved public visibility on 2026-07-20 after GitHub returned HTTP 403 for branch protection while private, then directed Codex on 2026-07-20 to choose and record one permanent project location with no persistent copies across the PC.
- Change control: any future locked-interface amendment requires an explicit creator question and recorded approval before editing.

Do not open world scale, content counts, society models, economy depth, combat scope, power roster, UI count, or post-1.0 release allocation during TASK-001.
