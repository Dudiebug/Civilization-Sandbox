# TASK-001 - Repository governance and reproducible bootstrap

**Status:** In Progress
**Milestone:** 0.1 - Project Foundation
**Release horizon:** Lean Version 1.0; complete Blueprint early-modern breadth is Version 1.5
**Risk:** Critical
**Depends on:** None
**Decision dependencies:** D01 and D02 recorded; repository host/location recorded
**Evidence folder:** `docs/evidence/TASK-001/`
**Blueprint/ADR source:** Blueprint Sections 4.7, 56-57, 65-68, 75.1, 80.1, 81; ADR-001

## Creator summary
Create the safe container for all later work. This task opens only the decisions needed for a reproducible toolchain. It does not decide gameplay scope, content counts, or future-release systems.

## Objective
A fresh supported development environment can clone, bootstrap, build a baseline project, run one headless test, and package evidence without manual repair.

## In scope
- Initialize Git and protected-branch/worktree conventions.
- Present and record the D01 engine/toolchain decision; pin the selected engine/editor/runtime and initial dependencies.
- Record D02 supported development hosts and reference hardware.
- Create idempotent bootstrap, build, test, package, and clean commands.
- Record prerequisites, dependency lock, baseline build manifest, and recoverable known-good baseline.
- Establish destructive-operation policy and rollback.

## Required outputs
- [x] ~~Repository root and Unity project skeleton.~~
- [x] ~~Accepted engine/toolchain ADR and exact dependency locks.~~
- [x] ~~Windows bootstrap/build/test/package scripts.~~
- [ ] Clean-account verification procedure and first evidence pack.
- [x] ~~Baseline build manifest and rollback instructions.~~

## Verification and acceptance
- [ ] Run from a fresh Windows user profile and clean clone.
- [ ] Baseline editor/player build succeeds non-interactively.
- [x] ~~One headless test executes and reports deterministically on the creator machine.~~
- [x] ~~Repeated bootstrap is idempotent on the creator machine.~~
- [x] ~~Broken prerequisite or package hash fails with a useful diagnosis.~~
- [x] ~~Documentation and relevant `codex.md` instructions match the implementation.~~
- [ ] Placeholder engine-specific folders are accepted, adapted, or removed consistently with D01.
- [ ] Independent adversarial review reports no blocking findings.
- [ ] Creator-visible acceptance is recorded.

## Locked implementation contract

- Unity `6000.3.20f1`, changeset `c9ba695d4f07`, Windows x64 Mono, and Linux x64 Mono.
- Direct packages: Burst `1.8.29`, Collections `2.6.8`, Entities `1.4.8`, Entities Graphics `1.4.21`, URP `17.3.0`, Test Framework `1.6.0`, Linux SDK `1.1.0`, Windows-to-Linux toolchain `1.1.0`, and local `com.civsandbox.tooling@0.1.0`.
- Required transitive Linux sysroot: `com.unity.sysroot.base@1.1.0`.
- The committed `Packages/packages-lock.json` hash in `Config/toolchain.json` is the package-integrity authority.
- `.codex/config.toml` must remain absent; scoped `codex.md` files are the repository instruction authority.
- Repository visibility is public, with protected `main`; this replaces the original private-repository assumption by explicit creator approval on 2026-07-20 after GitHub returned HTTP 403 for private branch protection.

## Change control

Before changing any locked interface, pin, authority file, required output, or acceptance boundary, the implementer must ask the creator an explicit question and wait for recorded approval. The approved amendment and reason must then be recorded in this plan and TASK-001 evidence.

Recorded amendments:

- 2026-07-20: the creator answered `public`, approving public repository visibility so GitHub branch protection can be enforced without weakening the protected-`main` requirement.
- 2026-07-20: the creator directed Codex to choose one permanent project location and prohibit copies across the PC. `C:\Users\dudie\Projects\Civilization-Sandbox` is the sole authoritative checkout and Unity project; persistent sibling worktrees and secondary development clones are retired. A guarded disposable verification clone may exist only for one verification command and must be removed before that command returns.

## Out of scope
World size, population target, content counts, gameplay systems, terrain, people, buildings, AI behavior, art, public release configuration, or Version 1.1-1.5 planning.

## Stop conditions
- A future D01/D02 or locked-interface amendment is needed but creator approval has not been recorded.
- The selected path cannot support clean headless validation.
- Bootstrap or build changes tracked project files outside an explicitly approved dependency amendment.
- Implementation starts choosing gameplay or release scope.

## Required work sequence
- [x] ~~Planner produces small milestones, exact files, tests, rollback, and stop conditions.~~
- [x] ~~Creator approves the plan.~~
- [ ] Implementer completes one milestone at a time.
- [ ] Verification agent reproduces evidence from a clean worktree.
- [ ] Adversarial reviewer challenges architecture and failure cases.
- [ ] Creator tests the observable result and accepts or requests changes.
- [ ] `Build/validate_plan.py` permits Done status.

## Suggested launch
Use `docs/prompts/01_PLAN_TASK.md` with **5.6 Sol / Max** for Critical planning. After approval, use **5.6 Terra / High** for ordinary implementation and **Terra / Medium** only for exact mechanical work.
