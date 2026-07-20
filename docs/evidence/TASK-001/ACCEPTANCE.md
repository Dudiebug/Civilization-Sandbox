# Creator Acceptance — TASK-001

**State:** PENDING

## Observe

1. Run bootstrap twice and confirm both diagnostics are understandable, both result JSON files pass, Git tracked state is unchanged, and the package-lock hash is identical.
2. Inspect the single passing EditMode test, Windows/Linux build outputs, and the headless Windows player exit.
3. Run evidence packaging and inspect its manifest and archive hash.
4. Inspect protected `main`, its `repository-policy` check, recovery export, and rollback commands.

## Acceptance gates

- [ ] Two idempotent bootstrap runs observed.
- [ ] Negative prerequisite, hash, cleanup, and evidence diagnostics are understandable.
- [ ] Exactly one headless EditMode test passes.
- [ ] Windows and Linux Mono players exist and Windows exits headlessly.
- [ ] Evidence package is complete and Git is clean.
- [ ] Independent review has no blocker.
- [ ] Recovery rehearsal succeeds.

**Decision:** PENDING
**Creator notes:** Record `APPROVE`, `CHANGES`, or `REJECT` after observing every gate.
