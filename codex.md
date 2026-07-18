# Repository Instructions — Civilization Sandbox

## Mission
Build the complete early-modern Version 1.0 civilization sandbox defined by the authoritative blueprint. Work one measured gate at a time. The repository, not conversation memory, is authoritative.

## Read before acting
1. `START_HERE.md`
2. `docs/plans/CURRENT_STEP.md`
3. The active `docs/plans/tasks/TASK-*.md`
4. Relevant scoped `codex.md`
5. Relevant specification, behavior contract, ADR, and tests

## Non-negotiable architecture
- Unity 6.3 LTS is provisional until the Phase 1 engine gate passes.
- Authoritative simulation is fixed-step, deterministic, camera-independent, headless-capable, and isolated from presentation.
- Stable domain IDs are independent of transient ECS handles.
- Randomness is keyed and reproducible; do not use uncontrolled RNG.
- Authoritative quantities use explicit units and integer/fixed-point forms where required.
- Presentation consumes read-only snapshots and never owns simulation truth.
- AI is bounded coded intelligence with contracts, traces, candidate caps, and knowledge filters.
- Story systems may select, pace, frame, and summarize; they may not fabricate authoritative outcomes.
- Saves are semantic, versioned, validated, migrated, atomic, and recoverable.
- Normal player control uses Binding Orders; Force Outcome is a separate validated mutation path.
- The default player is omniscient, but simulated actors use only their own believed information.

## Scope controls
Do not implement later ages, final art, live-LLM citizens, a universal technology tree, manual normal-city placement, full interiors, multiplayer, or black-box civilization intelligence during the first-playable program.

## Work protocol
- One active task contract at a time unless the plan explicitly declares safe parallel work.
- Plan before editing. List touched files, tests, risks, rollback, and exclusions.
- Keep diffs small and coherent. Do not perform opportunistic refactors.
- Never self-certify consequential work. Implementation and adversarial review are separate passes.
- Do not mark a task complete without an evidence pack and creator acceptance.
- Stop when scope, source-of-truth, or expected behavior is ambiguous.

## Required completion evidence
Build, tests, deterministic replay/state diff, persistence impact, performance measurement, documentation, independent review, and creator-visible acceptance. Use `docs/templates/EVIDENCE_TEMPLATE.md`.

## Creator-accessibility rules
Give the creator one decision at a time, short summaries first, exact commands in copyable blocks, and a clear stop/continue choice. Do not dump large unprioritized backlogs into chat.
