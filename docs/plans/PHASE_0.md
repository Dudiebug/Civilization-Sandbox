# Phase 0 — Governance and Reproducible Toolchain

**Status:** Not Started  
**Estimated duration:** 2–3 weeks

## Objective
Create a repository that coding agents can safely modify and independent sessions can reproduce.

## Entry condition
Authoritative blueprint and this planning kit are available; creator approves repository creation.

## Work checklist
- [ ] Initialize protected Git repository and branch/worktree conventions.
- [ ] Pin Unity 6.3 LTS patch, Unity Hub/editor installation method, .NET/PowerShell prerequisites, and package-lock policy.
- [ ] Create idempotent Windows bootstrap, build, test, and evidence commands.
- [ ] Create Linux player smoke-build path and document unsupported host assumptions.
- [ ] Establish assembly/package boundaries and dependency checks.
- [ ] Install analyzers and forbid uncontrolled RNG, presentation-to-simulation mutation, and forbidden dependencies.
- [ ] Create source-of-truth, ADR, task, behavior-contract, evidence, and regression workflows.
- [ ] Configure CI with retained logs/reports and clean-checkout validation.
- [ ] Record reference hardware and baseline environment.
- [ ] Deliberately inject one forbidden dependency and one unauthorized RNG call; prove CI rejects both.

## Exit gate
A fresh Windows account can clone, bootstrap, build, run one headless test, and package evidence without manual repair. Deliberate policy violations fail CI.

## Do not build in this phase
Terrain, citizens, buildings, art, or gameplay features.

## Completion record
Evidence index: Pending  
Independent phase review: Pending  
Creator gate decision: Pending
