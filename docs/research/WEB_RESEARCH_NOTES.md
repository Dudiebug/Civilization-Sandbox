# Web Research Notes — 2026-07-20

External URLs are research references only. Repository and CI validation do not fetch them.

## OpenAI model guidance

The current official model guide positions:

- **5.6 Sol** as the frontier choice for the most demanding, ambiguous work;
- **5.6 Terra** as the balance of intelligence, speed, and cost for everyday work;
- **5.6 Luna** as the efficient choice for high-volume, cost-sensitive work; and
- reasoning effort as a separate control, with Max reserved for the hardest tasks.

Codex documentation also describes **5.3 Codex Spark** as a limited-availability, near-instant text-only option. Availability can vary by account, so repository guidance treats it as optional rather than required.

Official sources, accessed 2026-07-20:

- [Latest model guide](https://developers.openai.com/api/docs/guides/latest-model)
- [Codex models](https://learn.chatgpt.com/docs/models)

## Codex instruction discovery

Official Codex discovery normally uses `AGENTS.md` or configured fallback filenames. The creator permanently chose not to commit `AGENTS.md` or `.codex/config.toml`, so this repository does not claim that `codex.md` loads automatically. Supplied prompts must tell agents to read root and nearest scoped `codex.md` files manually.

There is no project configuration that disables networking. Network access and approval are controlled by the active session. Current external facts may be researched in a bounded task with authoritative sources and access dates; documentation validation itself remains deterministic and offline-safe.

Official source, accessed 2026-07-20:

- [Codex project-instruction discovery](https://learn.chatgpt.com/docs/config-file/config-advanced#project-instructions-discovery)

## Unity production stack

The accepted editor and package baseline is recorded in `docs/decisions/ADR-002_UNITY_TOOLCHAIN_BASELINE.md`. Web sources are not permitted to silently replace that accepted record. Future upgrades require a superseding ADR with exact versions and compatibility evidence.

Official starting sources, accessed 2026-07-20:

- [Unity 6 release support](https://unity.com/releases/unity-6/support)
- [Unity command-line arguments](https://docs.unity3d.com/Manual/EditorCommandLineArguments.html)

## Decision impact

Use Sol/Extra High for ambiguous or consequential decisions and adversarial review, Terra/High for bounded implementation, and Terra/Medium for tests, documentation, read-heavy scans, and clean verification. Use Luna only for exact high-volume transformations, and optional Spark only for tiny text-only loops. These are recommendations in prompts and task metadata, never committed model configuration.
