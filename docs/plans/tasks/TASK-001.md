# TASK-001 - Repository governance and reproducible bootstrap

**Status:** Not Started
**Milestone:** 0.1 - Project Foundation
**Release horizon:** Lean Version 1.0; complete Blueprint early-modern breadth is Version 1.5
**Risk:** Critical
**Depends on:** None
**Decision dependencies:** D01, D02, repository host/location
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
- [ ] Repository root and selected-engine project skeleton.
- [ ] Accepted engine/toolchain ADR and exact dependency locks.
- [ ] Bootstrap/build/test/package scripts for approved hosts.
- [ ] Clean-account or clean-environment verification and first evidence pack.
- [ ] Baseline build manifest and rollback instructions.

## Verification and acceptance
- [ ] Clean clone/bootstrap/build/test succeeds on the approved development host.
- [ ] One headless deterministic test executes and reports results.
- [ ] Repeated bootstrap is idempotent.
- [ ] Broken prerequisite or dependency hash fails with a useful diagnosis.
- [ ] Placeholder engine-specific folders are accepted, adapted, or removed consistently with D01.
- [ ] Independent adversarial review reports no blocking findings.
- [ ] Creator-visible acceptance is recorded.

## Out of scope
World size, population target, content counts, gameplay systems, terrain, people, buildings, AI behavior, art, public release configuration, or Version 1.1-1.5 planning.

## Stop conditions
- D01 or D02 is required but not recorded.
- The selected path cannot support clean headless validation.
- Implementation starts choosing gameplay or release scope.

## Required work sequence
Plan -> creator decision(s) one at a time -> approved milestones -> implementation -> clean verification -> adversarial review -> creator acceptance -> plan validation.
