using UnityEngine;

namespace CivSandbox.Presentation
{
    internal static class EarlyModernSpriteFactory
    {
        private static readonly Color32[] Cloth =
        {
            new Color32(111, 57, 43, 255),
            new Color32(43, 83, 78, 255),
            new Color32(108, 86, 47, 255),
            new Color32(72, 64, 91, 255),
            new Color32(126, 74, 42, 255),
            new Color32(73, 91, 47, 255)
        };

        private static readonly Color32[] Skin =
        {
            new Color32(238, 194, 145, 255),
            new Color32(205, 151, 104, 255),
            new Color32(158, 105, 74, 255),
            new Color32(108, 72, 57, 255)
        };

        public static Sprite Create(int appearanceVariant)
        {
            const int width = 12;
            const int height = 20;
            var texture = new Texture2D(width, height, TextureFormat.RGBA32, false)
            {
                name = $"Early-modern person {appearanceVariant}",
                filterMode = FilterMode.Point,
                wrapMode = TextureWrapMode.Clamp
            };

            var pixels = new Color32[width * height];
            Color32 cloth = Cloth[appearanceVariant % Cloth.Length];
            Color32 skin = Skin[(appearanceVariant / 2) % Skin.Length];
            Color32 dark = new Color32(40, 31, 27, 255);
            Color32 leather = new Color32(74, 48, 31, 255);

            Fill(pixels, width, 4, 1, 2, 3, leather);
            Fill(pixels, width, 7, 1, 2, 3, leather);
            Fill(pixels, width, 3, 4, 6, 7, cloth);
            Fill(pixels, width, 2, 7, 8, 5, cloth);
            Fill(pixels, width, 1, 10, 2, 4, cloth);
            Fill(pixels, width, 9, 10, 2, 4, cloth);
            Fill(pixels, width, 4, 12, 4, 4, skin);
            Fill(pixels, width, 3, 15, 6, 2, dark);
            if ((appearanceVariant & 1) == 0)
            {
                Fill(pixels, width, 2, 16, 8, 2, cloth);
                Fill(pixels, width, 4, 18, 4, 1, cloth);
            }
            else
            {
                Fill(pixels, width, 3, 16, 6, 2, dark);
            }

            pixels[5 + 14 * width] = dark;
            pixels[7 + 14 * width] = dark;
            texture.SetPixels32(pixels);
            texture.Apply(false, true);
            return Sprite.Create(texture, new Rect(0, 0, width, height), new Vector2(0.5f, 0.05f), 12f, 0, SpriteMeshType.FullRect);
        }

        private static void Fill(Color32[] pixels, int width, int x, int y, int blockWidth, int blockHeight, Color32 color)
        {
            for (int row = y; row < y + blockHeight; row++)
            {
                for (int column = x; column < x + blockWidth; column++)
                {
                    pixels[column + row * width] = color;
                }
            }
        }
    }
}
