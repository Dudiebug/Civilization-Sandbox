# TASK-022 — Persistent people, households, inheritance, and traits

**Status:** Not Started  
**Phase:** Phase 3  
**Risk:** Critical  
**Depends on:** TASK-006, TASK-010, TASK-019, TASK-021  
**Evidence folder:** `docs/evidence/TASK-022/`
**Blueprint source:** Section 81, Task 022; Sections 16–27, 47–54, 69–75, 80.4–80.5, 81, and technical appendices

## Creator summary
Create the durable human substrate while keeping social simulation bounded.

## Objective
Persist people and lightweight households through life, needs, health, work eligibility, controlled inheritance, traits, attitudes, migration, death, and save/load.

## In scope
- Person component groups and stable identity.
- Household members, guardians, dependents, home, pooled resources, debts/possessions, migration.
- Controlled inheritable baselines and environmental expression.
- Fast state, medium attitudes, slow personality.
- Capped social ties and meaningful experiences.
- Birth/death/inheritance/knowledge-carrier events and long-run fixtures.

## Required outputs
- [ ] People/household packages and DTOs.
- [ ] Trait/personal-state policy data.
- [ ] 100-year demographic/headless fixtures.
- [ ] Omniscient query/read models.

## Verification and acceptance
- [ ] 10,000 identities survive births/deaths/save/load without reuse.
- [ ] Politics/religion/patriotism are learned, not inherited.
- [ ] Traits modify declared decision terms but never bypass feasibility.
- [ ] Household resources and inheritance reconcile.
- [ ] Camera absence produces identical checkpoints.
- [ ] Documentation and relevant `codex.md` instructions match the implementation.
- [ ] Independent adversarial review reports no blocking findings.
- [ ] Creator-visible acceptance is recorded.

## Out of scope
- Detailed romance, exhaustive genealogy, pairwise relationship graph, organ simulation, or deterministic hereditary ideology.

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
