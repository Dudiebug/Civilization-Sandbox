using CivSandbox.People;
using CivSandbox.Presentation;
using NUnit.Framework;
using UnityEngine;

namespace CivSandbox.WorldViewer.Tests
{
    public sealed class WorldSceneResetTests
    {
        [Test]
        public void DifferentSeedResetReplacesPresentersWithoutGhostPeople()
        {
            var first = new WorldSimulation(170601UL);
            var second = new WorldSimulation(170602UL);
            var sceneObject = new GameObject("Reset presentation test");

            try
            {
                WorldSceneView sceneView = sceneObject.AddComponent<WorldSceneView>();
                sceneView.Initialize(first.CreateSnapshot());
                Assert.That(sceneView.PersonViewCount, Is.EqualTo(WorldSimulation.PersonCount));

                sceneView.Apply(second.CreateSnapshot(), true);

                Assert.That(sceneView.PersonViewCount, Is.EqualTo(WorldSimulation.PersonCount));
                Assert.That(sceneObject.GetComponentsInChildren<PersonBillboardView>(false).Length, Is.EqualTo(WorldSimulation.PersonCount));
            }
            finally
            {
                Object.DestroyImmediate(sceneObject);
            }
        }

        [Test]
        public void RepeatedSeedResetsReuseABoundedSetOfVisualAssets()
        {
            var sceneObject = new GameObject("Reset asset test");

            try
            {
                WorldSceneView sceneView = sceneObject.AddComponent<WorldSceneView>();
                sceneView.Initialize(new WorldSimulation(170601UL).CreateSnapshot());

                for (ulong seed = 170602UL; seed < 170702UL; seed++)
                {
                    sceneView.Apply(new WorldSimulation(seed).CreateSnapshot(), true);
                    Assert.That(sceneView.PersonViewCount, Is.EqualTo(WorldSimulation.PersonCount));
                }

                int spriteCount = EarlyModernSpriteFactory.CachedSpriteCount;
                int litMaterialCount = EraMaterialFactory.CachedLitMaterialCount;
                int unlitMaterialCount = EraMaterialFactory.CachedUnlitMaterialCount;

                for (ulong seed = 170702UL; seed < 170802UL; seed++)
                {
                    sceneView.Apply(new WorldSimulation(seed).CreateSnapshot(), true);
                }

                Assert.That(spriteCount, Is.InRange(1, 12));
                Assert.That(EarlyModernSpriteFactory.CachedSpriteCount, Is.EqualTo(spriteCount));
                Assert.That(litMaterialCount, Is.InRange(1, 2));
                Assert.That(EraMaterialFactory.CachedLitMaterialCount, Is.EqualTo(litMaterialCount));
                Assert.That(unlitMaterialCount, Is.InRange(1, 2));
                Assert.That(EraMaterialFactory.CachedUnlitMaterialCount, Is.EqualTo(unlitMaterialCount));
            }
            finally
            {
                Object.DestroyImmediate(sceneObject);
            }
        }
    }
}
