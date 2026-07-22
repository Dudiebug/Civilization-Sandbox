using System;
using System.Collections;
using CivSandbox.People;
using CivSandbox.Presentation;
using CivSandbox.Simulation;
using CivSandbox.UI;
using CivSandbox.World;
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
        private const ulong FoundingSiteStream = 0x666f756e64696e67UL;
        private WorldViewerSession session;
        private WorldSceneView sceneView;
        private WorldCreationScreen worldCreationScreen;
        private WorldPreviewView worldPreviewView;
        private GameObject worldCreationObject;
        private GameObject worldPreviewObject;
        private Camera worldCreationBackdropCamera;
        private GeneratedWorld activeWorld;
        private GeneratedWorldCell activeFoundingCell;
        private bool overloadWarningActive;
        private double nextOverloadWarningWallTime;

        public ulong Seed => session.Seed;

        public SimulationSpeed Speed => session.Speed;

        public WorldSnapshot Snapshot => session.Snapshot;

        public StableEntityId? SelectedPersonId => session.SelectedPersonId;

        public bool IsCampSelected => session.IsCampSelected;

        public bool IsClockOverloaded => session.IsClockOverloaded;

        public TimeSpan TotalDroppedWallTime => session.TotalDroppedWallTime;

        private void Awake()
        {
            if (Application.isBatchMode)
            {
                GeneratedWorld smokeWorld = WorldGenerator.Generate(WorldGenerationSettings.Default);
                if (!smokeWorld.TryChooseFoundingCell(out GeneratedWorldCell smokeSite))
                {
                    throw new InvalidOperationException("Default generated world has no valid founding region.");
                }

                StartFoundingCamp(smokeWorld, smokeSite);
                return;
            }

            ShowWorldCreation();
        }

        private void ShowWorldCreation()
        {
            worldCreationObject = new GameObject("Title and world creation");
            worldCreationObject.transform.SetParent(transform, false);
            var cameraObject = new GameObject("World creation background camera");
            cameraObject.transform.SetParent(worldCreationObject.transform, false);
            worldCreationBackdropCamera = cameraObject.AddComponent<Camera>();
            worldCreationBackdropCamera.clearFlags = CameraClearFlags.SolidColor;
            worldCreationBackdropCamera.backgroundColor = new Color(0.075f, 0.11f, 0.13f, 1f);
            worldCreationBackdropCamera.depth = -100f;
            worldCreationScreen = worldCreationObject.AddComponent<WorldCreationScreen>();
            worldCreationScreen.Initialize(GenerateWorldPreview, StartFoundingCamp, ClearWorldPreview, ToggleCameraView);
        }

        private void GenerateWorldPreview(WorldGenerationSettings settings)
        {
            ClearWorldPreview();
            GeneratedWorld world = WorldGenerator.Generate(settings);
            CreateWorldPreview(world);
            worldCreationScreen.ShowGeneratedWorld(world);
            if (worldCreationBackdropCamera != null)
            {
                worldCreationBackdropCamera.enabled = false;
            }
        }

        private void CreateWorldPreview(GeneratedWorld world)
        {
            worldPreviewObject = new GameObject("Generated world preview");
            worldPreviewObject.transform.SetParent(transform, false);
            worldPreviewView = worldPreviewObject.AddComponent<WorldPreviewView>();
            worldPreviewView.Initialize(world);
        }

        private void ClearWorldPreview()
        {
            if (worldPreviewObject != null)
            {
                worldPreviewObject.SetActive(false);
                Destroy(worldPreviewObject);
            }

            worldPreviewObject = null;
            worldPreviewView = null;
            if (worldCreationObject != null && worldCreationObject.activeInHierarchy && worldCreationBackdropCamera != null)
            {
                worldCreationBackdropCamera.enabled = true;
            }
        }

        private void StartFoundingCamp(GeneratedWorld world, GeneratedWorldCell foundingCell)
        {
            if (world == null) throw new ArgumentNullException(nameof(world));
            if (!world.IsFoundingSite(foundingCell))
            {
                throw new ArgumentException("The founding tile must provide a selectable walkable footprint in the generated world.", nameof(foundingCell));
            }

            ulong campSeed = KeyedRandom.Sample(
                world.Settings.Seed.Value,
                FoundingSiteStream,
                foundingCell.Id.Local,
                0,
                0);
            activeWorld = world;
            activeFoundingCell = foundingCell;
            session = new WorldViewerSession(world, foundingCell, campSeed);

            if (worldPreviewView == null)
            {
                CreateWorldPreview(world);
            }

            if (worldCreationObject != null)
            {
                worldCreationObject.SetActive(false);
                Destroy(worldCreationObject);
                worldCreationObject = null;
                worldCreationScreen = null;
                worldCreationBackdropCamera = null;
            }

            var sceneObject = new GameObject("Read-only world presentation");
            sceneObject.transform.SetParent(transform, false);
            sceneView = sceneObject.AddComponent<WorldSceneView>();
            Vector3 foundingPosition = worldPreviewView.CellPosition(foundingCell);
            sceneView.InitializeInGeneratedWorld(
                Snapshot,
                worldPreviewView.PreviewCamera,
                foundingPosition,
                worldPreviewView.WorldUnitsPerMeter,
                worldPreviewView.SurfaceHeightAtWorldPosition);
            worldPreviewView.FocusOn(foundingCell);
            sceneView.WorldCamera.gameObject.AddComponent<WorldSelectionController>().Configure(
                sceneView.WorldCamera,
                SelectPerson,
                SelectCamp);

            var hudObject = new GameObject("World Viewer HUD");
            hudObject.transform.SetParent(transform, false);
            hudObject.AddComponent<WorldViewerHud>().Initialize(this, ToggleCameraView, ToggleResourceMarkers);
        }

        private void ToggleCameraView()
        {
            WorldCameraController controller = worldPreviewView == null || worldPreviewView.PreviewCamera == null
                ? null
                : worldPreviewView.PreviewCamera.GetComponent<WorldCameraController>();
            controller?.ToggleView();
        }

        private void ToggleResourceMarkers()
        {
            worldPreviewView?.ToggleResourceMarkers();
        }

        private IEnumerator Start()
        {
            if (!Application.isBatchMode)
            {
                yield break;
            }

            yield return null;
            Debug.Log(
                $"CIV-BUILD02-WORLDGEN-SMOKE-000: Generated {activeWorld.Columns}x{activeWorld.Rows} world " +
                $"{activeWorld.WorldId:x8} and founded valid {activeFoundingCell.Biome} region {activeFoundingCell.Id}.");
            Debug.Log(
                $"CIV-BUILD02-CAMP-SMOKE-000: Founding camp exposed {Snapshot.Camp.Count} finite reachable resource nodes " +
                $"with {Snapshot.Count} named workers and a conserved shelter project.");
            Debug.Log($"CIV-BUILD01-SMOKE-000: World Viewer reached its first frame with {Snapshot.Count} named people.");
            Application.Quit(0);
        }

        private void Update()
        {
            if (session == null)
            {
                if (worldPreviewView != null &&
                    worldCreationScreen != null &&
                    Input.GetMouseButtonDown(0) &&
                    worldCreationScreen.CanPickWorld(Input.mousePosition) &&
                    worldPreviewView.TrySelect(Input.mousePosition, out GeneratedWorldCell selectedCell))
                {
                    worldPreviewView.SetSelected(selectedCell.Id);
                    worldCreationScreen.SelectCell(selectedCell);
                }

                return;
            }

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
            sceneView.SetCampSelected(false);
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
                sceneView.SetCampSelected(false);
            }
        }

        public void SelectCamp()
        {
            session.SelectCamp();
            if (sceneView != null)
            {
                sceneView.SetSelected(null);
                sceneView.SetCampSelected(true);
            }
        }
    }
}
