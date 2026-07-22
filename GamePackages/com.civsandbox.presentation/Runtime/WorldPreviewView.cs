using System;
using System.Collections.Generic;
using CivSandbox.Simulation;
using CivSandbox.World;
using UnityEngine;
using UnityEngine.Rendering;

namespace CivSandbox.Presentation
{
    public sealed class WorldPreviewView : MonoBehaviour
    {
        private const float CellSize = 1f;
        private const int TerrainChunkSize = 64;
        private GeneratedWorld world;
        private Camera previewCamera;
        private GameObject selectionMarker;
        private GameObject landmarkObject;
        private float originEast;
        private float originNorth;

        public Camera PreviewCamera => previewCamera;

        public float WorldUnitsPerMeter => CellSize;

        public int TerrainChunkCount { get; private set; }

        public int LandmarkCount { get; private set; }

        public bool ResourceMarkersVisible => landmarkObject != null && landmarkObject.activeSelf;

        public void ToggleResourceMarkers()
        {
            if (landmarkObject != null) landmarkObject.SetActive(!landmarkObject.activeSelf);
        }

        public Vector3 CellPosition(GeneratedWorldCell cell)
        {
            return CellCenter(cell, CellSurfaceHeight(cell) + 0.04f);
        }

        public float SurfaceHeightAtWorldPosition(Vector3 worldPosition)
        {
            float gridEast = Mathf.Clamp((worldPosition.x - originEast) / CellSize, 0f, world.Columns);
            float gridNorth = Mathf.Clamp((worldPosition.z - originNorth) / CellSize, 0f, world.Rows);
            int column = Mathf.Min(world.Columns - 1, Mathf.FloorToInt(gridEast));
            int row = Mathf.Min(world.Rows - 1, Mathf.FloorToInt(gridNorth));
            float eastFraction = gridEast - column;
            float northFraction = gridNorth - row;
            float south = Mathf.Lerp(VertexHeight(column, row), VertexHeight(column + 1, row), eastFraction);
            float north = Mathf.Lerp(VertexHeight(column, row + 1), VertexHeight(column + 1, row + 1), eastFraction);
            return Mathf.Lerp(south, north, northFraction);
        }

        public void FocusOn(GeneratedWorldCell cell)
        {
            WorldCameraController controller = previewCamera == null
                ? null
                : previewCamera.GetComponent<WorldCameraController>();
            controller?.Focus(CellPosition(cell), 14f);
        }

        public void Initialize(GeneratedWorld generatedWorld)
        {
            world = generatedWorld ?? throw new ArgumentNullException(nameof(generatedWorld));
            originEast = -world.Columns * CellSize * 0.5f;
            originNorth = -world.Rows * CellSize * 0.5f;
            previewCamera = CreateCamera();
            CreateLight();
            CreateTerrain();
            CreateWater();
            CreateLandmarks();
            CreateSelectionMarker();
        }

        public bool TrySelect(Vector3 screenPoint, out GeneratedWorldCell selectedCell)
        {
            if (previewCamera == null)
            {
                selectedCell = default;
                return false;
            }

            Ray ray = previewCamera.ScreenPointToRay(screenPoint);
            if (ray.direction.y >= -0.0001f)
            {
                selectedCell = default;
                return false;
            }

            GeneratedWorldCell cell = default;
            float targetHeight = 0f;
            for (int iteration = 0; iteration < 3; iteration++)
            {
                float distance = (targetHeight - ray.origin.y) / ray.direction.y;
                if (distance < 0f)
                {
                    selectedCell = default;
                    return false;
                }

                Vector3 point = ray.GetPoint(distance);
                int column = Mathf.FloorToInt((point.x - originEast) / CellSize);
                int row = Mathf.FloorToInt((point.z - originNorth) / CellSize);
                if (column < 0 || column >= world.Columns || row < 0 || row >= world.Rows)
                {
                    selectedCell = default;
                    return false;
                }

                cell = world.CellAt(column, row);
                targetHeight = CellSurfaceHeight(cell);
            }

            if (!world.IsFoundingSite(cell))
            {
                selectedCell = default;
                return false;
            }

            selectedCell = cell;
            return true;
        }

