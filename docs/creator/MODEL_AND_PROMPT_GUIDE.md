# Model and Prompt Guide

Verified against official OpenAI Codex documentation and GPT-5.6 guidance on 2026-07-19.

## Which model to use

| Task | Model | Reasoning |
|---|---|---|
| Critical task planning, irreversible architecture, determinism, saves, or migrations | 5.6 Sol | Max |
| High-risk planning, difficult diagnosis, or adversarial review | 5.6 Sol | Extra High |
| Normal C#/Unity implementation from an approved task | 5.6 Terra | High |
| Tests, documentation, exploration, debugging, or small bounded refactors | 5.6 Terra | High |
| Highly repetitive edits with exact examples and no unresolved decisions | 5.6 Terra | Medium |

Use Sol for deciding **what** to build and reviewing consequential work. Use Terra for building and verifying an approved slice. This project does not use Luna or Codex Spark.

## Picker, Max, Ultra, and Fast mode

The repository config sets the project default, but it does not populate the app's model menu. Open the model control beneath the composer and select **Advanced** to choose Sol or Terra with a specific reasoning level.

- **Max** is the deepest single-agent reasoning level. Use it for Critical task plans and other decisions where a mistake would be expensive to reverse.
- **Ultra** coordinates multiple agents and uses more total tokens. Use it only when an approved plan explicitly declares independent parallel workstreams; it is not the default for Critical tasks.
- **Fast mode** trades higher credit consumption for lower latency. It is disabled for this project; use the Standard service tier.

OpenAI's published GPT-5.6 coding results support Terra as the routine default: Sol/Terra score 80.0/77.4 on the Artificial Analysis Coding Agent Index, 64.6/63.4 on SWE-Bench Pro, and 88.8/87.4 on Terminal-Bench 2.1. These are benchmark results, not guaranteed Codex-app completion times. Source: https://openai.com/index/gpt-5-6/

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
