using System;
using System.Collections.Generic;
using CivSandbox.People;
using UnityEngine;

namespace CivSandbox.Presentation
{
    internal enum PersonVisualLayer { Body, Lower, Upper, Outer, Footwear, Headwear }

    internal static class EarlyModernSpriteFactory
    {
        internal const int TextureWidth = 32;
        internal const int TextureHeight = 48;
        private const float PixelsPerUnit = 22f;
        private static readonly Dictionary<string, Sprite> Sprites = new Dictionary<string, Sprite>();

        private static readonly Color32[] NaturalDyes =
        {
            new Color32(207, 190, 145, 255), // undyed linen
            new Color32(128, 63, 47, 255),   // madder rust
            new Color32(69, 79, 111, 255),   // dull indigo
            new Color32(153, 112, 47, 255),  // weld ochre
            new Color32(94, 73, 63, 255),    // walnut brown
            new Color32(104, 57, 70, 255)    // muted burgundy
        };

        private static readonly Color32[] Skin =
        {
            new Color32(238, 194, 145, 255), new Color32(205, 151, 104, 255),
            new Color32(158, 105, 74, 255), new Color32(108, 72, 57, 255)
        };

        internal static Sprite Create(PersonVisualLayer layer, int appearanceVariant, ClothingAppearance clothing)
        {
            int item = ItemFor(layer, clothing);
            int color = ColorFor(layer, clothing);
            int visualVariant = layer == PersonVisualLayer.Body ? Math.Abs(appearanceVariant % 12) : 0;
            string key = $"{layer}:{visualVariant}:{item}:{color}";
            if (Sprites.TryGetValue(key, out Sprite cached)) return cached;

            var texture = new Texture2D(TextureWidth, TextureHeight, TextureFormat.RGBA32, false)
            {
                name = $"32x48 {layer} {item}", filterMode = FilterMode.Point, wrapMode = TextureWrapMode.Clamp
            };
            var pixels = new Color32[TextureWidth * TextureHeight];
            DrawLayer(pixels, layer, appearanceVariant, item, color);
            texture.SetPixels32(pixels);
            texture.Apply(false, true);
            Sprite sprite = Sprite.Create(texture, new Rect(0, 0, TextureWidth, TextureHeight), new Vector2(0.5f, 0.04f), PixelsPerUnit, 0, SpriteMeshType.FullRect);
            sprite.name = texture.name;
            Sprites.Add(key, sprite);
            return sprite;
        }

        internal static int CachedSpriteCount => Sprites.Count;

        private static int ItemFor(PersonVisualLayer layer, ClothingAppearance c)
        {
            switch (layer)
            {
                case PersonVisualLayer.Lower: return (int)c.Lower;
                case PersonVisualLayer.Upper: return (int)c.Upper;
                case PersonVisualLayer.Outer: return (int)c.Outer;
                case PersonVisualLayer.Footwear: return (int)c.Footwear;
                case PersonVisualLayer.Headwear: return (int)c.Headwear;
                default: return 0;
            }
        }

        private static int ColorFor(PersonVisualLayer layer, ClothingAppearance c)
        {
            switch (layer)
            {
                case PersonVisualLayer.Body: return 0;
                case PersonVisualLayer.Lower: return c.LowerColor % NaturalDyes.Length;
                case PersonVisualLayer.Outer: return c.Outer == OuterGarment.None ? 0 : c.OuterColor % NaturalDyes.Length;
                case PersonVisualLayer.Footwear: return 4;
                case PersonVisualLayer.Headwear: return c.Headwear == Headwear.None ? 0 : c.UpperColor % NaturalDyes.Length;
                default: return c.UpperColor % NaturalDyes.Length;
            }
        }