        public void SetSelected(StableEntityId? regionId)
        {
            if (selectionMarker == null)
            {
                return;
            }

            if (!regionId.HasValue || !world.TryGetCell(regionId.Value, out GeneratedWorldCell cell))
            {
                selectionMarker.SetActive(false);
                return;
            }

            selectionMarker.transform.localPosition = CellCenter(cell, CellSurfaceHeight(cell) + 0.11f);
            selectionMarker.SetActive(true);
        }

        private Camera CreateCamera()
        {
            var cameraObject = new GameObject("World creation camera");
            cameraObject.transform.SetParent(transform, false);
            var camera = cameraObject.AddComponent<Camera>();
            cameraObject.tag = "MainCamera";
            camera.orthographic = true;
            camera.nearClipPlane = 0.1f;
            camera.farClipPlane = Mathf.Max(300f, Mathf.Max(world.Columns, world.Rows) * CellSize * 4f);
            camera.backgroundColor = new Color(0.075f, 0.11f, 0.13f, 1f);
            camera.clearFlags = CameraClearFlags.SolidColor;
            float horizontalExtent = world.Columns * CellSize;
            float verticalExtent = world.Rows * CellSize;
            float span = Mathf.Max(horizontalExtent, verticalExtent);
            camera.orthographicSize = span * 0.72f;
            camera.transform.position = new Vector3(span * 0.78f, span * 1.02f, -span * 0.78f);
            camera.transform.rotation = Quaternion.LookRotation(-camera.transform.position.normalized, Vector3.up);
            float halfWidth = horizontalExtent * 0.5f;
            float halfDepth = verticalExtent * 0.5f;
            WorldCameraController controller = cameraObject.AddComponent<WorldCameraController>();
            controller.Configure(
                -halfWidth,
                halfWidth,
                -halfDepth,
                halfDepth,
                4.5f,
                Mathf.Max(12f, camera.orthographicSize * 1.15f));
            controller.SetViewMode(WorldCameraView.TopDown);
            return camera;
        }

        private void CreateLight()
        {
            var lightObject = new GameObject("World preview sun");
            lightObject.transform.SetParent(transform, false);
            var light = lightObject.AddComponent<Light>();
            light.type = LightType.Directional;
            light.color = new Color(1f, 0.90f, 0.73f);
            light.intensity = 1.05f;
            light.shadows = LightShadows.None;
            lightObject.transform.rotation = Quaternion.Euler(48f, -38f, 0f);
            RenderSettings.ambientLight = new Color(0.32f, 0.34f, 0.30f);
        }

        private void CreateTerrain()
        {
            for (int startRow = 0; startRow < world.Rows; startRow += TerrainChunkSize)
            {
                int rowCount = Math.Min(TerrainChunkSize, world.Rows - startRow);
                for (int startColumn = 0; startColumn < world.Columns; startColumn += TerrainChunkSize)
                {
                    int columnCount = Math.Min(TerrainChunkSize, world.Columns - startColumn);
                    CreateTerrainChunk(startColumn, startRow, columnCount, rowCount);
                }
            }
        }

        private void CreateTerrainChunk(int startColumn, int startRow, int columnCount, int rowCount)
        {
            var vertices = new List<Vector3>(columnCount * rowCount * 4);
            var colors = new List<Color32>(columnCount * rowCount * 4);
            var triangles = new List<int>(columnCount * rowCount * 6);

            for (int localRow = 0; localRow < rowCount; localRow++)
            {
                int gridRow = startRow + localRow;
                for (int localColumn = 0; localColumn < columnCount; localColumn++)
                {
                    int gridColumn = startColumn + localColumn;
                    float west = originEast + gridColumn * CellSize;
                    float east = west + CellSize;
                    float south = originNorth + gridRow * CellSize;
                    float north = south + CellSize;
                    int vertex = vertices.Count;
                    vertices.Add(new Vector3(west, VertexHeight(gridColumn, gridRow), south));
                    vertices.Add(new Vector3(west, VertexHeight(gridColumn, gridRow + 1), north));
                    vertices.Add(new Vector3(east, VertexHeight(gridColumn + 1, gridRow + 1), north));
                    vertices.Add(new Vector3(east, VertexHeight(gridColumn + 1, gridRow), south));
                    Color32 color = BiomeColor(world.CellAt(gridColumn, gridRow));
                    colors.Add(color);
                    colors.Add(color);
                    colors.Add(color);
                    colors.Add(color);
                    triangles.Add(vertex);
                    triangles.Add(vertex + 1);
                    triangles.Add(vertex + 2);
                    triangles.Add(vertex);
                    triangles.Add(vertex + 2);
                    triangles.Add(vertex + 3);
                }
            }

            var mesh = new Mesh
            {
                name = $"Semantic terrain chunk {startColumn / TerrainChunkSize},{startRow / TerrainChunkSize}",
                indexFormat = vertices.Count > 65535 ? IndexFormat.UInt32 : IndexFormat.UInt16
            };
            mesh.SetVertices(vertices);
            mesh.SetColors(colors);
            mesh.SetTriangles(triangles, 0);
            mesh.RecalculateBounds();
            mesh.RecalculateNormals();

            var terrainObject = new GameObject($"Terrain chunk {startColumn / TerrainChunkSize},{startRow / TerrainChunkSize}");
            terrainObject.transform.SetParent(transform, false);
            terrainObject.AddComponent<MeshFilter>().sharedMesh = mesh;
            terrainObject.AddComponent<MeshRenderer>().sharedMaterial = EraMaterialFactory.CreateTerrain();
            TerrainChunkCount++;
        }

