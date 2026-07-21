# TASK-004 - Command-line and CI harness

**Status:** Not Started
**Milestone:** 0.1 - Project Foundation
**Release horizon:** Lean Version 1.0; complete Blueprint early-modern breadth is Version 1.5
**Risk:** Critical
**Depends on:** TASK-001, TASK-003
**Decision dependencies:** None unless the approved task plan identifies an active decision gate
**Evidence folder:** `docs/evidence/TASK-004/`
**Blueprint source:** Section 81, Task 004; Sections 4.7, 65–68, 75.1, 80.1, 81, and Appendix F–H

## Creator summary
Create one repeatable command path that builds, tests, benchmarks, and packages evidence for humans and Codex.

## Objective
Local and CI runs use the same commands and produce retained, diagnosable artifacts.

## In scope
- Headless build and test entrypoints.
- CI workflow for clean checkout, architecture checks, unit/scenario tests, and artifacts.
- Deterministic seed-suite placeholder.
- Performance baseline artifact format.
- Failure diagnostics and exit-code contract.

## Required outputs
- [ ] Local command wrappers.
- [ ] CI workflow and artifact retention.
- [ ] JUnit/structured test reports.
- [ ] Baseline evidence bundle and machine-readable result summary.

## Verification and acceptance
- [ ] Clean CI run passes.
- [ ] Deliberate build/test failure returns nonzero and retains logs.
- [ ] Artifacts include revision, package hash, environment, and command line.
- [ ] Local and CI result schemas match.
- [ ] Documentation and relevant `codex.md` instructions match the implementation.
- [ ] Independent adversarial review reports no blocking findings.
- [ ] Creator-visible acceptance is recorded.

## Out of scope
- Gameplay features or hiding flaky tests through retries without diagnosis.

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
