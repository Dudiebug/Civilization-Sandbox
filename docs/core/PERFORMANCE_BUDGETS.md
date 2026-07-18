# Performance Budgets

Initial engineering contracts for the reference machine:

- 20 Hz authoritative base loop without unbounded backlog.
- Target 60 FPS at 1080p in ordinary settlement views.
- Operable at pause, 1×, 2×, 5×, and 10×; report CPU limitation rather than dropping simulation work.
- 10,000 persistent people in the Large benchmark.
- Working set below 6 GB.
- Representative 100-year save below 100 MB compressed.
- Main-thread save snapshot hitch below 250 ms; save below 10 s; load below 15 s.
- 24-hour wall-clock soak, 500-game-year accelerated run, and 100-year deterministic replay before release.
- 5,000 moving billboards and 1,000 building instances in the presentation proof.

Every system declares cadence, maximum candidates, spatial-query cap, allocation behavior, trace overhead, and benchmark scenario. Budgets may change only through measured evidence and an ADR.
