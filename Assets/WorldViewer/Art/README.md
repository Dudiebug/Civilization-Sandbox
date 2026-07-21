# World Viewer pixel-art standard

- World tiles use a 32 x 32 pixel grid.
- People use aligned 32 x 48 pixel layers for body, lower garment, upper garment, outer garment, footwear, and headwear.
- Runtime textures are RGBA32, point-filtered, clamped, uncompressed, and created without mipmaps.
- Clothing uses plausible early-modern forms and restrained natural-dye colors: undyed linen, madder rust, dull indigo, weld ochre, walnut brown, and muted burgundy.
- Names belong in the read-only inspector, not above people in the world.

`Concepts/early-modern-wardrobe-concept-v1.png` is the approved direction sheet for the initial 18 garment pieces. The runtime sprites are exact grid-aligned pixel interpretations, not direct crops of the concept sheet.
