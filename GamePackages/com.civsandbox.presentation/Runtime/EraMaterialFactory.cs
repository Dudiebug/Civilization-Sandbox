using System.Collections.Generic;
using UnityEngine;

namespace CivSandbox.Presentation
{
    internal static class EraMaterialFactory
    {
        private static readonly Dictionary<Color32, Material> LitMaterials = new Dictionary<Color32, Material>();
        private static readonly Dictionary<Color32, Material> UnlitMaterials = new Dictionary<Color32, Material>();
        private static Material terrainMaterial;

        internal static int CachedLitMaterialCount => LitMaterials.Count;

        internal static int CachedUnlitMaterialCount => UnlitMaterials.Count;

        public static Material CreateLit(Color color)
        {
            // The default sprite shader is part of the pinned player's always-included set and works
            // in both the built-in and URP paths. Build 01 uses it intentionally for prototype solids.
            return GetOrCreate(LitMaterials, color, "Build 01 Runtime Lit Material");
        }

        public static Material CreateUnlit(Color color)
        {
            return GetOrCreate(UnlitMaterials, color, "Build 01 Runtime Unlit Material");
        }

        public static Material CreateTerrain()
        {
            if (terrainMaterial != null)
            {
                return terrainMaterial;
            }

            Shader shader = Shader.Find("CivSandbox/Vertex Color Terrain") ?? Shader.Find("Sprites/Default");
            terrainMaterial = new Material(shader)
            {
                name = "Build 02 Vertex Color Terrain Material"
            };
            return terrainMaterial;
        }

        private static Material GetOrCreate(Dictionary<Color32, Material> cache, Color color, string materialName)
        {
            Color32 key = color;
            if (cache.TryGetValue(key, out Material cached) && cached != null)
            {
                return cached;
            }

            Shader shader = Shader.Find("Sprites/Default") ?? Shader.Find("Unlit/Color") ?? Shader.Find("Standard");
            if (shader == null)
            {
                return null;
            }

            var material = new Material(shader)
            {
                color = color,
                name = materialName
            };
            cache[key] = material;
            return material;
        }
    }
}
