# Model and Prompt Guide

Verified against official OpenAI Codex documentation on 2026-07-17.

## Which model to use

| Task | Model | Reasoning |
|---|---|---|
| Architecture, phase planning, risky migrations, root-cause analysis | 5.6 Sol | Extra High; use Max/Ultra only for genuinely decomposable hard work |
| Adversarial review, security, determinism, save integrity | 5.6 Sol | Extra High |
| Normal engine/project implementation from an approved task | 5.6 Terra | High |
| Tests, documentation updates, small refactors with clear boundaries | 5.6 Terra | Medium or High |
| Repetitive schema/content work with exact examples | 5.6 Luna | Medium |
| Tiny interactive edits or rapid compile-fix loops | 5.3 Codex Spark | Medium; only when available and scope is very small |

Use the strongest model for deciding **what** to build and reviewing consequences. Use the balanced model for building an already-approved slice. Use the fast model only after ambiguity is removed.

## Network and web research

The project config disables Codex web access during normal implementation. This protects reproducibility and prevents unpinned downloads. Use a separately approved bounded research task when current external facts are required, record authoritative sources and access dates, then return to offline implementation.

## Prompt formula

```
ROLE: [planner / implementer / verifier / reviewer]
TASK: [TASK-ID and one-sentence goal]
READ: [exact source files]
SCOPE: [allowed files and behavior]
DO NOT: [explicit exclusions]
DONE WHEN: [tests, replay, save, perf, docs, evidence]
STOP IF: [ambiguity, architecture conflict, failed prerequisite]
OUTPUT: [short result format]
```

## Planning prompt

```
Act as planner only. Do not edit files.
Read the active task, blueprint references, scoped codex.md files, current ADRs, and relevant tests.
Return: assumptions, affected boundaries, milestones of at most one coherent diff each, exact verification, rollback, risks, and questions that truly block work.
Keep the creator summary under 12 lines. Stop for approval.
```

## Implementation prompt

```
Implement only the approved milestone. Do not broaden scope or redesign neighboring systems.
Run the specified checks. Update contracts/docs in the same diff.
At the end, report changed files, commands, results, evidence path, known limitations, and whether the milestone is ready for independent review. Do not mark the task done.
```

## Review prompt

```
Review adversarially. Do not assume the implementation is correct.
Search for nondeterminism, hidden omniscience, authority violations, global scans, unbounded candidates, save incompatibility, duplicate concepts, invalid Force Outcome reconciliation, camera dependence, missing negative tests, and undocumented behavior.
Classify findings as BLOCKING, MAJOR, MINOR, or NOTE. Cite exact files and tests.
```

## Prompting rules that matter most
- Give Codex the goal, context, constraints, and definition of done.
- Use Plan mode for complex work.
- Specify verification commands and required evidence.
- Ask for small milestones and stop points.
- Do not ask it to “build the whole system,” “finish the AI,” or “make it smart.”
- Never let the same session plan, implement, verify, and approve a high-risk change.