        private static void DrawLayer(Color32[] p, PersonVisualLayer layer, int variant, int item, int colorIndex)
        {
            Color32 cloth = NaturalDyes[colorIndex];
            Color32 shade = Shade(cloth, 0.68f);
            Color32 seam = Shade(cloth, 0.45f);
            switch (layer)
            {
                case PersonVisualLayer.Body:
                    Color32 skin = Skin[Math.Abs(variant / 2) % Skin.Length];
                    Color32 hair = new Color32((byte)(45 + (variant % 4) * 17), (byte)(31 + (variant % 3) * 12), 24, 255);
                    Fill(p, 11, 27, 10, 12, skin); Fill(p, 12, 36, 8, 8, skin);
                    Fill(p, 11, 40, 10, 5, hair); Fill(p, 10, 37, 2, 5, hair); Fill(p, 20, 37, 2, 5, hair);
                    Fill(p, 8, 25, 3, 13, skin); Fill(p, 21, 25, 3, 13, skin);
                    Fill(p, 12, 5, 3, 21, skin); Fill(p, 17, 5, 3, 21, skin);
                    Pixel(p, 14, 38, hair); Pixel(p, 18, 38, hair);
                    break;
                case PersonVisualLayer.Lower:
                    if (item == (int)LowerGarment.KneeBreeches)
                    {
                        Fill(p, 10, 12, 5, 16, cloth); Fill(p, 17, 12, 5, 16, cloth); Fill(p, 9, 25, 14, 5, shade);
                        Fill(p, 10, 12, 5, 2, seam); Fill(p, 17, 12, 5, 2, seam);
                    }
                    else
                    {
                        int flare = item == (int)LowerGarment.Petticoat ? 8 : 7;
                        Fill(p, 12, 24, 8, 5, cloth); Fill(p, 10, 17, 12, 8, cloth); Fill(p, flare, 7, 32 - flare * 2, 10, cloth);
                        Fill(p, flare, 7, 32 - flare * 2, 2, shade);
                        if (item == (int)LowerGarment.TiedApron) { Fill(p, 11, 15, 10, 12, NaturalDyes[0]); Fill(p, 10, 25, 12, 2, seam); }
                    }
                    break;
                case PersonVisualLayer.Upper:
                    Fill(p, 10, 25, 12, 12, cloth); Fill(p, 7, 26, 4, 10, cloth); Fill(p, 21, 26, 4, 10, cloth);
                    Fill(p, 10, 25, 12, 2, shade);
                    if (item == (int)UpperGarment.WoolWaistcoat || item == (int)UpperGarment.LacedBodice)
                    {
                        Fill(p, 12, 27, 8, 10, shade); for (int y = 28; y < 36; y += 3) { Pixel(p, 15, y, NaturalDyes[0]); Pixel(p, 17, y, NaturalDyes[0]); }
                    }
                    if (item == (int)UpperGarment.WorkJacket || item == (int)UpperGarment.LongCoat)
                    {
                        Fill(p, 9, 25, 14, 13, cloth); Fill(p, 15, 25, 2, 13, seam); Pixel(p, 18, 31, NaturalDyes[3]);
                    }
                    if (item == (int)UpperGarment.LongCoat) { Fill(p, 8, 14, 7, 13, cloth); Fill(p, 17, 14, 7, 13, cloth); }
                    Fill(p, 13, 36, 6, 2, NaturalDyes[0]);
                    break;
                case PersonVisualLayer.Outer:
                    if (item == (int)OuterGarment.HoodedCloak)
                    {
                        Fill(p, 9, 30, 14, 8, cloth); Fill(p, 7, 17, 18, 14, cloth); Fill(p, 5, 9, 22, 9, cloth);
                        Fill(p, 10, 42, 12, 3, cloth); Fill(p, 9, 37, 3, 6, cloth); Fill(p, 20, 37, 3, 6, cloth);
                        Fill(p, 12, 36, 8, 2, shade); Fill(p, 5, 9, 22, 2, seam);
                    }
                    else if (item == (int)OuterGarment.ShoulderShawl)
                    {
                        Fill(p, 7, 30, 18, 7, cloth); Fill(p, 9, 25, 6, 6, cloth); Fill(p, 17, 25, 6, 6, cloth); Fill(p, 7, 30, 18, 2, shade);
                    }
                    break;
                case PersonVisualLayer.Footwear:
                    Color32 leather = NaturalDyes[4]; Fill(p, 9, 3, 7, item == 1 ? 9 : 5, leather); Fill(p, 17, 3, 7, item == 1 ? 9 : 5, leather);
                    Fill(p, 8, 3, 8, 2, seam); Fill(p, 17, 3, 8, 2, seam);
                    if (item == 0) { Pixel(p, 13, 6, NaturalDyes[3]); Pixel(p, 20, 6, NaturalDyes[3]); }
                    break;
                case PersonVisualLayer.Headwear:
                    if (item == 0) break;
                    if (item == (int)Headwear.LinenCap) { Fill(p, 11, 42, 10, 4, NaturalDyes[0]); Fill(p, 10, 42, 12, 2, shade); }
                    if (item == (int)Headwear.Bonnet) { Fill(p, 9, 41, 14, 5, cloth); Fill(p, 8, 41, 16, 2, shade); }
                    if (item == (int)Headwear.BroadFeltHat) { Fill(p, 5, 42, 22, 3, cloth); Fill(p, 11, 44, 10, 3, shade); }
                    if (item == (int)Headwear.CockedHat) { Fill(p, 7, 42, 18, 3, cloth); Fill(p, 10, 44, 12, 3, shade); Pixel(p, 8, 45, cloth); Pixel(p, 23, 45, cloth); }
                    break;
            }
        }

        private static Color32 Shade(Color32 c, float amount) => new Color32((byte)(c.r * amount), (byte)(c.g * amount), (byte)(c.b * amount), c.a);
        private static void Pixel(Color32[] p, int x, int y, Color32 c) { p[x + y * TextureWidth] = c; }
        private static void Fill(Color32[] p, int x, int y, int width, int height, Color32 c)
        {
            for (int row = y; row < y + height; row++) for (int column = x; column < x + width; column++) Pixel(p, column, row, c);
        }
    }
}