        private void CreateWater()
        {
            GameObject water = GameObject.CreatePrimitive(PrimitiveType.Cube);
            water.name = "World sea level";
            water.transform.SetParent(transform, false);
            water.transform.localPosition = new Vector3(0f, -0.015f, 0f);
            water.transform.localScale = new Vector3(world.Columns * CellSize + 1f, 0.035f, world.Rows * CellSize + 1f);
            RuntimeObjectLifecycle.Destroy(water.GetComponent<Collider>());
            water.GetComponent<MeshRenderer>().sharedMaterial = EraMaterialFactory.CreateUnlit(new Color(0.12f, 0.36f, 0.48f, 0.78f));
        }

        private void CreateLandmarks()
        {
            var vertices = new List<Vector3>();
            var colors = new List<Color32>();
            var triangles = new List<int>();
            int maximumLandmarks = Math.Min(32768, Math.Max(1152, world.CellCount / 8));
            for (int index = 0; index < world.CellCount && LandmarkCount < maximumLandmarks; index++)
            {
                GeneratedWorldCell cell = world[index];
                ulong sample = KeyedRandom.Sample(world.WorldId, 0x6c616e646d61726bUL, cell.Id.Local, 0, 0);
                if (sample % (ulong)world.CellCount >= (ulong)maximumLandmarks ||
                    !TryChooseVisibleResource(cell, out WorldResourceKind kind, out int amount))
                {
                    continue;
                }

                float surface = cell.IsWater ? 0.04f : CellSurfaceHeight(cell);
                Vector3 center = CellCenter(cell, surface);
                float offsetEast = ((int)(sample >> 8) % 31 - 15) * CellSize / 100f;
                float offsetNorth = ((int)(sample >> 16) % 31 - 15) * CellSize / 100f;
                AddResourceMarker(
                    vertices,
                    colors,
                    triangles,
                    center + new Vector3(offsetEast, 0.02f, offsetNorth),
                    kind,
                    amount,
                    sample);
                LandmarkCount++;
            }

            if (vertices.Count == 0)
            {
                return;
            }

            var mesh = new Mesh
            {
                name = "Generated semantic resource markers",
                indexFormat = vertices.Count > 65535 ? IndexFormat.UInt32 : IndexFormat.UInt16
            };
            mesh.SetVertices(vertices);
            mesh.SetColors(colors);
            mesh.SetTriangles(triangles, 0);
            mesh.RecalculateBounds();
            mesh.RecalculateNormals();
            landmarkObject = new GameObject("Visible trees, food, water, plants, and exposed deposits");
            landmarkObject.transform.SetParent(transform, false);
            landmarkObject.AddComponent<MeshFilter>().sharedMesh = mesh;
            landmarkObject.AddComponent<MeshRenderer>().sharedMaterial = EraMaterialFactory.CreateUnlit(Color.white);
        }

