# Web Research Notes — 2026-07-17

## OpenAI Codex workflow
Official OpenAI documentation currently lists:

- **GPT-5.6 Sol** (`gpt-5.6-sol`) as the flagship/strongest option for complex coding, computer use, research, and security work.
- **GPT-5.6 Terra** (`gpt-5.6-terra`) as the balanced everyday option.
- **GPT-5.6 Luna** (`gpt-5.6-luna`) as the fast, lowest-cost option in the family.
- **GPT-5.3 Codex Spark** (`gpt-5.3-codex-spark`) as a text-only research preview for near-instant iteration.
- Higher reasoning effort improves hard tasks at additional time/token cost; Plan mode and scoped milestones are recommended for complex work.
- The Codex config key `project_doc_fallback_filenames` allows `codex.md` to be loaded when `AGENTS.md` is absent.
- `review_model`, `plan_mode_reasoning_effort`, workspace-write sandboxing, approval policy, and network controls are supported project configuration concepts.

Official sources:
- https://learn.chatgpt.com/docs/models
- https://learn.chatgpt.com/guides/best-practices
- https://learn.chatgpt.com/docs/config-file/config-reference

## Unity production stack
Official Unity sources support the blueprint's provisional direction: Unity 6.3 is an LTS release, Entities is the data-oriented ECS package, Entities Graphics supports DOTS rendering/LOD workflows, and Unity supports command-line build/test operation. Exact editor patch and package versions must be re-verified and pinned in TASK-001 rather than copied from a web page into this kit.

Official starting sources:
- https://unity.com/releases/unity-6/support
- https://docs.unity3d.com/Manual/EditorCommandLineArguments.html
- Unity Test Framework and Entities package documentation for the exact pinned editor/package versions.

## Decision impact
The repository defaults to Terra/High for normal implementation and Sol/Extra High for planning/review. Web access is disabled in normal project execution; bounded research tasks may enable it deliberately and record sources.
