# TASK-002 — Source-of-truth, story, and AI document set

**Status:** Not Started  
**Phase:** Phase 0  
**Risk:** High  
**Depends on:** TASK-001  
**Evidence folder:** `docs/evidence/TASK-002/`
**Blueprint source:** Section 81, Task 002; Sections 4.7, 65–68, 75.1, 80.1, 81, and Appendix F–H

## Creator summary
Install the durable memory Codex will use instead of chat history, including the special rules for simulation truth, story authority, and coded intelligence.

## Objective
Every future task can locate authoritative product, architecture, data, save, AI, story, risk, and acceptance instructions from the repository.

## In scope
- Adopt this kit and blueprint into the real repository.
- Confirm document hierarchy and ownership.
- Complete architecture map, unit catalog, save/history rules, story authority, AI principles, decision hierarchy, and behavior-contract catalog.
- Configure `codex.md` fallback and scoped instructions without creating `AGENTS.md`.
- Add document validation and stale-link checks.

## Required outputs
- [ ] Approved repository documentation hierarchy.
- [ ] Current architecture and domain maps.
- [ ] Story Director authority contract.
- [ ] AI golden rules, contract template, decision catalog, and regression ledger.
- [ ] Documentation validator and index.

## Verification and acceptance
- [ ] No conflicting source-of-truth statements.
- [ ] All required files are reachable from root instructions.
- [ ] No `AGENTS.md` file exists.
- [ ] Codex loads root and scoped `codex.md` through configured fallback.
- [ ] Deliberately broken internal reference fails validation.
- [ ] Documentation and relevant `codex.md` instructions match the implementation.
- [ ] Independent adversarial review reports no blocking findings.
- [ ] Creator-visible acceptance is recorded.

## Out of scope
- Implementing game behavior or using documentation to supersede locked blueprint decisions without an ADR.

## Required work sequence
- [ ] Planner produces small milestones, exact files, tests, rollback, and stop conditions.
- [ ] Creator approves the plan.
- [ ] Implementer completes one milestone at a time.
- [ ] Verification agent reproduces evidence from a clean worktree.
- [ ] Adversarial reviewer challenges architecture and failure cases.
- [ ] Creator tests the observable result and accepts or requests changes.
- [ ] `Build/validate_plan.py` permits Done status.

## Suggested launch
Use `docs/prompts/01_PLAN_TASK.md` with **5.6 Sol / Extra High** for planning. After approval, use **5.6 Terra / High** for ordinary implementation unless the plan identifies a critical architecture or migration change.
