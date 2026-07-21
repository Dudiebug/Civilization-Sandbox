# Repository Instructions - Civilization Sandbox

## Mission
Build the lean regional Version 1.0 defined by ADR-001 while preserving the Blueprint v2.0 identity and architecture needed for the Version 1.5 complete early-modern target. Work one measured milestone and one approved task at a time. The repository, not conversation memory, is authoritative.

## Read before acting
1. `START_HERE.md`
2. `REVISION_SUMMARY.md`
3. `docs/plans/CURRENT_STEP.md`
4. The active `docs/plans/tasks/TASK-*.md`
5. Relevant scoped `codex.md`
6. Relevant specification, behavior contract, ADR, decision record, and tests

## Authority and release scope
- Blueprint v2.0 remains authoritative for north-star identity, technical principles, and Version 1.5 completeness.
- `docs/decisions/ADR-001_RELEASE_SCOPE_REBASELINE.md` governs release labels and staged scope.
- The active milestone defines what may be implemented now.
- A future package or schema doorway is not permission to build the future system.
- Do not invent unresolved product decisions. Check `docs/plans/DECISION_QUEUE.md`; stop when an open decision is required.

## Non-negotiable architecture
- D01 selects Unity `6000.3.20f1` for the Milestone 0.1 baseline; any later reconsideration requires an explicit creator decision and superseding ADR.
- Authoritative simulation is fixed-step, deterministic, camera-independent, headless-capable, and isolated from presentation.
- Stable domain IDs are independent of transient engine handles.
- Randomness is keyed and reproducible; do not use uncontrolled RNG.
- Authoritative quantities use explicit units and integer/fixed-point forms where required.
- Presentation consumes read-only snapshots and never owns simulation truth.
- AI is bounded coded intelligence with contracts, traces, candidate caps, and knowledge filters.
- Story systems may select, pace, frame, and summarize; they may not fabricate authoritative outcomes.
- Saves are semantic, versioned, validated, migrated, atomic, and recoverable.
- Normal player control uses Binding Orders; Force Outcome is a separate validated mutation path.
- The default player is omniscient, but simulated actors use only their own believed information.

## Version 1.0 identity guardrails
- Persistent named people.
- Physical settlement growth, damage, rebuilding, and ruins.
- Explainable societal divergence.
- Autonomous exact implementation.
- Omniscient inspection and basic causal explanation.
- Separate Binding Order, indirect intervention, and Force Outcome paths.
- Durable major events and physical scars.

## Scope controls
Do not implement Version 1.1-1.5 breadth, later ages, final art, live-LLM citizens, a universal technology tree, manual normal-city placement, full interiors, multiplayer, or black-box civilization intelligence during the pre-1.0 program unless the current milestone explicitly authorizes it.

## Work protocol
- One active task contract at a time unless the plan explicitly declares safe parallel work.
- Plan before editing. List touched files, tests, risks, rollback, exclusions, and decision dependencies.
- Keep diffs small and coherent. Do not perform opportunistic refactors.
- Never self-certify consequential work. Implementation and adversarial review are separate passes.
- Do not mark a task complete without an evidence pack and creator acceptance.
- Stop when scope, source-of-truth, expected behavior, or an unopened creator decision is required.

## Required completion evidence
Build, tests, deterministic replay/state diff, persistence impact, performance measurement, documentation, independent review, and creator-visible acceptance, proportional to the task's actual risk. Use `docs/templates/EVIDENCE_TEMPLATE.md`; mark genuinely irrelevant fields N/A with a reason.

## Creator-accessibility rules
Give the creator one decision at a time, short summaries first, exact commands in copyable blocks, and a clear stop/continue choice. Do not dump large unprioritized backlogs or ask for future-milestone choices early.
