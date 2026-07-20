# TASK-005 - Authoritative world clock and scheduler shell

**Status:** Not Started
**Milestone:** 0.1 - Project Foundation
**Release horizon:** Lean Version 1.0; complete Blueprint early-modern breadth is Version 1.5
**Risk:** Critical
**Depends on:** TASK-004
**Decision dependencies:** None unless the approved task plan identifies an active decision gate
**Evidence folder:** `docs/evidence/TASK-005/`
**Blueprint source:** Section 81, Task 005; Sections 10, 17, 47, 51, 55–57, 66–68, 75.2, 80.2, 81, and technical appendices

## Creator summary
Create the fixed, camera-independent heartbeat that all future systems will obey.

## Objective
Typed world time, fixed scheduling, speed controls, deterministic due-event order, staggering, and backlog reporting run headlessly.

## In scope
- 64-bit simulation-second world clock.
- 20 Hz fixed wall loop with pause/1×/2×/5×/10× deltas.
- Due-time queue ordered by time, priority, stable target, and sequence.
- Stable-ID staggering and watchdog metrics.
- Typed Duration/WorldTime APIs and overflow guards.

## Required outputs
- [ ] Clock/scheduler package and tests.
- [ ] Headless scheduler demo.
- [ ] Backlog/overdue telemetry.
- [ ] Deterministic event-order fixture.

## Verification and acceptance
- [ ] Same seed/commands produce identical event order.
- [ ] Frame rate and rendering absence do not affect checkpoints.
- [ ] 10× reports CPU limitation instead of dropping due work.
- [ ] Large elapsed deltas preserve rational accumulation.
- [ ] Documentation and relevant `codex.md` instructions match the implementation.
- [ ] Independent adversarial review reports no blocking findings.
- [ ] Creator-visible acceptance is recorded.

## Out of scope
- Real person AI, calendar content, or polling every entity every frame.

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
