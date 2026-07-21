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
        private WorldSimulation simulation;
        private WorldSceneView sceneView;

        public ulong Seed => simulation.Seed.Value;

        public SimulationSpeed Speed { get; private set; } = SimulationSpeed.Paused;

        public WorldSnapshot Snapshot { get; private set; }

        private void Awake()
        {
            simulation = new WorldSimulation(DefaultSeed);
            Snapshot = simulation.CreateSnapshot();

            var sceneObject = new GameObject("Read-only world presentation");
            sceneObject.transform.SetParent(transform, false);
            sceneView = sceneObject.AddComponent<WorldSceneView>();
            sceneView.Initialize(Snapshot);

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

        public void Reset(ulong seed)
        {
            simulation.Reset(seed);
            Speed = SimulationSpeed.Paused;
            Snapshot = simulation.CreateSnapshot();
            sceneView.Apply(Snapshot);
        }

        public void SetSpeed(SimulationSpeed speed)
        {
            Speed = speed;
        }
    }
}
