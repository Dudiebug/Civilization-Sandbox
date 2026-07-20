# TASK-003 - Architecture and code-quality gates

**Status:** Not Started
**Milestone:** 0.1 - Project Foundation
**Release horizon:** Lean Version 1.0; complete Blueprint early-modern breadth is Version 1.5
**Risk:** Critical
**Depends on:** TASK-001, TASK-002
**Decision dependencies:** None unless the approved task plan identifies an active decision gate
**Evidence folder:** `docs/evidence/TASK-003/`
**Blueprint source:** Section 81, Task 003; Sections 4.7, 65–68, 75.1, 80.1, 81, and Appendix F–H

## Creator summary
Make architectural violations fail automatically before they accumulate.

## Objective
CI rejects forbidden dependencies, uncontrolled randomness, simulation state in presentation, and invalid content/schema boundaries.

## In scope
- Define selected-engine modules/assemblies/packages and the allowed dependency graph.
- Add analyzers or architecture tests for forbidden APIs and layers.
- Forbid uncontrolled RNG and frame/camera inputs in authoritative packages.
- Add content-schema validation skeleton and stable-ID rules.
- Add no-authoritative-state-in-presentation checks.

## Required outputs
- [ ] Assembly/package dependency policy.
- [ ] Automated architecture tests/analyzers.
- [ ] Forbidden API catalog and exception process.
- [ ] Content validation command and sample failing fixtures.

## Verification and acceptance
- [ ] Inject forbidden presentation-to-simulation dependency; CI fails.
- [ ] Inject uncontrolled RNG in authoritative code; CI fails.
- [ ] Inject duplicate/invalid content ID; validation fails.
- [ ] Valid minimal project remains buildable.
- [ ] Documentation and relevant `codex.md` instructions match the implementation.
- [ ] Independent adversarial review reports no blocking findings.
- [ ] Creator-visible acceptance is recorded.

## Out of scope
- Premature domain implementations or analyzers that require manual interpretation to determine pass/fail.

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
