# ADR-002 - Unity and Toolchain Baseline

**Status:** Accepted
**Date:** 2026-07-19
**Decision owner:** Creator
**Risk tier:** Critical
**Blueprint compatibility:** Implements the provisional Unity 6.3 LTS direction for the accepted Milestone 0.1 baseline without changing the later measured engine gate.

## Context

The first repository task needed a Windows bootstrap that could diagnose drift before gameplay code existed. Exact versions were observed and the selected Unity package graph was locally resolved, built, and verified. License activation remains an account-holder responsibility and must not be automated.

## Decision

Use Unity `6000.3.20f1` changeset `c9ba695d4f07`, Windows Mono, Linux Mono, and the exact direct packages recorded in `Config/toolchain.json`. The direct package graph includes Linux SDK `1.1.0` and the Windows-to-Linux toolchain `1.1.0`; both require transitive sysroot base `1.1.0`. Pin Git `2.55.0.3`, PowerShell `7.6.3`, Python `3.13.14`, GitHub CLI `2.96.0`, and Unity Hub `3.19.5` with the recorded installer hashes.

The bootstrap is an audit unless `-InstallPrerequisites` is supplied. An install never activates a Unity license, downgrades a newer tool, or removes software. A mismatch remains a failure requiring a human decision.

Hub CLI installation is used despite its deprecation because Hub `3.19.5` is pinned and its behavior is controlled for this baseline. Reversal requires a stable Unity installation interface whose executable and version can be independently pinned and verified.

The committed `Packages/packages-lock.json` SHA-256 is the package-integrity authority. A direct package pin and its resolved dependency graph must change together in a reviewed task.

## Consequences

- Bootstrap results are machine-readable and retain diagnostics and hashes.
- Fresh profiles need Git and WinGet before cloning, plus Unity account sign-in and license entitlement before editor/build commands.
- Newer tools are not silently accepted as the pinned baseline and are never automatically downgraded.
- TASK-001 measurements describe bootstrap/reference behavior; TASK-011 still owns the measured engine and scale ladder.

## Validation

TASK-001 clean-profile bootstrap, Windows/Linux builds, EditMode test, headless-player check, protected merge, recovery rehearsal, and creator acceptance passed.

## Rollback or supersession

Revert this ADR and `Config/toolchain.json`, or accept a superseding measured engine/toolchain ADR. Installed tools remain on the machine and are reported for optional manual removal; no repository script uninstalls them.

## Creator disclosure

This is the accepted development baseline, not a promise that every eventual Version 1.5 package is active or that later benchmark gates cannot trigger a reviewed engine decision.
