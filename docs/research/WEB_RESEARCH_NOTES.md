# Web Research Notes — updated 2026-07-19

## OpenAI Codex workflow
Official OpenAI documentation currently lists:

- **GPT-5.6 Sol** (`gpt-5.6-sol`) as the flagship/strongest option for complex coding, computer use, research, and security work.
- **GPT-5.6 Terra** (`gpt-5.6-terra`) as the balanced everyday option.
- **GPT-5.6 Luna** (`gpt-5.6-luna`) remains part of the published family, but the creator chose not to use it in this project's active workflow.
- Higher reasoning effort improves hard tasks at additional time/token cost; Plan mode and scoped milestones are recommended for complex work.
- Max gives a single agent more reasoning time than Extra High. Ultra coordinates multiple agents, so it is reserved for approved work that divides cleanly into independent workstreams.
- Fast mode increases supported-model speed at higher credit consumption; the creator chose the Standard tier and disabled Fast mode for this project.
- The Codex config key `project_doc_fallback_filenames` allows `codex.md` to be loaded when `AGENTS.md` is absent.
- `review_model`, `plan_mode_reasoning_effort`, workspace-write sandboxing, approval policy, and network controls are supported project configuration concepts.

Official sources:
- https://learn.chatgpt.com/docs/models
- https://learn.chatgpt.com/guides/best-practices
- https://learn.chatgpt.com/docs/config-file/config-reference
- https://developers.openai.com/api/docs/guides/model-guidance?model=gpt-5.6
- https://openai.com/index/gpt-5-6/

## Unity production stack
Official Unity sources support the blueprint's provisional direction: Unity 6.3 is an LTS release, Entities is the data-oriented ECS package, Entities Graphics supports DOTS rendering/LOD workflows, and Unity supports command-line build/test operation. Exact editor patch and package versions must be re-verified and pinned in TASK-001 rather than copied from a web page into this kit.

Official starting sources:
- https://unity.com/releases/unity-6/support
- https://docs.unity3d.com/Manual/EditorCommandLineArguments.html
- Unity Test Framework and Entities package documentation for the exact pinned editor/package versions.

## Decision impact
The repository defaults to Terra/High for normal work. High-risk planning/review uses Sol/Extra High; Critical planning/review uses Sol/Max. Luna and Fast mode are excluded from the active project workflow. Web access is disabled in normal project execution; bounded research tasks may enable it deliberately and record sources.
