# Risk Register - Operating Summary

The full Blueprint risk register remains authoritative for the Version 1.5 horizon. Review this concise register at each active milestone gate.

| ID | Risk | Trigger | Primary prevention | Gate evidence |
|---|---|---|---|---|
| R01 | Scope expands toward Version 1.5 before 1.0 works | Future-release work enters an active task | Active-milestone scope and explicit exclusions | Diff/task review |
| R02 | Decisions are locked before evidence | Agents invent counts, systems, or product choices not yet opened | Decision queue and stop conditions | Decision audit |
| R03 | Agent architectural drift | Duplicate concepts, cross-layer mutations, hidden shortcuts | Package boundaries, ADRs, adversarial review | Architecture checks |
| R04 | Nondeterminism | Replay/state checksum diverges | Stable ordering, keyed RNG, fixed accumulators | Replay suite |
| R05 | Save corruption or growth | Invalid references, large or unrecoverable saves | Semantic chunks, atomic write, migration fixtures | Save suite and size report |
| R06 | Performance collapse | Backlog, allocations, or global scans grow | Measured scale ladder, cadence/candidate/query budgets | p95 and soak reports |
| R07 | Pathfinding explosion | Per-person long-range searches | Hierarchical routes and bounded invalidation | Route benchmark |
| R08 | Hidden omniscience | AI acts on truth it cannot know | Observer contexts and knowledge-filter tests | Perspective scenarios |
| R09 | Force Outcome invalid state | Forced mutation breaks ownership/membership/cache invariants | Reconciliation layer and invariant tests | Force scenarios |
| R10 | Artificial storyteller | Narrative fabricates outcomes or favors watched regions | Strict story authority and simulation comparison | Checksum/profile tests |
| R11 | Settlements or societies converge | Matched scenarios produce the same layout and choices | Contrast experiments and divergence tests | Divergence report |
| R12 | Creator overload | Work arrives as large questionnaires or ambiguous dumps | Milestone-timed decisions and one-question prompts | Creator acceptance |
| R13 | Lean release becomes generic | Scope cuts remove physical causality, divergence, or god-player identity | Identity guardrails and release acceptance | Identity review |

Any critical trigger pauses feature expansion until a mitigation task or explicit creator exception is accepted.
