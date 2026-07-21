# ADR-001: Unity and Toolchain Baseline

**Status:** Accepted for TASK-001 implementation
**Date:** 2026-07-19

## Context

The first repository task needs a Windows bootstrap that can diagnose drift before any gameplay code exists. Exact versions were observed and the selected Unity package graph was locally resolved and compiled. License activation is an account-holder responsibility and must not be automated.

## Decision

Use Unity `6000.3.20f1` changeset `c9ba695d4f07`, Windows Mono, Linux Mono, and the exact direct packages recorded in `Config/toolchain.json`. The direct package graph includes Linux SDK `1.1.0` and the Windows-to-Linux toolchain `1.1.0`; both require transitive sysroot base `1.1.0`. Pin Git `2.55.0.3`, PowerShell `7.6.3`, Python `3.13.14`, GitHub CLI `2.96.0`, and Unity Hub `3.19.5` with the recorded installer hashes.

The bootstrap is an audit unless `-InstallPrerequisites` is supplied. An install never activates a Unity license, downgrades a newer tool, or removes software. A mismatch remains a failure requiring a human decision.

Hub CLI installation is used despite its deprecation because Hub `3.19.5` is itself pinned and its behavior is controlled for this baseline. Reversal requires a stable Unity installation interface whose executable and version can be independently pinned and verified.

The committed `Packages/packages-lock.json` SHA-256 is the package-integrity authority. A direct package pin and its resolved dependency graph must change together in a reviewed task.

## Consequences

- Bootstrap results are machine-readable and retain diagnostics and hashes.
- Fresh profiles need Git and WinGet before cloning, plus Unity account sign-in and license entitlement before editor/build commands.
- Newer tools are not silently accepted as the pinned baseline and are never automatically downgraded.
- TASK-001 measures import/build/test duration on the creator machine only; it does not establish the later performance gate.

## Rollback

Revert this ADR and `Config/toolchain.json`. Installed tools remain on the machine and are reported for optional manual removal; no repository script uninstalls them.
