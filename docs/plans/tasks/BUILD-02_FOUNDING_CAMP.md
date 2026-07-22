# Prompt - Build 02: Founding Camp

Create the first real gameplay loop: the player creates a deterministic world, chooses a founding site, and a small group attempts to survive and establish basic shelter. Preserve the Blueprint identity and evolve the World Viewer rather than discarding its simulation boundary.

Player outcome: the title screen leads through New Game into bounded world-size and procedural-generation controls, a generated preview, and founding-site selection. The entered world has colorful top-down readability plus selected 3D terrain and environmental forms. People then search for reachable food and water, gather and carry resources, contribute to a shared stockpile, consume necessities, build basic shelter, and either stabilize or fail. Clicking a person or the camp explains the immediate cause of the current action or failure.

Start by inspecting the current simulation and presentation. The already-started survival-needs and inspection slice remains first. After that slice passes its gate, implement the approved minimum world-creation slice before deeper gathering: title screen, New Game setup, deterministic seed/settings, named size presets, generated preview, semantic geography, initial hybrid 2.5D presentation, founding-site selection, and transition into play. Continue with reachable resources, gathering, hauling, stockpile/reservations, shelter construction, explanations, and minimum save/reload. Implement and play-test one slice at a time.

Follow [ADR-004](../../decisions/ADR-004_CONFIGURABLE_HIERARCHICAL_WORLD_CREATION.md). Exact world dimensions and control ranges are provisional tuning values, not hidden final product decisions. Stable identity may reserve generated-world, celestial-body, region, and site levels, but Build 02 implements only the planetary world and founding-site levels needed for play.

Use deterministic choices, explicit resource units, real positions/travel, and conservation. A camera cannot influence decisions. A resource cannot be consumed or delivered twice. Save only semantic state needed by this build and include a safe previous-good recovery path.

Acceptance: identical generation version, seed, and settings reproduce identical semantic geography independent of camera and rendering detail; preview and play reference the same stable world/site; enabled size presets meet declared provisional budgets; at least one seed reaches a viable camp and one fails for an understandable material reason; save/reload during hunger or construction preserves subsequent behavior; the player can follow the causal chain.

Do not add deep families, broad economy, governments, warfare, Story Director profiles, mature ecosystems, final terrain/content polish, large catalogs, orbital systems, or multi-planet gameplay. Do not let 3D presentation or camera state become simulation authority.
