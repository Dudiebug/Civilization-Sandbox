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
            if (SceneManager.GetActiveScene().name != "WorldViewer" || Object.FindFirstObjectByType<WorldViewerRoot>() != null)
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
        private const double FixedWallTickSeconds = 1.0 / WorldSimulation.FixedWallTicksPerSecond;
        private WorldSimulation simulation;
        private WorldSceneView sceneView;
        private readonly WorldSelectionState selection = new WorldSelectionState();
        private double wallAccumulator;

        public ulong Seed => simulation.Seed.Value;

        public SimulationSpeed Speed { get; private set; } = SimulationSpeed.Normal;

        public WorldSnapshot Snapshot { get; private set; }

        public StableEntityId? SelectedPersonId => selection.SelectedPersonId;

        private void Awake()
        {
            simulation = new WorldSimulation(DefaultSeed);
            Snapshot = simulation.CreateSnapshot();

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
            wallAccumulator += Time.unscaledDeltaTime;
            int processedTicks = 0;
            while (wallAccumulator >= FixedWallTickSeconds && processedTicks < 8)
            {
                simulation.AdvanceFixedWallTick(Speed);
                wallAccumulator -= FixedWallTickSeconds;
                processedTicks++;
            }

            if (processedTicks > 0 && Speed != SimulationSpeed.Paused)
            {
                Snapshot = simulation.CreateSnapshot();
                sceneView.Apply(Snapshot);
            }
        }

        public void Reset(ulong seed)
        {
            simulation.Reset(seed);
            Speed = SimulationSpeed.Normal;
            wallAccumulator = 0d;
            SelectPerson(null);
            Snapshot = simulation.CreateSnapshot();
            sceneView.Apply(Snapshot, true);
        }

        public void SetSpeed(SimulationSpeed speed)
        {
            Speed = speed;
        }

        public void SelectPerson(StableEntityId? personId)
        {
            if (personId.HasValue)
            {
                selection.Select(personId.Value);
            }
            else
            {
                selection.Clear();
            }

            if (sceneView != null)
            {
                sceneView.SetSelected(selection.SelectedPersonId);
            }
        }
    }
}
