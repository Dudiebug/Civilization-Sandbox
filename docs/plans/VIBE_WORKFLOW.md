# Vibe-First Development Workflow

This project is intended to be coded almost entirely through AI prompting. The creator directs the experience, plays builds, chooses tradeoffs, and accepts or rejects results. Codex handles code, tests, build commands, documentation, and rollback.

## Two planning levels

### Playable build

A build is a meaningful Unity experience, such as "people can found a camp" or "two societies visibly diverge." A build may take several AI sessions. It has a creator-facing acceptance playtest.

### Implementation slice

A slice is one observable change within the current build, normally completed in one focused AI session. Examples:

- people become hungry;
- a hungry person selects reachable food;
- gathered food enters a stockpile;
- delivered materials advance a hut;
- clicking a person explains the current action.

Slices are generated just in time. They are not maintained as a giant permanent backlog.

## The working loop

1. **Prompt:** give Codex the current build prompt and identify the next observable slice.
2. **Plan briefly:** Codex states the intended behavior, likely files, and verification in a short implementation note.
3. **Implement:** Codex changes only what the slice needs.
4. **Run:** Codex compiles/tests; the creator tries the result in Unity whenever the slice is visual or behavioral.
5. **Decide:** keep, adjust, or revert.
6. **Commit:** retain a known-working recovery point.
7. **Continue:** choose the next slice from actual play, not from stale assumptions.

## Repair rule

If two focused repair prompts fail to restore the intended behavior, return to the last working commit and re-approach the slice using what was learned. Do not build a chain of confused patches.

## Minimum automatic safety net

These protections earn their keep throughout the project:

- Unity must compile and launch after each accepted slice.
- Simulation truth must not depend on camera, frame rate, or presentation state.
- Uncontrolled randomness is prohibited in authoritative behavior.
- Save compatibility is checked whenever persisted state changes.
- Important resource creation/destruction and identity references are validated.
- A fast regression scenario protects each major behavior once that behavior becomes relied upon.

Everything else is added when a playable build proves it is needed.

## Evidence proportionality

- **Ordinary slice:** compile, focused test or scenario, Unity observation when relevant, working commit.
- **Risky foundation change:** deterministic comparison, rollback point, targeted review.
- **Playable build gate:** integrated playtest, save/reload, representative seeds, performance check, creator acceptance.
- **Release gate:** clean install, migrations, accessibility, long-run stability, packaging, independent review.

Independent adversarial review is reserved for risky architecture changes, build gates, and releases. It is not required after every small visual or tuning prompt.

## Context rule for Codex

Each session should read:

- the Blueprint sections relevant to the current build;
- `VISION_AUTHORITY.md`;
- the current build prompt;
- the implementation and test files directly involved.

Do not load the entire future roadmap into every implementation context.
