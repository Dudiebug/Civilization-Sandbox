# Repository Instructions - Civilization Sandbox

## Mission
Build the complete Civilization Sandbox vision through the vibe-first playable-build roadmap. Work on one selected build and one playable implementation slice at a time. The Blueprint and repository, not conversation memory, are authoritative.

## Read before acting
1. `START_HERE.md`
2. `docs/DOCUMENT_INDEX.md`
3. `REVISION_SUMMARY.md`
4. `docs/plans/VISION_AUTHORITY.md`
5. `docs/plans/FULL_ROADMAP.md`
6. The selected `docs/plans/tasks/BUILD-*.md`
7. Relevant scoped `codex.md`
8. Relevant specification, behavior contract, ADR, decision record, and tests

## Authority and release scope
- Blueprint v2.0 remains authoritative for north-star identity, technical principles, and Version 1.5 completeness.
- `docs/decisions/ADR-001_RELEASE_SCOPE_REBASELINE.md` governs release labels and staged scope.
- `docs/decisions/ADR-002_UNITY_TOOLCHAIN_BASELINE.md` governs the accepted Unity/toolchain baseline.
- `docs/decisions/ADR-003_CODEX_INSTRUCTION_DISCOVERY.md` requires manual prompt routing and forbids repository `AGENTS.md` and `.codex/config.toml`.
- The selected playable build defines what may be implemented now.
- A future package or schema doorway is not permission to build the future system.
- Do not silently make a product decision that changes the vision. Use `docs/plans/DECISION_GATES.md` and ask the creator when required.

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
- One active playable build at a time. Split it into 3-8 playable slices and implement one slice at a time.
- Plan each slice before editing. State the player-visible result, touched boundaries, verification, rollback, and exclusions.
- Keep diffs small and coherent. Do not perform opportunistic refactors.
- Run Unity and preserve a working commit after each accepted slice. Revert failed directions early.
- Use formal independent review and creator acceptance at playable-build and release gates, proportional to risk.
- Stop when scope, source-of-truth, expected behavior, or an unopened creator decision is required.

## Permanent project location
- The sole authoritative checkout and Unity project is `C:\Users\dudie\Projects\Civilization-Sandbox`.
- Do not create sibling worktrees, secondary development clones, copied Unity projects, or alternate project folders anywhere else on the creator's PC.
- All branches are handled by switching branches in this one checkout after preserving and publishing the current branch.
- The only permitted local duplicate is a disposable verification clone created and removed within one guarded verification command; it must never be registered in Unity Hub or retained as a development copy.
- Any change to this location or one-folder policy requires an explicit creator question and recorded approval before acting.

## Required completion evidence
Build, tests, deterministic replay/state diff, persistence impact, performance measurement, documentation, independent review, and creator-visible acceptance, proportional to the task's actual risk. Use `docs/templates/EVIDENCE_TEMPLATE.md`; mark genuinely irrelevant fields N/A with a reason.

## Creator-accessibility rules
Give the creator one decision at a time, short summaries first, exact commands in copyable blocks, and a clear stop/continue choice. Do not dump large unprioritized backlogs or ask for future-milestone choices early.
