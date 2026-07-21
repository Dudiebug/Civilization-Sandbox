# Creator Quick Reference

## Today
Open `docs/plans/CURRENT_STEP.md`. Work only on that task.

## Scope
Version 1.0 is the lean regional release. Version 1.5 is the original complete early-modern target. Check `docs/plans/DECISION_QUEUE.md` before answering product questions; most decisions are intentionally deferred.

## Model
- Plan or high-risk review: **5.6 Sol, Extra High**.
- Normal implementation: **5.6 Terra, High**.
- Small repetitive edits: **5.6 Luna, Medium**.
- Tiny real-time iteration: **5.3 Codex Spark**, only when available and tightly scoped.

## Prompt shape
**Goal -> Context -> Constraints -> Decision dependencies -> Done evidence -> Stop conditions.**

## Never approve from a summary alone
Ask for changed files, commands run, test/replay/save/performance results, risks, evidence location, rollback, and any creator decision that was actually used.

## Completion
A task is crossed off only after automated verification, independent adversarial review, evidence pack, and visible creator acceptance.

## Emergency stop phrase
`STOP. Do not edit. Summarize current state, uncommitted changes, failures, unopened decisions, and the safest rollback.`
