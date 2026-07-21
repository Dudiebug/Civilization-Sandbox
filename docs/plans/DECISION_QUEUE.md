# Milestone-Timed Decision Queue

This is not a questionnaire. No decision is required until the stated gate. Agents must not choose an answer early to make planning easier.

When a decision opens, the planner presents one question at a time, two or three evidence-grounded options, consequences, a recommended default if appropriate, and a clear defer/approve choice.

## Recorded decisions

- D01 resolved in TASK-001: Unity `6000.3.20f1` and the exact tool/module/package pins in `Config/toolchain.json`.
- D02 resolved in TASK-001: pinned Windows development host and the informational creator hardware baseline in `Config/benchmark-reference.json`.
- Reopening D01 or D02 requires an explicit creator question and approval before changing a locked interface.

| ID | Decision | Open no earlier than | Required before | Evidence expected |
|---|---|---|---|---|
| D01 | Engine/toolchain path and exact pinned versions | TASK-001 planning | TASK-001 implementation | Clean bootstrap feasibility, licensing/support, headless/test/presentation needs |
| D02 | Reference development hardware and supported build hosts | TASK-001 planning | Milestone 0.1 gate | Available hardware and CI constraints |
| D03 | Version 1.0 target scale tier: population, world envelope, and stress tier | TASK-011 results | Milestone 0.2 gate | Tick, memory, save/load, routing, and presentation ladder |
| D04 | Living Camp person/household detail and initial world-generation envelope | Milestone 0.3 planning | TASK-021/TASK-022 implementation | Decision Lab results and accepted scale tier |
| D05 | Physical Settlement resource, building, occupation, and settlement-depth envelope | Milestone 0.4 planning | First 0.4 task implementation | Living Camp bottlenecks and creator playtest |
| D06 | Divergence variables, number of societies, and thin institutional/government abstraction | Milestone 0.5 planning | First 0.5 task implementation | Physical Settlement behavior and paired-world prototype |
| D07 | Trade, allocation, price, and route complexity for the connected region | Milestone 0.6 planning | First 0.6 task implementation | Supply/route tests and desired player readability |
| D08 | Conflict scale, violence presentation, occupation depth, and recovery boundary | Milestone 0.7 planning | First 0.7 task implementation | Connected-region performance and creator tone review |
| D09 | Version 1.0 power roster, UI organization, and explanation depth | Milestone 0.8 planning | First 0.8 task implementation | Integrated systems and creator interaction test |
| D10 | Version 1.0 release envelope: exact supported scale, soak duration, save horizon, minimum hardware, and distribution target | Milestone 0.9 planning | Release candidate lock | Full integrated benchmarks, external playtest, save-growth evidence |
| D11 | Exact Version 1.1 theme and content | After Version 1.0 acceptance | Version 1.1 planning | Player feedback and unfinished 1.5 backlog |

## Explicitly not being decided now
- Exact world dimensions or population count.
- Exact resource, building, occupation, government, cultural, technology, diplomacy, power, or UI counts.
- Final title, price, release date, store page, marketing plan, or full platform list.
- Exact 1.1-1.4 feature allocation.
- Whether a reserved Version 1.5 system is promoted into Version 1.0.

A task that reaches an unopened decision stops and records the blocker rather than inventing a temporary product truth.
