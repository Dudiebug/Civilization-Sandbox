# TASK-017 - AI Decision Laboratory runner

**Status:** Not Started
**Milestone:** 0.2 - Decision and Engine Proof
**Release horizon:** Minimum robust laboratory for Version 1.0; broader analytics may deepen by Version 1.5
**Risk:** Critical
**Depends on:** TASK-009, TASK-010, TASK-015, TASK-016
**Decision dependencies:** Exact seed/defect counts are set in the approved task plan
**Evidence folder:** `docs/evidence/TASK-017/`
**Blueprint/ADR source:** Blueprint Sections 58-64, 67-68, 81; ADR-001

## Creator summary
Create the reproducible lab that can challenge the three foundational behaviors without building a universal AI research platform first.

## Objective
Load fixtures, run required deterministic/scenario/invariant/metamorphic or multi-seed checks, compare traces/state, save/load mid-decision, and measure per-decision cost.

## In scope
- Fixture schema/loader and headless batch runner.
- Golden, negative, boundary, and invariant assertions.
- Metamorphic and multi-seed support where a contract declares them.
- Before/after trace/state reports.
- Save/load-at-decision and deliberate-defect modes.
- p50/p95/max and allocation reports.

## Required outputs
- [ ] Decision Lab CLI and reports.
- [ ] Sample fixtures and CI integration.
- [ ] Defect-injection mechanism covering implemented primitive families.

## Verification and acceptance
- [ ] A known behavioral change produces a focused comparison.
- [ ] Save/load mid-commitment preserves later selection.
- [ ] Deliberate defects in implemented primitive families are detected.
- [ ] Batch performance is measured at the accepted Version 1.0 tier.
- [ ] Exact counts are evidence-based task-plan values, not permanent product decisions.
- [ ] Independent review and creator acceptance pass.

## Out of scope
Tuning broad gameplay content, a giant decision catalog, or accepting “looks smart” as evidence.
