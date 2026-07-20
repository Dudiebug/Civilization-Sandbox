# TASK-016 — Canonical AI primitives

**Status:** Not Started  
**Phase:** Phase 2  
**Risk:** Critical  
**Depends on:** TASK-005, TASK-007, TASK-012, TASK-015  
**Evidence folder:** `docs/evidence/TASK-016/`
**Blueprint source:** Section 81, Task 016; Sections 58–64, 67–68, 75.3, 80.3, 81, and Appendix A/B/G

## Creator summary
Implement one approved set of bounded, deterministic decision building blocks.

## Objective
Provide preconditions, candidate adapters, normalized curves, knowledge filters, reservations, cooldown, hysteresis, stable selection, and trace emission.

## In scope
- Canonical normalized curve/evaluator library.
- Authority and observer-knowledge contexts.
- Precondition result/evidence types.
- Commitment/invalidation/cooldown/hysteresis primitives.
- Reservation interfaces and deterministic tie/weighted selection.
- Budget counters and trace writer.

## Required outputs
- [ ] Versioned AI runtime package.
- [ ] Primitive unit/property tests.
- [ ] Usage examples and forbidden patterns.

## Verification and acceptance
- [ ] Impossible candidates cannot become feasible through utility or traits.
- [ ] Unknown information is unavailable in AI context.
- [ ] Candidate/observation/query budgets enforce hard limits.
- [ ] Parallel order does not change selection.
- [ ] Trace reconstructs all terms and rejections.
- [ ] Documentation and relevant `codex.md` instructions match the implementation.
- [ ] Independent adversarial review reports no blocking findings.
- [ ] Creator-visible acceptance is recorded.

## Out of scope
- A universal one-size-fits-all brain, arbitrary reflection, or per-feature scoring frameworks.

## Required work sequence
- [ ] Planner produces small milestones, exact files, tests, rollback, and stop conditions.
- [ ] Creator approves the plan.
- [ ] Implementer completes one milestone at a time.
- [ ] Verification agent reproduces evidence from a clean worktree.
- [ ] Adversarial reviewer challenges architecture and failure cases.
- [ ] Creator tests the observable result and accepts or requests changes.
- [ ] `Build/validate_plan.py` permits Done status.

## Suggested launch
Use `docs/prompts/01_PLAN_TASK.md` with **5.6 Sol / Max** for Critical planning. After approval, use **5.6 Terra / High** for ordinary implementation and **Terra / Medium** only for exact mechanical work.
