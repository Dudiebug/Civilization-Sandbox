# Creator Quick Reference

## Today
Open `docs/plans/CURRENT_STEP.md`. Work only on that task.

## Model
- Open the model control and select **Advanced** to expose individual model and reasoning choices.
- Critical planning or review: **5.6 Sol, Max**.
- High-risk planning or review: **5.6 Sol, Extra High**.
- Normal implementation, testing, documentation, exploration, and debugging: **5.6 Terra, High**.
- Exact mechanical work with no unresolved decisions: **5.6 Terra, Medium**.
- Use **Ultra** only when an approved plan names independent parallel workstreams. Do not use Luna or Fast mode for this project.

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
