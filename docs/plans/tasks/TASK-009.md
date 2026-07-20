# TASK-009 — Canonical checksum and state diff

**Status:** Not Started  
**Phase:** Phase 1  
**Risk:** Critical  
**Depends on:** TASK-005, TASK-006, TASK-007, TASK-008  
**Evidence folder:** `docs/evidence/TASK-009/`
**Blueprint source:** Section 81, Task 009; Sections 10, 17, 47, 51, 55–57, 66–68, 75.2, 80.2, 81, and technical appendices

## Creator summary
Make invisible divergence diagnosable instead of discovering it months later.

## Objective
Generate deterministic subsystem digests and localized state diffs over authoritative state while excluding presentation caches.

## In scope
- Canonical field ordering and serialization rules.
- Subsystem and whole-world checksums.
- Checkpoint schedule and command-log association.
- First-divergence localization report.
- Explicit excluded/derived-state catalog.

## Required outputs
- [ ] Checksum/state-diff library.
- [ ] Replay report format.
- [ ] Diagnostic CLI.
- [ ] Known-divergence fixtures.

## Verification and acceptance
- [ ] Identical runs produce identical checkpoints.
- [ ] A seeded one-field divergence identifies subsystem/entity/field.
- [ ] Presentation changes do not change checksum.
- [ ] Hash iteration/order variations are neutralized.
- [ ] Documentation and relevant `codex.md` instructions match the implementation.
- [ ] Independent adversarial review reports no blocking findings.
- [ ] Creator-visible acceptance is recorded.

## Out of scope
- Treating a single opaque hash mismatch as sufficient diagnosis.

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
