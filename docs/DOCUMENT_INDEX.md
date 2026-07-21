# Source of Truth and Document Index

This index is the human routing map for authoritative project documents. `Config/document-registry.json` owns machine-readable document identity, owner, status, release scope, and reachability requirements.

## Authority order

1. The immutable [Blueprint v2.0](blueprint/Civilization_Sandbox_Game_Development_Blueprint_v2.0.pdf) owns the north-star identity, technical principles, and complete early-modern Version 1.5 horizon.
2. Accepted ADRs may supersede only the exact scope they name. [ADR-001](decisions/ADR-001_RELEASE_SCOPE_REBASELINE.md) changes release labels and staging; it does not rewrite the Blueprint's identity or technical principles.
3. Approved release and milestone plans activate work and defer unopened decisions. They cannot silently change higher authority.
4. Current specifications and behavior contracts own implementation boundaries and observable rules.
5. The active task contract narrows an approved objective into one auditable change. It cannot override an ADR or specification without an approved amendment.
6. Code and tests implement the documents above. Evidence records what passed; neither code nor evidence silently changes intended behavior.
7. Chat history is context only.

When two sources conflict, stop at the highest conflicting layer, record the exact statements, and request an ADR or task amendment. Do not choose the convenient statement.

## Start and operating instructions

- [Repository overview](../README.md)
- [Start Here](../START_HERE.md)
- [Repository instructions](../codex.md)
- [Vision authority](plans/VISION_AUTHORITY.md)
- [Playable build prompts](plans/tasks/README.md)
- [Creator quick reference](creator/QUICK_REFERENCE.md)
- [Model and prompt guide](creator/MODEL_AND_PROMPT_GUIDE.md)

`codex.md` is a repository convention that must be opened explicitly by the session prompt. The repository contains neither `AGENTS.md` nor `.codex/config.toml`, so it does not claim automatic Codex instruction discovery.

## Accepted decisions

- [ADR-001 - Release-scope rebaseline](decisions/ADR-001_RELEASE_SCOPE_REBASELINE.md)
- [ADR-002 - Unity and toolchain baseline](decisions/ADR-002_UNITY_TOOLCHAIN_BASELINE.md)
- [ADR-003 - Manual Codex instruction routing](decisions/ADR-003_CODEX_INSTRUCTION_DISCOVERY.md)

## Playable-build roadmap

- [Roadmap kit overview](plans/README.md)
- [Full roadmap](plans/FULL_ROADMAP.md)
- [Vision authority](plans/VISION_AUTHORITY.md)
- [Scope coverage](plans/SCOPE_COVERAGE.md)
- [Vibe workflow](plans/VIBE_WORKFLOW.md)
- [Decision gates](plans/DECISION_GATES.md)
- [Source and change notes](plans/SOURCE_AND_CHANGE_NOTES.md)
- [Downloadable roadmap ZIP](plans/Civilization_Sandbox_Vibe_First_Full_Roadmap_Kit.zip)

## Architecture, data, and verification

- [Technical architecture](core/TECHNICAL_ARCHITECTURE.md)
- [Planned package map](../GamePackages/PACKAGE_MAP.md)
- [Authoritative unit catalog](core/UNIT_CATALOG.md)
- [Simulation contracts](core/SIMULATION_CONTRACTS.md)
- [Data, identity, history, and saves](core/DATA_AND_SAVE.md)
- [Performance budgets](core/PERFORMANCE_BUDGETS.md)
- [Risk register](core/RISK_REGISTER.md)
- [Testing and evidence](core/TESTING_AND_EVIDENCE.md)

## Coded intelligence

- [AI golden rules](ai/AI_GOLDEN_RULES.md)
- [Decision authority matrix](ai/DECISION_AUTHORITY.md)
- [AI Behavior Contract template](ai/BEHAVIOR_CONTRACT_TEMPLATE.md)
- [Behavior Contract catalog](ai/BEHAVIOR_CATALOG.md)
- [Decision trace schema](ai/TRACE_SCHEMA.md)
- [AI Decision Laboratory](ai/DECISION_LAB.md)
- [AI regression strategy](ai/REGRESSION_STRATEGY.md)
- [Regression ledger](REGRESSION_LEDGER.md)

## Story and history

- [Story authority](story/STORY_AUTHORITY.md)
- [Structured historical event schema](story/EVENT_SCHEMA.md)
- [History retention](story/HISTORY_RETENTION.md)
- [Chronicle rules](story/CHRONICLE_RULES.md)
- [Incident Candidate Contract template](story/INCIDENT_CONTRACT_TEMPLATE.md)

## Evidence and review

- [Evidence pack template](templates/EVIDENCE_TEMPLATE.md)
- [Adversarial review checklist](templates/REVIEW_CHECKLIST.md)

## Maintenance rule

Add a source-of-truth document to the registry and this index in the same change. Run `python Build/validate_docs.py`; an unregistered, unreachable, duplicated, or broken internal reference is a repository error.
