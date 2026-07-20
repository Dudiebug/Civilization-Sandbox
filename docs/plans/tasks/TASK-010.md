# TASK-010 — Semantic persistence skeleton

**Status:** Not Started  
**Phase:** Phase 1  
**Risk:** Critical  
**Depends on:** TASK-006, TASK-008, TASK-009  
**Evidence folder:** `docs/evidence/TASK-010/`
**Blueprint source:** Section 81, Task 010; Sections 10, 17, 47, 51, 55–57, 66–68, 75.2, 80.2, 81, and technical appendices

## Creator summary
Create recoverable versioned saves before gameplay state becomes difficult to migrate.

## Objective
Atomic semantic save/load with manifest, versioned chunks, checksums, migrations, previous-good backup, active queues, and history.

## In scope
- Human-readable manifest and build/content/schema metadata.
- Versioned domain DTO/chunk container with stable keys.
- Quiescent barrier and staged snapshot.
- Atomic temporary write/flush/replace and backup rotation.
- Sequential pure migration fixtures and corruption diagnostics.

## Required outputs
- [ ] Save/load package and CLI fixture.
- [ ] Format/version policy.
- [ ] Migration test harness.
- [ ] Interrupted-write and corrupted-chunk fixtures.

## Verification and acceptance
- [ ] Round-trip canonical checksum equality.
- [ ] Interrupted write leaves old or new valid save.
- [ ] Corruption identifies the affected chunk and backup path.
- [ ] Unsupported future format fails safely.
- [ ] Removed keys remain reserved.
- [ ] Documentation and relevant `codex.md` instructions match the implementation.
- [ ] Independent adversarial review reports no blocking findings.
- [ ] Creator-visible acceptance is recorded.

## Out of scope
- Raw ECS memory, scene objects, presentation caches, or silent best-effort repair that hides data loss.

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
