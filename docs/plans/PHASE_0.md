# Milestone 0.1 - Project Foundation

**Status:** Not Started
**Task range:** TASK-001 through TASK-010

**Detailed program context:** `PRE_1_0_ROADMAP.md`, Section 5.

## Purpose
Create a reproducible, deterministic, migration-aware project shell before gameplay state exists.

## Player-visible outcome
No finished gameplay is required. A headless baseline can advance deterministic state, record a simple event, save, load, replay, and explain the first divergence.

## Required work
- Repository governance, clean bootstrap, build/test/evidence commands, and CI.
- Maintain the accepted Unity/toolchain path and exact pins recorded by D01 and ADR-002; current Unity-shaped files are governed integration boundaries.
- Architecture and dependency gates.
- Typed authoritative time and scheduler shell.
- Stable nonreused identity.
- Keyed random streams.
- Minimum causal event record.
- Canonical checksum/state diff.
- Semantic atomic persistence skeleton.

## Decisions opened in this milestone
- D01 engine/toolchain path and exact pins — accepted and locked by ADR-002.
- D02 reference development hardware and supported build hosts — accepted in TASK-001 evidence.

No world-size, population, art, economy, politics, combat, content-count, or release decisions are opened.

## Exit gate
From a clean supported environment, the selected engine/toolchain can bootstrap, build, run a headless deterministic fixture, record a stable event, save/load with checksum equality, diagnose a deliberate divergence, and package evidence. Deliberate architecture and RNG violations fail automatically.

## Stop/redesign triggers
- Clean bootstrap depends on undocumented manual repair.
- Engine/toolchain cannot support headless tests or semantic state separation.
- Replay changes with scheduling or presentation.
- Save/load cannot preserve stable identity and event references.

## Explicitly deferred
Spatial scale, full routing, rendering density, gameplay AI, people, terrain, settlements, and all content breadth.
