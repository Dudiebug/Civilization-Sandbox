using System;
using CivSandbox.People;
using UnityEngine;

namespace CivSandbox.Presentation
{
    public sealed class CampSceneView : MonoBehaviour
    {
        private readonly GameObject[] stockpileMarkers = new GameObject[4];
        private GameObject selectionMarker;
        private Transform shelterFloor;
        private Transform shelterWalls;
        private GameObject shelterRoof;

        public void Initialize(
            CampSnapshot camp,
            Vector3 worldOffset,
            float positionScale,
            Func<Vector3, float> terrainHeight)
        {
            gameObject.name = "Founding camp and shared stockpile";
            Vector3 campHorizontal = new Vector3(worldOffset.x, 0f, worldOffset.z);
            float surface = terrainHeight == null ? worldOffset.y : terrainHeight(campHorizontal);
            transform.position = new Vector3(worldOffset.x, surface + 0.04f, worldOffset.z);

            CreateStockpileMarker(0, "Water stockpile", new Vector3(-2.2f, 0.18f, -1.4f), new Color(0.18f, 0.52f, 0.72f));
            CreateStockpileMarker(1, "Food stockpile", new Vector3(-1.3f, 0.18f, -1.4f), new Color(0.78f, 0.60f, 0.18f));
            CreateStockpileMarker(2, "Timber stockpile", new Vector3(-2.2f, 0.18f, -0.5f), new Color(0.34f, 0.19f, 0.08f));
            CreateStockpileMarker(3, "Stone stockpile", new Vector3(-1.3f, 0.18f, -0.5f), new Color(0.42f, 0.43f, 0.42f));
            CreateShelter();
            CreateSelectionMarker();
            Apply(camp);
        }

        public void Apply(CampSnapshot camp)
        {
            CampStockpileSnapshot stockpile = camp.Stockpile;
            SetPile(stockpileMarkers[0], stockpile.WaterUnits);
            SetPile(stockpileMarkers[1], stockpile.FoodUnits);
            SetPile(stockpileMarkers[2], stockpile.TimberUnits);
            SetPile(stockpileMarkers[3], stockpile.StoneUnits);

            ShelterSnapshot shelter = camp.Shelter;
            float progress = Mathf.Clamp01(shelter.ProgressPercent / 100f);
            shelterFloor.localScale = new Vector3(3.8f, 0.16f, 2.8f);
            shelterWalls.localScale = new Vector3(3.5f, Mathf.Max(0.08f, 1.8f * progress), 2.5f);
            shelterWalls.localPosition = new Vector3(2.8f, shelterWalls.localScale.y * 0.5f + 0.12f, 1.9f);
            shelterWalls.gameObject.SetActive(shelter.MaterialsCommitted);
            shelterRoof.SetActive(shelter.IsComplete);
        }

        public void SetSelected(bool selected)
        {
            if (selectionMarker != null) selectionMarker.SetActive(selected);
        }

        private void CreateStockpileMarker(int index, string markerName, Vector3 position, Color color)
        {
            GameObject marker = GameObject.CreatePrimitive(PrimitiveType.Cube);
            marker.name = markerName;
            marker.transform.SetParent(transform, false);
            marker.transform.localPosition = position;
            RuntimeObjectLifecycle.Destroy(marker.GetComponent<Collider>());
            marker.GetComponent<MeshRenderer>().sharedMaterial = EraMaterialFactory.CreateLit(color);
            stockpileMarkers[index] = marker;
        }

        private static void SetPile(GameObject marker, int units)
        {
            float visible = units <= 0 ? 0.08f : Mathf.Clamp(0.18f + units * 0.012f, 0.18f, 0.85f);
            marker.transform.localScale = new Vector3(0.62f, visible, 0.62f);
        }

        private void CreateShelter()
        {
            GameObject floor = GameObject.CreatePrimitive(PrimitiveType.Cube);
            floor.name = "Shelter foundation";
            floor.transform.SetParent(transform, false);
            floor.transform.localPosition = new Vector3(2.8f, 0.08f, 1.9f);
            floor.GetComponent<MeshRenderer>().sharedMaterial = EraMaterialFactory.CreateLit(new Color(0.35f, 0.23f, 0.11f));
            shelterFloor = floor.transform;

            GameObject walls = GameObject.CreatePrimitive(PrimitiveType.Cube);
            walls.name = "Shelter construction progress";
            walls.transform.SetParent(transform, false);
            RuntimeObjectLifecycle.Destroy(walls.GetComponent<Collider>());
            walls.GetComponent<MeshRenderer>().sharedMaterial = EraMaterialFactory.CreateLit(new Color(0.46f, 0.29f, 0.13f));
            shelterWalls = walls.transform;

            shelterRoof = GameObject.CreatePrimitive(PrimitiveType.Cube);
            shelterRoof.name = "Completed shelter roof";
            shelterRoof.transform.SetParent(transform, false);
            shelterRoof.transform.localPosition = new Vector3(2.8f, 2.02f, 1.9f);
            shelterRoof.transform.localRotation = Quaternion.Euler(0f, 0f, 8f);
            shelterRoof.transform.localScale = new Vector3(4.1f, 0.22f, 3.1f);
            RuntimeObjectLifecycle.Destroy(shelterRoof.GetComponent<Collider>());
            shelterRoof.GetComponent<MeshRenderer>().sharedMaterial = EraMaterialFactory.CreateLit(new Color(0.29f, 0.16f, 0.07f));
        }

        private void CreateSelectionMarker()
        {
            selectionMarker = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
            selectionMarker.name = "Selected camp marker";
            selectionMarker.transform.SetParent(transform, false);
            selectionMarker.transform.localPosition = new Vector3(0.4f, 0.02f, 0.2f);
            selectionMarker.transform.localScale = new Vector3(5.2f, 0.02f, 4.5f);
            RuntimeObjectLifecycle.Destroy(selectionMarker.GetComponent<Collider>());
            selectionMarker.GetComponent<MeshRenderer>().sharedMaterial = EraMaterialFactory.CreateUnlit(
                new Color(0.95f, 0.57f, 0.10f, 0.55f));
            selectionMarker.SetActive(false);
        }
    }
}
