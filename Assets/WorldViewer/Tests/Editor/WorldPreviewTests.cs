using System.Diagnostics;
using CivSandbox.People;
using CivSandbox.Presentation;
using CivSandbox.World;
using NUnit.Framework;
using UnityEngine;

namespace CivSandbox.WorldViewer.Tests
{
    public sealed class WorldPreviewTests
    {
        [Test]
        public void PreviewBuildsOneSelectableTerrainAndBoundedLandmarks()
        {
            GeneratedWorld world = WorldGenerator.Generate(WorldGenerationSettings.Default);
            var previewObject = new GameObject("World preview test");
            try
            {
                WorldPreviewView preview = previewObject.AddComponent<WorldPreviewView>();
                preview.Initialize(world);

                Assert.That(preview.PreviewCamera, Is.Not.Null);
                Assert.That(preview.PreviewCamera.orthographic, Is.True);
                Vector3 forward = preview.PreviewCamera.transform.forward;
                float horizontalLook = new Vector2(forward.x, forward.z).magnitude;
                Assert.That(horizontalLook, Is.GreaterThan(0.1f), "The near-top-down view must retain enough angle to reveal elevation.");
                Assert.That(forward.y, Is.LessThan(-0.9f), "The default world view must read as top-down.");
                Assert.That(Mathf.Abs(forward.x), Is.LessThan(0.02f), "Top-down starts north-up instead of rotating the square into a diamond.");
                Assert.That(forward.z, Is.GreaterThan(0.1f), "The near-top-down view must retain enough pitch to reveal elevation.");
                Assert.That(preview.PreviewCamera.GetComponent<WorldCameraController>().ViewMode, Is.EqualTo(WorldCameraView.TopDown));
                Assert.That(previewObject.GetComponentsInChildren<MeshCollider>(false), Is.Empty);
                Assert.That(preview.TerrainChunkCount, Is.EqualTo(36));
                Assert.That(preview.LandmarkCount, Is.InRange(100, 32768));
                Assert.That(preview.ResourceMarkersVisible, Is.True);
                preview.ToggleResourceMarkers();
                Assert.That(preview.ResourceMarkersVisible, Is.False);
                preview.ToggleResourceMarkers();
                Assert.That(preview.ResourceMarkersVisible, Is.True);
                Assert.That(previewObject.GetComponentsInChildren<PersonBillboardView>(false), Is.Empty);
            }
            finally
            {
                Object.DestroyImmediate(previewObject);
            }
        }

        [Test]
        public void PreviewCameraAndSelectionCannotChangeSemanticWorldChecksum()
        {
            GeneratedWorld world = WorldGenerator.Generate(WorldGenerationSettings.Default);
            Assert.That(world.TryFindFirstFoundableCell(out GeneratedWorldCell site), Is.True);
            ulong before = world.Checksum;
            var previewObject = new GameObject("World preview independence test");
            try
            {
                WorldPreviewView preview = previewObject.AddComponent<WorldPreviewView>();
                preview.Initialize(world);
                preview.PreviewCamera.transform.position += new Vector3(12f, -4f, 7f);
                preview.PreviewCamera.orthographicSize *= 0.7f;
                preview.SetSelected(site.Id);
                WorldCameraController controller = preview.PreviewCamera.GetComponent<WorldCameraController>();
                controller.ToggleView();

                Assert.That(world.Checksum, Is.EqualTo(before));
                Assert.That(controller.ViewMode, Is.EqualTo(WorldCameraView.Angled));
            }
            finally
            {
                Object.DestroyImmediate(previewObject);
            }
        }

        [Test]
        public void MaxPreviewBuildsBoundedChunksInsidePrototypeBudget()
        {
            WorldGenerationSettings baseline = WorldGenerationSettings.Default;
            var maxSettings = new WorldGenerationSettings(
                baseline.Seed.Value,
                WorldSizePreset.Large,
                baseline.LandPercent,
                baseline.TemperaturePercent,
                baseline.RainfallPercent,
                baseline.MountainPercent,
                baseline.ForestPercent,
                baseline.ResourcePercent);
            GeneratedWorld world = WorldGenerator.Generate(maxSettings);
            var previewObject = new GameObject("Max world preview budget test");

            try
            {
                var stopwatch = Stopwatch.StartNew();
                WorldPreviewView preview = previewObject.AddComponent<WorldPreviewView>();
                preview.Initialize(world);
                stopwatch.Stop();

                Assert.That(preview.TerrainChunkCount, Is.EqualTo(256));
                Assert.That(previewObject.GetComponentsInChildren<MeshCollider>(false), Is.Empty);
                Assert.That(stopwatch.ElapsedMilliseconds, Is.LessThan(15000),
                    "Max 1024x1024 chunked terrain exceeded the provisional fifteen-second editor budget.");
            }
            finally
            {
                Object.DestroyImmediate(previewObject);
            }
        }
    }
}
