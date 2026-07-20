# TASK-001 — Repository governance and reproducible Windows bootstrap

**Status:** Not Started  
**Phase:** Phase 0  
**Risk:** Critical  
**Depends on:** None  
**Evidence folder:** `docs/evidence/TASK-001/`
**Blueprint source:** Section 81, Task 001; Sections 4.7, 65–68, 75.1, 80.1, 81, and Appendix F–H

## Creator summary
Create the safe container for all later work. The result is a clean, pinned, recoverable repository—not gameplay.

## Objective
A fresh Windows account can clone, bootstrap, build a baseline Unity project, run one headless test, and package evidence without manual repair.

## In scope
- Initialize Git and protected-branch/worktree conventions.
- Pin the exact Unity 6.3 LTS patch and initial package set.
- Create idempotent PowerShell bootstrap, build, test, package, and clean commands.
- Record tool prerequisites, reference hardware, package lock, and baseline build manifest.
- Create recoverable known-good baseline and destructive-operation policy.

## Required outputs
- [ ] Repository root and Unity project skeleton.
- [ ] Pinned dependency/package files.
- [ ] Windows bootstrap/build/test/package scripts.
- [ ] Clean-account verification procedure and first evidence pack.
- [ ] Initial ADR for exact engine/package pins.

## Verification and acceptance
- [ ] Run from a fresh Windows user profile and clean clone.
- [ ] Baseline editor/player build succeeds non-interactively.
- [ ] One headless test executes and reports deterministically.
- [ ] Repeated bootstrap is idempotent.
- [ ] Broken prerequisite or package hash fails with a useful diagnosis.
- [ ] Documentation and relevant `codex.md` instructions match the implementation.
- [ ] Independent adversarial review reports no blocking findings.
- [ ] Creator-visible acceptance is recorded.

## Out of scope
- Terrain, gameplay systems, citizens, buildings, AI, art, or broad content.

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
