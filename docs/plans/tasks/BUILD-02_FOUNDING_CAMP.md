# Prompt - Build 02: Founding Camp

Create the first real gameplay loop: a small group attempts to survive and establish basic shelter. Preserve the Blueprint identity and reuse the World Viewer rather than replacing it.

Player outcome: people search for reachable food and water, gather and carry resources, contribute to a shared stockpile, consume necessities, build basic shelter, and either stabilize or fail. Clicking a person or the camp explains the immediate cause of the current action or failure.

Start by inspecting the current simulation and presentation. Propose 4-8 playable slices such as needs, reachable resources, gathering, hauling, stockpile/reservations, shelter construction, explanations, and minimum save/reload. Implement and play-test one slice at a time.

Use deterministic choices, explicit resource units, real positions/travel, and conservation. A camera cannot influence decisions. A resource cannot be consumed or delivered twice. Save only semantic state needed by this build and include a safe previous-good recovery path.

Acceptance: at least one seed reaches a viable camp and one fails for an understandable material reason; save/reload during hunger or construction preserves subsequent behavior; the player can follow the causal chain.

Do not add deep families, broad economy, governments, warfare, Story Director profiles, final terrain, or large content catalogs.
