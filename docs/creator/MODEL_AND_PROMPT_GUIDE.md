# Model and Prompt Guide

Verified against current official OpenAI documentation on 2026-07-20. These are prompt and task-metadata recommendations, not repository model configuration.

## Choose the model and reasoning separately

Model choice controls the capability, speed, and cost tradeoff. Reasoning effort controls how much work that model spends on the request. Choose from the work's ambiguity, consequence, test-oracle clarity, and volume:

- Prefer a stronger model when requirements conflict, architectural judgment is required, or a wrong answer would be expensive to unwind.
- Prefer a balanced model when the approved boundary and acceptance tests are clear.
- Prefer an efficient model for large exact transformations with a strong oracle. Do not delegate architecture or product decisions to this tier.
- Increase reasoning when semantic conflicts, edge cases, or consequential uncertainty remain. Do not use reasoning effort as a substitute for missing requirements.

| Work type | Recommended starting point | Escalation rule |
|---|---|---|
| Ambiguous architecture, high-risk planning, migrations, save/determinism analysis | 5.6 Sol, Extra High | Use Max only when the outcome is consequential and Extra High leaves material uncertainty. |
| Adversarial architecture, security, or replay review | 5.6 Sol, Extra High | Use Max for tightly evaluated, exceptionally difficult reviews. |
| Approved bounded implementation | 5.6 Terra, High | Medium is enough when interfaces and tests are completely specified. |
| Tests, documentation, and read-heavy scans | 5.6 Terra, Medium | Use High when reconciling semantic conflicts. |
| Large exact mechanical transformations | 5.6 Luna, Medium, when available | Otherwise use Terra, Medium; never assign architecture decisions to this tier. |
| Tiny text-only edit or compile loops | 5.3 Codex Spark, when available to the account | Avoid it for broad context, visual work, or consequential decisions. |
| Command-driven clean verification | 5.6 Terra, Medium | Switch to Sol, High if unexpected failures require interpretation. |

When uncertain, use the stronger model to decide and review, then Terra to implement the approved result.

## Choose a reasoning effort

- **Low or Medium:** mechanical work, clear transformations, read-heavy scans, and verification with an unambiguous oracle.
- **High:** complex implementation logic, interacting constraints, and edge-case analysis.
- **Extra High:** ambiguity, architecture, migrations, determinism, and consequential adversarial review.
- **Max:** rare, exceptionally difficult work with a tight evaluation loop where Extra High still leaves material uncertainty.
- **Ultra:** only when the creator explicitly requests parallel agents and the task separates into genuinely independent workstreams. It is not a synonym for deeper reasoning on a single inseparable problem.

## Instruction routing and network use

This repository intentionally contains neither `AGENTS.md` nor `.codex/config.toml`. Codex therefore must not claim that the repository's `codex.md` files load automatically. Every supplied session, planning, implementation, verification, and review prompt explicitly routes the agent to root `codex.md` and the nearest scoped `codex.md` files.

There is no committed project configuration that disables networking. Network availability and approval are session concerns. When current external facts are required, keep research bounded, prefer primary sources, and record the source and access date; implementation and CI validation remain offline-safe and must not download dependencies.

## Prompt formula

```
ROLE: [planner / implementer / verifier / reviewer]
TASK: [TASK-ID and one-sentence goal]
READ: [START_HERE.md, root codex.md, exact source files, nearest scoped codex.md]
MODEL: [recommended model and reasoning, with escalation condition]
SCOPE: [allowed files and behavior]
DO NOT: [explicit exclusions]
DONE WHEN: [tests, replay, save, perf, docs, evidence]
STOP IF: [ambiguity, architecture conflict, failed prerequisite]
OUTPUT: [short result format]
```

## Planning prompt

```
Act as planner only. Do not edit files.
Manually read START_HERE.md, root codex.md, the active task, Blueprint references,
nearest scoped codex.md files, current ADRs, and relevant tests.
Recommend a model and reasoning effort from ambiguity and consequence; do not create model config.
Return: assumptions, affected boundaries, milestones of at most one coherent diff each,
exact verification, rollback, risks, and questions that truly block work.
Keep the creator summary under 12 lines. Stop for approval.
```

## Implementation prompt

```
Implement only the approved milestone. Manually read root and nearest scoped codex.md files.
Do not broaden scope or redesign neighboring systems. Use the task's model guidance as advice,
not configuration. Run the specified checks and update contracts/docs in the same diff.
At the end, report changed files, commands, results, evidence path, known limitations,
and whether the milestone is ready for independent review. Do not mark the task done.
```

## Review prompt

```
Review adversarially. Manually read root and nearest scoped codex.md files.
Do not assume the implementation is correct. Search for nondeterminism, hidden omniscience,
authority violations, global scans, unbounded candidates, save incompatibility, duplicate concepts,
invalid Force Outcome reconciliation, camera dependence, missing negative tests, and undocumented behavior.
Classify findings as BLOCKING, MAJOR, MINOR, or NOTE. Cite exact files and tests.
```

## Prompting rules that matter most

- Give Codex the goal, source context, constraints, and definition of done.
- Route `codex.md` manually; do not rely on discovery that this repository deliberately does not configure.
- Use Plan mode for complex work and name the verification commands and required evidence.
- Ask for small milestones and explicit stop conditions.
- Do not ask an agent to “build the whole system,” “finish the AI,” or “make it smart.”
- Never let the same session plan, implement, verify, and approve a high-risk change.

## Official sources

- [OpenAI latest model guidance](https://developers.openai.com/api/docs/guides/latest-model) — accessed 2026-07-20.
- [Codex project-instruction discovery](https://learn.chatgpt.com/docs/config-file/config-advanced#project-instructions-discovery) — accessed 2026-07-20.
