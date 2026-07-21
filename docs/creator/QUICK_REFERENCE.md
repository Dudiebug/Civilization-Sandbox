# Creator Quick Reference

## Today

Open the next selected `docs/plans/tasks/BUILD-*.md`. Work on only that build and one playable slice at a time. Prompts must manually route the agent to root and nearest scoped `codex.md` files; this repository does not configure automatic discovery.

## Scope

Version 1.0 is the lean regional release. Version 1.5 is the original complete early-modern target, followed by the preserved later-age and distant-expansion vision. Use `docs/plans/DECISION_GATES.md` before making a material product choice.

## Model

| Need | Start with |
|---|---|
| Ambiguous planning, determinism/save analysis, consequential review | **5.6 Sol, Extra High** |
| Approved bounded implementation | **5.6 Terra, High** |
| Tests, docs, read-heavy scans, clean verification | **5.6 Terra, Medium** |
| Large exact mechanical change | **5.6 Luna, Medium**, when available |
| Tiny text-only loop | **5.3 Codex Spark**, when available and tightly scoped |

Use Max only for exceptionally difficult, tightly evaluated consequential work. Use Ultra only when you explicitly request parallel agents and the work genuinely separates. When unsure: use Sol to decide and review, then Terra to implement.

## Prompt shape

**Goal -> Context -> manual instruction routing -> Constraints -> Decision dependencies -> Model/reasoning guidance -> Done evidence -> Stop conditions.**

## Never approve from a summary alone

Ask for changed files, commands run, test/replay/save/performance results, risks, evidence location, rollback, and any creator decision that was actually used.

## Completion

A slice is kept only after it runs and the creator accepts the visible direction. A complete build also receives proportional automated verification, independent review, and recorded creator acceptance.

## Emergency stop phrase

`STOP. Do not edit. Summarize current state, uncommitted changes, failures, unopened decisions, and the safest rollback.`