        private void CreateSelectionMarker()
        {
            selectionMarker = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
            selectionMarker.name = "Founding region marker";
            selectionMarker.transform.SetParent(transform, false);
            selectionMarker.transform.localScale = new Vector3(CellSize * 0.92f, 0.035f, CellSize * 0.92f);
            RuntimeObjectLifecycle.Destroy(selectionMarker.GetComponent<Collider>());
            selectionMarker.GetComponent<MeshRenderer>().sharedMaterial = EraMaterialFactory.CreateUnlit(new Color(1f, 0.73f, 0.18f, 0.94f));
            selectionMarker.SetActive(false);
        }

        private Vector3 CellCenter(GeneratedWorldCell cell, float height)
        {
            return new Vector3(
                originEast + (cell.Column + 0.5f) * CellSize,
                height,
                originNorth + (cell.Row + 0.5f) * CellSize);
        }

        private static float SurfaceHeight(GeneratedWorldCell cell)
        {
            if (cell.IsWater)
            {
                return -0.18f - (500 - cell.ElevationPermille) * 0.00065f;
            }

            return 0.08f + (cell.ElevationPermille - 500) * 0.0060f;
        }

        private float VertexHeight(int gridColumn, int gridRow)
        {
            float total = 0f;
            int count = 0;
            for (int rowOffset = -1; rowOffset <= 0; rowOffset++)
            {
                int row = gridRow + rowOffset;
                if (row < 0 || row >= world.Rows) continue;
                for (int columnOffset = -1; columnOffset <= 0; columnOffset++)
                {
                    int column = gridColumn + columnOffset;
                    if (column < 0 || column >= world.Columns) continue;
                    total += SurfaceHeight(world.CellAt(column, row));
                    count++;
                }
            }

            return count == 0 ? 0f : total / count;
        }

        private float CellSurfaceHeight(GeneratedWorldCell cell)
        {
            return (VertexHeight(cell.Column, cell.Row) +
                    VertexHeight(cell.Column + 1, cell.Row) +
                    VertexHeight(cell.Column, cell.Row + 1) +
                    VertexHeight(cell.Column + 1, cell.Row + 1)) * 0.25f;
        }

        private static Color32 BiomeColor(GeneratedWorldCell cell)
        {
            Color32 baseColor;
            switch (cell.Biome)
            {
                case WorldBiome.Ocean:
                    baseColor = new Color32(37, 103, 139, 255);
                    break;
                case WorldBiome.Coast:
                    baseColor = new Color32(201, 181, 112, 255);
                    break;
                case WorldBiome.Woodland:
                    baseColor = new Color32(49, 105, 59, 255);
                    break;
                case WorldBiome.Dryland:
                    baseColor = new Color32(181, 133, 72, 255);
                    break;
                case WorldBiome.Highland:
                    baseColor = new Color32(120, 111, 98, 255);
                    break;
                case WorldBiome.Snow:
                    baseColor = new Color32(218, 228, 222, 255);
                    break;
                default:
                    baseColor = new Color32(112, 151, 77, 255);
                    break;
            }

            int semanticShade = cell.IsWater
                ? (cell.ElevationPermille - 250) / 18
                : (cell.ElevationPermille - 600) / 22 + (cell.MoisturePermille - 500) / 45;
            int pixelVariation = ((((cell.Column * 73) ^ (cell.Row * 151)) % 7) - 3) * 4;
            return Shift(baseColor, semanticShade + pixelVariation);
        }

        private static Color32 Shift(Color32 color, int amount)
        {
            return new Color32(
                (byte)Mathf.Clamp(color.r + amount, 0, 255),
                (byte)Mathf.Clamp(color.g + amount, 0, 255),
                (byte)Mathf.Clamp(color.b + amount, 0, 255),
                color.a);
        }

        private static void AddPyramid(
            List<Vector3> vertices,
            List<Color32> colors,
            List<int> triangles,
            Vector3 center,
            float radius,
            float height,
            Color32 color)
        {
            int start = vertices.Count;
            vertices.Add(center + new Vector3(-radius, 0f, -radius));
            vertices.Add(center + new Vector3(-radius, 0f, radius));
            vertices.Add(center + new Vector3(radius, 0f, radius));
            vertices.Add(center + new Vector3(radius, 0f, -radius));
            vertices.Add(center + new Vector3(0f, height, 0f));
            for (int index = 0; index < 5; index++) colors.Add(color);
            triangles.Add(start); triangles.Add(start + 1); triangles.Add(start + 4);
            triangles.Add(start + 1); triangles.Add(start + 2); triangles.Add(start + 4);
            triangles.Add(start + 2); triangles.Add(start + 3); triangles.Add(start + 4);
            triangles.Add(start + 3); triangles.Add(start); triangles.Add(start + 4);
            triangles.Add(start); triangles.Add(start + 3); triangles.Add(start + 2);
            triangles.Add(start); triangles.Add(start + 2); triangles.Add(start + 1);
        }

