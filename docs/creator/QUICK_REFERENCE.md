# Creator Quick Reference

## Today
Open `docs/plans/CURRENT_STEP.md`. Work only on that task.

## Model
- Plan or high-risk review: **5.6 Sol, Extra High**.
- Normal implementation: **5.6 Terra, High**.
- Small repetitive edits: **5.6 Luna, Medium**.
- Tiny real-time iteration: **5.3 Codex Spark**, only when available and tightly scoped.

## Prompt shape
**Goal → Context → Constraints → Done evidence → Stop conditions.**

## Never approve from a summary alone
Ask for: changed files, commands run, test/replay/save/performance results, risks, evidence location, and rollback.

## Completion
A task is crossed off only after:
1. automated verification;
2. independent adversarial review;
3. evidence pack;
4. your visible acceptance.

## Emergency stop phrase
`STOP. Do not edit. Summarize current state, uncommitted changes, failures, and the safest rollback.`
