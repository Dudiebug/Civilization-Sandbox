using CivSandbox.People;
using CivSandbox.Presentation;
using CivSandbox.World;
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
                Assert.That(sceneObject.GetComponentsInChildren<TextMesh>(false), Is.Empty,
                    "Names should remain available through selection and the inspector, not float above people.");
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
                Assert.That(litMaterialCount, Is.InRange(1, 12));
                Assert.That(EraMaterialFactory.CachedLitMaterialCount, Is.EqualTo(litMaterialCount));
                Assert.That(unlitMaterialCount, Is.InRange(1, 8));
                Assert.That(EraMaterialFactory.CachedUnlitMaterialCount, Is.EqualTo(unlitMaterialCount));
            }
            finally
            {
                Object.DestroyImmediate(sceneObject);
            }
        }

        [Test]
        public void GeneratedWorldCampUsesPersonScaleTilesOnWalkableTerrain()
        {
            GeneratedWorld world = WorldGenerator.Generate(WorldGenerationSettings.Default);
            Assert.That(world.TryChooseFoundingCell(out GeneratedWorldCell site), Is.True);
            Assert.That(site.IsWater, Is.False);
            Assert.That(world.IsFoundingSite(site), Is.True);
            var previewObject = new GameObject("Generated world preview test");
            var sceneObject = new GameObject("Generated world camp test");

            try
            {
                WorldPreviewView preview = previewObject.AddComponent<WorldPreviewView>();
                preview.Initialize(world);
                Vector3 siteCenter = preview.CellPosition(site);
                WorldSceneView sceneView = sceneObject.AddComponent<WorldSceneView>();
                sceneView.InitializeInGeneratedWorld(
                    new WorldSimulation(170601UL).CreateSnapshot(),
                    preview.PreviewCamera,
                    siteCenter,
                    preview.WorldUnitsPerMeter,
                    preview.SurfaceHeightAtWorldPosition);

                PersonBillboardView[] people = sceneObject.GetComponentsInChildren<PersonBillboardView>(false);
                Assert.That(people.Length, Is.EqualTo(WorldSimulation.PersonCount));
                foreach (PersonBillboardView person in people)
                {
                    Assert.That(Mathf.Abs(person.transform.position.x - siteCenter.x), Is.LessThan(GeneratedWorld.FoundingClearanceCells));
                    Assert.That(Mathf.Abs(person.transform.position.z - siteCenter.z), Is.LessThan(GeneratedWorld.FoundingClearanceCells));
                    Assert.That(person.GetComponentInChildren<SpriteRenderer>().sprite.bounds.size.x, Is.EqualTo(1f).Within(0.001f));
                    Assert.That(
                        person.transform.position.y,
                        Is.EqualTo(preview.SurfaceHeightAtWorldPosition(person.transform.position) + 0.03f).Within(0.001f));
                }
            }
            finally
            {
                Object.DestroyImmediate(sceneObject);
                Object.DestroyImmediate(previewObject);
            }
        }

        [Test]
        public void GeneratedFoundingCampPresentsSharedStockpileAndShelterProgress()
        {
            GeneratedWorld world = WorldGenerator.Generate(WorldGenerationSettings.Default);
            Assert.That(world.TryChooseFoundingCell(out GeneratedWorldCell site), Is.True);
            var simulation = new WorldSimulation(world, site, 5151UL);
            var previewObject = new GameObject("Camp presentation preview test");
            var sceneObject = new GameObject("Camp presentation scene test");

            try
            {
                WorldPreviewView preview = previewObject.AddComponent<WorldPreviewView>();
                preview.Initialize(world);
                WorldSceneView sceneView = sceneObject.AddComponent<WorldSceneView>();
                sceneView.InitializeInGeneratedWorld(
                    simulation.CreateSnapshot(),
                    preview.PreviewCamera,
                    preview.CellPosition(site),
                    preview.WorldUnitsPerMeter,
                    preview.SurfaceHeightAtWorldPosition);

                Assert.That(sceneView.CampView, Is.Not.Null);
                Assert.That(sceneView.CampView.transform.Find("Shelter foundation"), Is.Not.Null);
                Assert.That(sceneView.CampView.transform.Find("Water stockpile"), Is.Not.Null);

                for (int tick = 0; tick < 30000 && !simulation.CreateSnapshot().Camp.Shelter.IsComplete; tick++)
                {
                    simulation.AdvanceFixedWallTick(CivSandbox.Simulation.SimulationSpeed.Normal);
                }

                WorldSnapshot complete = simulation.CreateSnapshot();
                sceneView.Apply(complete, true);
                Transform roof = sceneView.CampView.transform.Find("Completed shelter roof");
                Assert.That(complete.Camp.Shelter.IsComplete, Is.True);
                Assert.That(roof, Is.Not.Null);
                Assert.That(roof.gameObject.activeSelf, Is.True);
            }
            finally
            {
                Object.DestroyImmediate(sceneObject);
                Object.DestroyImmediate(previewObject);
            }
        }
    }
}
