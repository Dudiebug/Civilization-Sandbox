# BUILD-01 Clothing Revision

**State:** IMPLEMENTED — CREATOR RE-REVIEW PENDING

## Creator direction

The initial placeholder people and overhead names were rejected. The approved revision direction is a modular early-modern clothing system with a consistent pixel-art standard: 32 x 32 world tiles, 32 x 48 people, and RGBA32 point-filtered textures.

## Implemented

- Six aligned person layers: body/hair, lower garment, upper garment, outer garment, footwear, and headwear.
- Eighteen initial garment definitions: linen shirt, linen shift, wool waistcoat, laced bodice, work jacket, long coat, knee breeches, petticoat, work skirt, tied apron, hooded cloak, shoulder shawl, linen cap, bonnet, broad felt hat, cocked hat, buckle shoes, and ankle boots.
- Six restrained natural-dye colors and twelve body/hair variants.
- Deterministic outfit assignment derived from world seed and stable person ID; clothing is included in authoritative checksums and detached snapshots.
- Overhead names removed. Names and outfit pieces remain available through read-only omniscient inspection.
- Visual-only Build 01 scope: no inventory ownership, equipping UI, warmth, durability, status, production, or clothing economy.

## Verification

- Clean implementation commit: `a94da6c47051a48b5b01a778b9809d9f27de2801`.
- Automated verifier: `Build/Verify-WorldViewer.ps1` — 8 people tests, 19 presentation tests, Windows player build, and first-frame smoke.
- Added coverage proves deterministic clothing, valid ranges and diversity, six 32 x 48 RGBA32 point-filtered layers, no child `TextMesh`, and a bounded shared sprite cache.
- Runtime-rendered visual sample: [clothing-preview.png](clothing-preview.png).
- Art-direction source: [early-modern-wardrobe-concept-v1.png](../../../Assets/WorldViewer/Art/Concepts/early-modern-wardrobe-concept-v1.png).

Creator acceptance remains pending and is not marked on the creator's behalf.
