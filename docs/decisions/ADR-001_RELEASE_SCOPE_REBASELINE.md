# ADR-001 - Rebaseline Version 1.0 and move the original early-modern target to Version 1.5

**Status:** Accepted
**Date:** 2026-07-20
**Decision owner:** Creator
**Blueprint compatibility:** Supersedes Blueprint v2.0 only where that document labels the complete early-modern product as Version 1.0. It preserves the product identity, technical principles, and complete early-modern scope as the Version 1.5 horizon.

## Context
Blueprint v2.0 defines a studio-scale complete early-modern game as Version 1.0. Even a thin implementation requires many interacting domains, a 10,000-person scale gate, extensive UI, full diplomacy and war, ecosystems, player powers, chronicles, historical maps, and long-run release engineering. That is too broad for the first commercial release and creates pressure to make many product decisions before playable evidence exists.

## Decision
1. **Version 1.0** will be a lean regional civilization sandbox, working title **Founding Worlds**. The title is not locked.
2. Pre-release development is divided into Milestones **0.1 through 0.9**.
3. **Versions 1.1 through 1.4** are staged early-modern depth releases with working themes, not fixed feature commitments.
4. **Version 1.5** is the completion point for the original Blueprint v2.0 early-modern Version 1.0 product target.
5. **Version 2.0** remains the first industrialization expansion family.
6. Exact quantitative and content decisions are made at milestone gates, after relevant prototypes and measurements. Agents may not silently choose them early.

## Version 1.0 identity contract
Version 1.0 must still include a complete expression of:
- persistent named people;
- physically built and damaged settlements;
- societies that diverge because of conditions and institutions;
- autonomous exact implementation;
- omniscient inspection with basic causal explanation;
- separate order, intervention, and force-control paths;
- durable major history and physical scars.

## Consequences
### Benefits
- The first release can be coherent without implementing the entire early-modern simulation platform.
- Decisions are made closer to evidence and are easier for the creator to review.
- The project can use smaller worlds and fewer systems while preserving its distinctive causal loop.
- The original blueprint remains useful as a long-horizon architecture and content specification.

### Costs
- The Blueprint PDF and current production plan use different release labels; repository routing must explain the precedence clearly.
- Some architecture may be reserved before it is exercised at full depth.
- Save and content schemas must evolve across 1.0-1.5 without treating every future field as a Version 1.0 implementation requirement.

## Migration
- Rewrite the master roadmap, milestone plans, release checklists, first-task scopes, status tooling, and core summaries.
- Preserve the Blueprint PDF unchanged.
- Create a separate Version 1.5 completeness map containing the original full early-modern targets.
- Do not mark any implementation task complete as part of this planning revision.

## Evidence
- Creator direction: original Version 1.0 may become Version 1.5; the lean regional scope is preferred for the first release.
- Blueprint Sections 69-75 already support staged prototypes and acknowledge that the complete early-modern target is studio-scale.
- Revised planning kit validation and roadmap audit are recorded in `docs/evidence/ROADMAP-REBASELINE/EVIDENCE.md`.

## Reversal trigger
Reconsider only if measured milestone evidence and creator approval show that adding a specific post-1.0 capability materially improves the first release without endangering coherence, schedule, persistence, performance, or completion. Reversal does not happen because a feature is interesting or technically easy in isolation.
