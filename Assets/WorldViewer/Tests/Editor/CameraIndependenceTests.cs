using System;
using CivSandbox.People;
using CivSandbox.Presentation;
using CivSandbox.Simulation;
using CivSandbox.World;
using NUnit.Framework;
using UnityEngine;

namespace CivSandbox.WorldViewer.Tests
{
    public sealed class CameraIndependenceTests
    {
        [Test]
        public void MovingZoomingAndHidingCameraCannotChangeAuthoritativeCheckpoint()
        {
            var observed = new WorldViewerSession(170601UL);
            var headless = new WorldViewerSession(170601UL);
            var cameraObject = new GameObject("Camera independence probe");

            try
            {
                Camera camera = cameraObject.AddComponent<Camera>();
                camera.orthographic = true;
                camera.transform.rotation = Quaternion.Euler(42f, -45f, 0f);
                var controller = cameraObject.AddComponent<WorldCameraController>();
                controller.Configure(new WorldBounds(-22000, 22000, -22000, 22000));

                for (int tick = 0; tick < 240; tick++)
                {
                    observed.AdvanceWallTime(TimeSpan.FromMilliseconds(50));
                    controller.SetViewForVerification(
                        new Vector3((tick % 17) - 8f, 34f + tick % 3, (tick % 23) - 11f),
                        7f + tick % 25);
                    cameraObject.SetActive((tick & 3) != 0);

                    headless.AdvanceWallTime(TimeSpan.FromMilliseconds(50));
                }

                Assert.That(observed.Snapshot.Time, Is.EqualTo(headless.Snapshot.Time));
                Assert.That(observed.ComputeAuthoritativeChecksum(), Is.EqualTo(headless.ComputeAuthoritativeChecksum()));
            }
            finally
            {
                UnityEngine.Object.DestroyImmediate(cameraObject);
            }
        }

        [Test]
        public void CameraControllerHasNoAuthoritativeSimulationField()
        {
            var fields = typeof(WorldCameraController).GetFields(
                System.Reflection.BindingFlags.Instance |
                System.Reflection.BindingFlags.Public |
                System.Reflection.BindingFlags.NonPublic);

            foreach (var field in fields)
            {
                Assert.That(typeof(WorldSimulation).IsAssignableFrom(field.FieldType), Is.False, field.Name);
            }
        }
    }
}
