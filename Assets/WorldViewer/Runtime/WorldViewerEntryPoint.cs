using System;
using System.Collections;
using CivSandbox.People;
using CivSandbox.Presentation;
using CivSandbox.Simulation;
using CivSandbox.UI;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace CivSandbox.WorldViewer
{
    public static class WorldViewerEntryPoint
    {
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
        private static void CreateWorldViewer()
        {
            if (SceneManager.GetActiveScene().name != "WorldViewer" || UnityEngine.Object.FindFirstObjectByType<WorldViewerRoot>() != null)
            {
                return;
            }

            var rootObject = new GameObject("World Viewer");
            rootObject.AddComponent<WorldViewerRoot>();
        }
    }

    public sealed class WorldViewerRoot : MonoBehaviour, IWorldViewerSession
    {
        private const ulong DefaultSeed = 170601UL;
        private WorldViewerSession session;
        private WorldSceneView sceneView;
        private bool overloadWarningActive;
        private double nextOverloadWarningWallTime;

        public ulong Seed => session.Seed;

        public SimulationSpeed Speed => session.Speed;

        public WorldSnapshot Snapshot => session.Snapshot;

        public StableEntityId? SelectedPersonId => session.SelectedPersonId;

        public bool IsClockOverloaded => session.IsClockOverloaded;

        public TimeSpan TotalDroppedWallTime => session.TotalDroppedWallTime;

        private void Awake()
        {
            session = new WorldViewerSession(DefaultSeed);

            var sceneObject = new GameObject("Read-only world presentation");
            sceneObject.transform.SetParent(transform, false);
            sceneView = sceneObject.AddComponent<WorldSceneView>();
            sceneView.Initialize(Snapshot);
            sceneView.WorldCamera.gameObject.AddComponent<WorldSelectionController>().Configure(sceneView.WorldCamera, SelectPerson);

            var hudObject = new GameObject("World Viewer HUD");
            hudObject.transform.SetParent(transform, false);
            hudObject.AddComponent<WorldViewerHud>().Initialize(this);
        }

        private IEnumerator Start()
        {
            if (!Application.isBatchMode)
            {
                yield break;
            }

            yield return null;
            Debug.Log($"CIV-BUILD01-SMOKE-000: World Viewer reached its first frame with {Snapshot.Count} named people.");
            Application.Quit(0);
        }

        private void Update()
        {
            FixedStepPumpReport pumpReport = session.AdvanceWallTime(TimeSpan.FromSeconds(Time.unscaledDeltaTime));

            double wallTime = Time.unscaledTimeAsDouble;
            if (pumpReport.IsOverloaded && (!overloadWarningActive || wallTime >= nextOverloadWarningWallTime))
            {
                string code = pumpReport.TotalDroppedWallTime > TimeSpan.Zero
                    ? "CIV-BUILD01-CLOCK-001"
                    : "CIV-BUILD01-CLOCK-000";
                Debug.LogWarning(
                    $"{code}: Fixed-step overload has {pumpReport.PendingWholeSteps} whole steps in the bounded backlog; " +
                    $"cumulative dropped wall time is {pumpReport.TotalDroppedWallTime.TotalMilliseconds:0.###} ms.");
                nextOverloadWarningWallTime = wallTime + 5d;
            }

            overloadWarningActive = pumpReport.IsOverloaded;

            if (pumpReport.StepsToRun > 0 && Speed != SimulationSpeed.Paused)
            {
                sceneView.Apply(Snapshot);
            }
        }

        public void Reset(ulong seed)
        {
            session.Reset(seed);
            overloadWarningActive = false;
            nextOverloadWarningWallTime = 0d;
            sceneView.Apply(Snapshot, true);
            sceneView.SetSelected(null);
        }

        public void SetSpeed(SimulationSpeed speed)
        {
            TimeSpan previousDroppedWallTime = session.TotalDroppedWallTime;
            session.SetSpeed(speed);
            TimeSpan discardedWallTime = session.TotalDroppedWallTime - previousDroppedWallTime;
            if (discardedWallTime > TimeSpan.Zero)
            {
                Debug.LogWarning(
                    $"CIV-BUILD01-CLOCK-002: Speed changed to {speed.Label()}; discarded " +
                    $"{discardedWallTime.TotalMilliseconds:0.###} ms of retained wall time.");
            }
        }

        public void SelectPerson(StableEntityId? personId)
        {
            session.SelectPerson(personId);

            if (sceneView != null)
            {
                sceneView.SetSelected(session.SelectedPersonId);
            }
        }
    }
}
