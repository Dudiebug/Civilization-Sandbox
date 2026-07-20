# Definition of Done

## A task or feature is done only when
- [ ] Player-visible or tool-visible behavior matches the approved task specification.
- [ ] Scope matches the active milestone and no unopened product decision was silently made.
- [ ] Authority, knowledge, determinism, units, and invariants are explicit where relevant.
- [ ] Relevant behavior contract/content schema is versioned and validated.
- [ ] Required positive, negative, boundary, replay/save, and performance evidence passes or is justified N/A.
- [ ] Decision traces or causal evidence explain important outcomes where relevant.
- [ ] Save/migration/compaction impact is handled at the depth required by the task.
- [ ] Performance, allocations, queries, and candidate counts fit the approved budget.
- [ ] Documentation, ADRs, and release notes are current.
- [ ] A clean worktree reproduces the evidence.
- [ ] Independent adversarial review has no blocking finding.
- [ ] The creator accepts the observable result.

## A milestone is done only when
- [ ] Every declared gate scenario passes.
- [ ] Identity guardrails are preserved.
- [ ] Decisions opened by the milestone are recorded; future decisions remain deferred.
- [ ] Critical/high risks are closed, reduced, or explicitly accepted.
- [ ] No hidden future-release system is required for the next milestone to be meaningful.
- [ ] Rollback/fallback and next-scope recommendation are documented.
- [ ] The milestone evidence index and creator gate decision are committed.

## Version 1.0 is done only when
- [ ] `VERSION_1_PRODUCTION.md` and the approved 0.9 release-scope manifest pass.
- [ ] No Version 1.5-only item is treated as an undocumented release blocker.
- [ ] The creator accepts the game as a coherent standalone regional civilization sandbox.

## Not sufficient
Compilation, one successful run, screenshots, agent confidence, self-review, a passing happy path, or implementing architecture-reserved work ahead of its milestone.
