# TASK-001 Evidence Pack

**State:** IN PROGRESS — this file does not claim task completion.

## Environment

- Candidate branch: `task/TASK-001-bootstrap`
- Recovery baseline: `task-001-start-aec44fc` at `aec44fcb4285767716fbd22715fd96233459ab8e`
- Preserved creator edits: `prep/task001-approved-docs`
- OS/reference hardware: `Config/benchmark-reference.json`
- Unity/toolchain/package authority: `Config/toolchain.json`

## Required results

- Build: PASS locally — Windows and Linux x64 Mono players compile without package drift; clean-profile reproduction remains required.
- Tests: PASS locally — exactly one EditMode test and headless Windows smoke pass; clean-profile reproduction remains required.
- Replay/state diff: N/A — TASK-001 creates no authoritative state or replay format.
- Persistence/migration: N/A — TASK-001 creates no save format.
- Performance: PENDING — local timings are recorded below; clean-profile timings remain required and no runtime gate is established.
- Documentation: PASS — governance, ADR, command contract, CI, rollback, and deferred scopes are committed.
- Independent review: PENDING — see `REVIEW.md`.
- Creator acceptance: PENDING — see `ACCEPTANCE.md`.

## Evidence checklist

- [ ] Clean standard Windows profile and non-OneDrive clone details.
- [x] Prerequisite audit/install transcript; no license automation.
- [x] Two local bootstrap result JSON files with identical tracked state and lock hash.
- [x] Clean editor import with zero compiler errors.
- [x] Windows and Linux build result JSON, logs, binaries, and hashes.
- [x] NUnit XML showing exactly one passing deterministic EditMode test.
- [x] Headless Windows-player exit log.
- [x] Copied-lock tamper failure with `CIV001-LOCK-003`.
- [x] Cleanup dry-run, whitelist, and outside-root refusal demonstration.
- [x] Missing-evidence packaging failure.
- [ ] Live branch-rule export and successful `repository-policy` run.
- [ ] Evidence archive manifest and SHA-256.
- [ ] Recovery rehearsal from `baseline/task-001-accepted` after acceptance.

## Local implementation results — 2026-07-19 PDT

- Clean first import: PASS, Unity `6000.3.20f1`; 70.075-second initial asset refresh (99.85-second editor wall time); resolved direct packages exactly match the contract; no C# warnings or errors after the obsolete-API correction.
- Pre-amendment package lock: PASS at that run, SHA-256 `5dda07b9b8c1ab85cb77a5353a94a47414866f87ce222b3c8220d3d0d727e8df`; superseded by the creator-approved Linux toolchain amendment below.
- Repository-only bootstrap: PASS in 2.67 seconds.
- Same-profile clean clone in `C:\Users\dudie\AppData\Local\CivSandboxTask001Verification`: PASS for two repository-only bootstrap runs; tracked state and lock hash remained unchanged and the temporary clone was removed. This is not credited as the required new standard-user profile run.
- Windows x64 Mono player: PASS in 84.76 seconds; executable SHA-256 `34c4e304e53e56499267dfd9c975c63dc279ed3011a69a8ca16eb207f1856a8f`.
- EditMode/headless Windows smoke: PASS in 17.48 seconds; NUnit XML has one passed, zero failed; XML SHA-256 `c3067171e7defdfb6061957c3ae51575f3984c6e41db4a809b282d01fd07395e`.
- PowerShell guard suite: PASS, 12 tests, including a nested reparse-point escape refusal.
- Full audit negative path: PASS as a negative demonstration; stable missing-tool/module diagnostics and exit code 1 were retained under `Artifacts/negative/`.
- Evidence omission negative path: PASS as a negative demonstration; `Package-Evidence.ps1` refused to create an archive without the Linux executable.

## Local implementation results — 2026-07-20 PDT

- Pinned prerequisites: PASS — Git `2.55.0.3`, PowerShell `7.6.3`, Python `3.13.14`, GitHub CLI `2.96.0`, and Unity Hub `3.19.5`; GitHub CLI is authenticated as the repository owner.
- Unity modules: PASS — Windows Mono and Linux Mono are installed. The Hub-downloaded Linux Mono installer matched Unity's published MD5 metadata; observed SHA-256 `d3a70b44912f0ce21ca9b1ea198b20a09072bd6df4ed279d239a37d1cbc95ccd`.
- Full bootstrap idempotency: PASS after amendment — two consecutive audits returned zero, Git state plus both package files were unchanged, and package-lock SHA-256 remained `b5ab878043267eb78c9ece02004322353aa41947c938bc48c6850d838e206548`.
- PowerShell guard suite: PASS, 14 tests. Coverage now includes direct and nested path-redirecting reparse points, while allowing non-redirecting OneDrive cloud markers.
- Windows x64 Mono player rebuild: PASS; executable SHA-256 `34c4e304e53e56499267dfd9c975c63dc279ed3011a69a8ca16eb207f1856a8f`.
- Linux x64 Mono player rebuild: PASS; executable SHA-256 `8027f7d1f9ae7dacfc826fb218adcc1ae7464af098a8a59397e7531c0f7ec0bc`.
- Linux package-integrity gate: RESOLVED by explicit creator approval. Direct `com.unity.sdk.linux-x86_64@1.1.0` and `com.unity.toolchain.win-x86_64-linux@1.1.0`, with transitive `com.unity.sysroot.base@1.1.0`, are now part of the locked interface and build without tracked-file mutation.
- Unity wrapper termination: PASS after correction — build/test scripts wait on the editor process handle, avoiding PowerShell descendant-tree waits on Unity's long-lived Roslyn compiler server.
- Roadmap rebaseline: integrated from preserved branch `prep/roadmap-rebaseline-20260720`; active TASK-001 status and implementation evidence were retained, and `.codex/config.toml` remains absent by creator decision.

## Current blockers

1. Protected private-branch enforcement and the first successful `repository-policy` run remain pending.
2. Non-OneDrive clean standard-profile reproduction, independent review, creator acceptance, merge, accepted tag, and recovery rehearsal remain pending.

## Changed surface

Repository governance, pinned configuration, Unity-generated baseline settings, local tooling package, Windows commands, a single bootstrap test, minimal repository CI, and evidence documentation. No gameplay, save, replay, presentation, or player-facing domain API is included.

## Rollback

Follow `docs/core/REPOSITORY_GOVERNANCE.md`. Revert TASK-001 commits, remove only ignored generated paths through `Build/Clean.ps1`, and leave machine-installed software intact. Remove the required GitHub check before reverting its workflow.

## Final declaration

TASK-001 remains open until every pending line has reproducible evidence, independent review has no blocker, the creator records `Decision: APPROVE`, protected merge succeeds, and recovery from the accepted tag is rehearsed.
