# Risk Register — Operating Summary

The full blueprint risk register remains authoritative. Review this short list at every phase gate.

| ID | Risk | Trigger | Primary prevention | Gate evidence |
|---|---|---|---|---|
| R01 | Scope expansion | Later-age or polish work enters active task | One-task scope and deferred list | Diff and task review |
| R02 | Agent architectural drift | Duplicate concepts, cross-layer mutations, hidden shortcuts | Package boundaries, ADRs, adversarial review | Architecture checks |
| R03 | Nondeterminism | Replay/state checksum diverges | Stable ordering, keyed RNG, fixed accumulators | Replay suite |
| R04 | Save corruption/growth | Invalid references, large or unrecoverable saves | Semantic chunks, atomic write, migration fixtures | Save suite and size report |
| R05 | Performance collapse | Backlog, allocations, global scans | Cadence/candidate/query budgets, synthetic benchmarks | p95 and soak reports |
| R06 | Pathfinding explosion | Per-person long-range searches | Hierarchical routes and bounded invalidation | Route benchmark |
| R07 | Hidden omniscience | AI acts on truth it cannot know | Observer contexts and knowledge-filter tests | Perspective scenarios |
| R08 | Force Outcome invalid state | Forced mutation breaks ownership/membership/cache invariants | Reconciliation layer and invariant tests | Force scenarios |
| R09 | Artificial storyteller | Narrative fabricates outcomes or favors watched regions | Strict authority API and Pure Simulation comparison | Checksum/profile tests |
| R10 | City/visual unreadability | Dense billboards, occlusion, unclear direction | Stress scenes and action-readability contracts | Visual gate |
| R11 | All civilizations converge | Same solution portfolio across matched seeds | Capability alternatives and statistical divergence tests | Divergence report |
| R12 | Creator overload | Work arrives as large, ambiguous, unreviewable dumps | Cognitive-access rules and one-decision prompts | Creator acceptance |

Any critical trigger pauses feature expansion until a mitigation task is accepted.