        private static bool TryChooseVisibleResource(
            GeneratedWorldCell cell,
            out WorldResourceKind kind,
            out int amount)
        {
            kind = WorldResourceKind.StapleFood;
            amount = 0;
            if (cell.IsWater)
            {
                return false;
            }

            Consider(WorldResourceKind.FreshWater, cell.Resources.FreshWaterPermille, ref kind, ref amount);
            Consider(WorldResourceKind.StapleFood, cell.Resources.StapleFoodPermille, ref kind, ref amount);
            Consider(WorldResourceKind.ProteinFood, cell.Resources.ProteinFoodPermille, ref kind, ref amount);
            Consider(WorldResourceKind.Timber, cell.Resources.TimberPermille, ref kind, ref amount);
            Consider(WorldResourceKind.Stone, cell.Resources.StonePermille, ref kind, ref amount);
            Consider(WorldResourceKind.Clay, cell.Resources.ClayPermille, ref kind, ref amount);
            Consider(WorldResourceKind.Fiber, cell.Resources.FiberPermille, ref kind, ref amount);
            Consider(WorldResourceKind.MedicinalInputs, cell.Resources.MedicinalInputsPermille, ref kind, ref amount);
            if (cell.Biome == WorldBiome.Highland)
            {
                Consider(WorldResourceKind.IronOre, cell.Resources.IronOrePermille - 120, ref kind, ref amount);
                Consider(WorldResourceKind.Coal, cell.Resources.CoalPermille - 180, ref kind, ref amount);
            }

            return amount >= 170;
        }

        private static void Consider(WorldResourceKind candidate, int candidateAmount, ref WorldResourceKind kind, ref int amount)
        {
            if (candidateAmount > amount)
            {
                kind = candidate;
                amount = candidateAmount;
            }
        }

        private static void AddResourceMarker(
            List<Vector3> vertices,
            List<Color32> colors,
            List<int> triangles,
            Vector3 center,
            WorldResourceKind kind,
            int amount,
            ulong sample)
        {
            float richness = Mathf.Lerp(0.78f, 1.18f, Mathf.Clamp01(amount / 1000f));
            float radius;
            float height;
            Color32 color;
            switch (kind)
            {
                case WorldResourceKind.FreshWater:
                    radius = 0.25f; height = 0.08f; color = new Color32(76, 188, 220, 255); break;
                case WorldResourceKind.StapleFood:
                    radius = 0.18f; height = 0.24f; color = new Color32(216, 178, 70, 255); break;
                case WorldResourceKind.ProteinFood:
                    radius = 0.17f; height = 0.20f; color = new Color32(192, 112, 67, 255); break;
                case WorldResourceKind.Timber:
                    radius = 0.30f; height = 0.72f + (sample % 17UL) / 100f; color = new Color32(35, 89, 43, 255); break;
                case WorldResourceKind.Stone:
                    radius = 0.28f; height = 0.32f; color = new Color32(126, 126, 119, 255); break;
                case WorldResourceKind.Clay:
                    radius = 0.24f; height = 0.10f; color = new Color32(166, 91, 57, 255); break;
                case WorldResourceKind.Fiber:
                    radius = 0.15f; height = 0.36f; color = new Color32(176, 162, 72, 255); break;
                case WorldResourceKind.IronOre:
                    radius = 0.25f; height = 0.28f; color = new Color32(132, 69, 49, 255); break;
                case WorldResourceKind.Coal:
                    radius = 0.24f; height = 0.26f; color = new Color32(48, 49, 53, 255); break;
                default:
                    radius = 0.16f; height = 0.25f; color = new Color32(83, 143, 86, 255); break;
            }

            AddPyramid(vertices, colors, triangles, center, radius * richness, height * richness, color);
        }
    }
}
