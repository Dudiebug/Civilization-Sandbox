# TASK-015 — AI Behavior Contract schema and decision catalog

**Status:** Not Started  
**Phase:** Phase 2  
**Risk:** Critical  
**Depends on:** TASK-002, TASK-003, TASK-010  
**Evidence folder:** `docs/evidence/TASK-015/`
**Blueprint source:** Section 81, Task 015; Sections 58–64, 67–68, 75.3, 80.3, 81, and Appendix A/B/G

## Creator summary
Define the machine-readable contract every consequential decision must satisfy.

## Objective
Create versioned contract schema, Markdown representation, catalog, authority validation, save compatibility, and repository routing.

## In scope
- Stable decision/contract IDs and versioning.
- Fields for owner, trigger, candidates, observations, preconditions, scores, randomness, commitments, traces, budgets, and scenarios.
- Schema/content validation.
- Catalog index and deprecation/reserved-ID rules.
- Links to fixtures, implementation, trace schema, and performance reports.

## Required outputs
- [ ] Contract schema and validator.
- [ ] Catalog data and generated documentation.
- [ ] Three initial contract stubs.
- [ ] Invalid contract fixtures.

## Verification and acceptance
- [ ] Missing authority/candidate cap/perspective/budget fails validation.
- [ ] Retired IDs cannot be reused.
- [ ] Catalog links resolve.
- [ ] Schema round-trip is stable and migration-ready.
- [ ] Documentation and relevant `codex.md` instructions match the implementation.
- [ ] Independent adversarial review reports no blocking findings.
- [ ] Creator-visible acceptance is recorded.

## Out of scope
- Implementing specific behavior logic or allowing content data to inject arbitrary code.

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
