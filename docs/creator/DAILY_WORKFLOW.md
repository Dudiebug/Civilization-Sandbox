# Daily Workflow

## Start — 5 minutes
1. Open the next selected `docs/plans/tasks/BUILD-*.md` prompt.
2. Check that only one playable slice is active.
3. Review the last working commit and handoff.
4. Choose the model from `MODEL_AND_PROMPT_GUIDE.md`.

## Work cycle
Prompt → approve one slice → implement → run in Unity → creator judgment → commit or revert.

## Stop conditions
Stop immediately for an unexplained test failure, source-of-truth conflict, growing scope, nondeterministic result, save break, performance regression, or ambiguous product decision.

## End — 5 minutes
Use `docs/prompts/07_END_SESSION_HANDOFF.md`. Update the task status, next exact action, changed files, failures, and evidence paths. Never rely on remembering the chat tomorrow.

## Low-energy mode
Do only one of these: read a short plan, run an existing verification command, perform a creator-visible test, or approve/reject one clearly stated decision. Do not begin new architecture work.
