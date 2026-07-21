using UnityEngine;

namespace CivSandbox.Presentation
{
    internal static class EraMaterialFactory
    {
        public static Material CreateLit(Color color)
        {
            Shader shader = Shader.Find("Universal Render Pipeline/Lit") ?? Shader.Find("Standard");
            if (shader == null)
            {
                return null;
            }

            var material = new Material(shader)
            {
                color = color,
                name = "Build 01 Runtime Lit Material"
            };
            return material;
        }

        public static Material CreateUnlit(Color color)
        {
            Shader shader = Shader.Find("Universal Render Pipeline/Unlit") ?? Shader.Find("Unlit/Color") ?? Shader.Find("Sprites/Default");
            if (shader == null)
            {
                return null;
            }

            var material = new Material(shader)
            {
                color = color,
                name = "Build 01 Runtime Unlit Material"
            };
            return material;
        }
    }
}
