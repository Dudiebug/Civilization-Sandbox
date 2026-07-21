# Creator Quick Reference

## Today

Open `docs/plans/CURRENT_STEP.md`. Work only on that task. Prompts must manually route the agent to root and nearest scoped `codex.md` files; this repository does not configure automatic discovery.

## Scope

Version 1.0 is the lean regional release. Version 1.5 is the original complete early-modern target. Check `docs/plans/DECISION_QUEUE.md` before answering product questions; most decisions are intentionally deferred.

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

A task is crossed off only after automated verification, independent adversarial review, evidence pack, and visible creator acceptance.

## Emergency stop phrase

`STOP. Do not edit. Summarize current state, uncommitted changes, failures, unopened decisions, and the safest rollback.`
