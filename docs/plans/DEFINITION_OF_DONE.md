# Definition of Done

## A feature is done only when
- [ ] Player-visible behavior matches an approved specification.
- [ ] Authority, knowledge, determinism, units, and invariants are explicit.
- [ ] Relevant behavior contract/content schema is versioned and validated.
- [ ] Positive, negative, boundary, replay/save, and performance tests pass.
- [ ] Decision traces or causal evidence explain important outcomes.
- [ ] Save/migration/compaction impact is implemented and tested.
- [ ] Performance, allocations, queries, and candidate counts fit budget.
- [ ] Documentation, ADRs, and release notes are current.
- [ ] A clean worktree reproduces the evidence.
- [ ] Independent adversarial review has no blocking finding.
- [ ] The creator accepts the observable result.

## A phase is done only when
- [ ] Every declared gate scenario passes.
- [ ] All critical/high risks are closed, reduced, or explicitly accepted by the creator.
- [ ] No hidden deferred work is required for the next phase to be meaningful.
- [ ] The rollback/fallback decision is documented.
- [ ] The phase evidence index and creator gate decision are committed.

## Not sufficient
Compilation, one successful run, screenshots, agent confidence, a self-review, or a passing happy path.
