# ADR-004 - Configurable Hierarchical World Creation

**Status:** Accepted
**Date:** 2026-07-21
**Decision owner:** Creator
**Risk tier:** High
**Blueprint compatibility:** Strengthens deterministic geography, physical causality, scale transitions, and the preserved multi-planet horizon without activating distant-age gameplay early.

## Context

Build 01 intentionally used a small diamond-shaped diagnostic field. The creator rejected that shape as the product direction and approved a world-creation experience structurally inspired by configurable colony-simulation world setup, presented as a colorful living top-down world, with selected three-dimensional environmental elements. The same identity model must be able to grow into the preserved multi-planet stage without making Build 02 implement space systems.

## Decision

- Build 02 introduces the first playable world-creation path: **Title Screen -> New Game -> World Setup -> Generated Preview -> Founding-Site Selection -> Play**.
- World Setup exposes bounded, named world-size presets and creator-facing procedural controls. Initial controls include a deterministic seed and measured envelopes for land/water balance, temperature, rainfall, mountains, forests, and resource abundance. Exact preset dimensions and ranges remain provisional until performance and play evidence support them.
- A seed plus canonical generation settings must reproduce the same semantic world. The preview and playable world are projections of that same generated state, not separate decorative generations.
- Geography must materially constrain survival and later history. Water, traversability, fertility, vegetation, stone, and resource placement cannot be cosmetic-only.
- Presentation uses a hybrid 2.5D direction: top-down pixel readability with bounded three-dimensional terrain elevation, water depth, cliffs, vegetation, rocks, and later structures. Presentation detail, camera state, visibility, and rendering cannot alter authoritative generation or simulation.
- Semantic identity is hierarchical and nonreused across generated worlds, celestial bodies, regions, sites, and entities. Concrete schemas are introduced only by the playable slice that needs them.
- Build 20 extends this hierarchy through orbital and interplanetary play. Build 02 does not implement planets, orbital topology, interplanetary travel, or distant-age content.
- Generated image assets are not required for this direction. Procedural geometry, project-owned materials, and authored assets may be evaluated through normal playable slices.

### Creator scale amendment - 2026-07-21

- Build 02 terrain cells are person-scale ground tiles in the playable presentation. One standing person occupies approximately one tile; a cell is not a giant region containing an entire visible camp.
- The current provisional presets are **Small 96x96**, **Standard 384x384**, and **Max 1024x1024** tiles. The 1024x1024 preset is the current Build 02 ceiling, not a final release-scale promise.
- The terrain preview uses chunked geometry and camera culling so the Max preset does not require one monolithic million-cell mesh or collider.
- The default view presents the world from a near-top-down orthographic angle while preserving readable elevation. The player can toggle to a stronger angled view, rotate, pan, and zoom; neither camera mode can affect authoritative state.
- A founding tile must provide a bounded walkable local footprint for the initial company; merely coloring a tile as land is insufficient if the legacy local movement area would overlap water.

### Creator resource and camera amendment - 2026-07-21

- Terrain colors use hard person-scale cell boundaries and faceted lighting instead of interpolated color gradients. Pixel sprites and terrain must use point-like, crisp visual decisions rather than blurred texture filtering.
- Build 02 activates deterministic source fields for **fresh water, staple food, protein food, timber, stone, clay, fiber, iron ore, coal, and medicinal inputs**. Surface sources receive bounded visible markers; underground resources remain semantic and are only exposed visually where geology makes them surface-readable.
- The complete Blueprint commodity set remains 18 classes. **Seed stock, fuelwood, lumber, masonry/brick, tools, construction goods, textiles/consumer goods, and weapons/ammunition** are processed outputs activated with the recipes, workplaces, skills, and technical capabilities that can actually produce or use them. They are not scattered across the generated map as fake deposits.
- Resource generation in this slice establishes stable semantic availability and visual density. Extraction, depletion, reservations, hauling, stockpile conservation, processing, and capability-gated use remain the connected follow-on slices required before resources become fully playable.

### Creator camp-loop and calendar amendment - 2026-07-21

- At the default **1x** control, calendar time advances at **five world seconds per real second**. Ordinary person movement and labor remain at one actor-second per real second; the calendar scale does not silently multiply physical activity. Explicit 2x, 5x, and 10x controls accelerate both after the player chooses them.
- The first connected resource slice activates fresh water, food, timber, and stone as finite reachable camp nodes derived from the generated semantic fields. A person performs a bounded source search, reserves units, physically travels to the source, gathers, carries the load back, and deposits it into the shared stockpile.
- Shelter construction commits 60 timber and 30 stone exactly once, then requires attributed person labor. Source remainder, reservations, carried units, stockpile units, committed materials, and consumption remain authoritative integer quantities included in deterministic checksums.
- The HUD exposes stockpile totals, reachable remainder, shelter material/labor progress, action reasons, a resource legend, and a marker toggle. Clicking the physical camp foundation selects its read model; presentation remains unable to mutate camp truth.
- This amendment does not activate processed commodities, rivers and lake hydrology, wildlife populations, road networks, crop production, mature ecosystems, or final environment art.

## Consequences

- Build 02 grows from a single regional test patch into the minimum world-creation foundation while retaining the Founding Camp survival loop.
- World generation, semantic world data, and presentation require separate boundaries so graphical iteration cannot change authoritative results.
- Larger presets require measured generation time, memory, presentation, path/query, save, and simulation budgets. A preset may not ship merely because it can be displayed.
- Build 09 still owns release-quality onboarding, tuning, accessibility, content lock, and world-creation polish.
- Mature environments, ecosystems, disasters, and complete early-modern breadth remain later work.

## Migration

- The existing World Viewer seed becomes the seed input for a provisional default world setup.
- The Build 01 diagnostic field remains valid only as a test fixture or safe fallback; it is not the intended New Game world.
- Build 02 persistence must save canonical world-generation settings, generation version, stable world identity, and semantic changes needed by the active build. It must not serialize presentation meshes as authority.
- Later celestial-body fields extend semantic containers through versioned migrations rather than replacing planetary identities.

## Validation

- Repeating the same generation version, seed, and settings produces the same canonical world checksum regardless of camera, frame rate, preview use, or rendering detail.
- Changing one exposed control produces a bounded, explainable change and cannot create invalid or unreachable founding-site choices.
- Preview geography and the entered founding site refer to the same stable world and region identities.
- Each enabled world-size preset passes declared generation-time, memory, traversal/query, save-size, and simulation budgets.
- 3D presentation can be disabled or changed without altering generation or simulation checkpoints.
- Save/reload preserves generation settings, stable identities, founding location, and subsequent semantic changes.

## Rollback or supersession

If a control, world size, or 3D element fails readability, performance, determinism, or play-value gates, reduce or defer that bounded element while preserving configurable deterministic world creation and semantic geography. Replacing the hierarchical world direction or removing its later multi-planet continuity requires another explicit creator decision that supersedes this ADR.

## Creator disclosure

The creator explicitly approved configurable world sizes at New Game, procedural-generation control, a colony-simulation-style world structure with a colorful living-world presentation, eventual multi-planet continuity, and adding selected 3D elements. Exact numeric presets, control ranges, geometry techniques, and future celestial breadth remain deliberately unopened until playable evidence exists.
