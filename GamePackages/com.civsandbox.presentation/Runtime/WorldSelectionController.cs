using System;
using CivSandbox.Simulation;
using UnityEngine;

namespace CivSandbox.Presentation
{
    public sealed class WorldSelectionController : MonoBehaviour
    {
        private Camera worldCamera;
        private Action<StableEntityId?> selectionChanged;
        private Action campSelected;

        public void Configure(Camera camera, Action<StableEntityId?> onSelectionChanged, Action onCampSelected = null)
        {
            worldCamera = camera;
            selectionChanged = onSelectionChanged;
            campSelected = onCampSelected;
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                selectionChanged?.Invoke(null);
                return;
            }

            if (worldCamera == null || !Input.GetMouseButtonDown(0) || IsPointerOverHud(Input.mousePosition))
            {
                return;
            }

            Ray ray = worldCamera.ScreenPointToRay(Input.mousePosition);
            RaycastHit[] hits = Physics.RaycastAll(ray, 200f);
            PersonBillboardView nearest = null;
            CampSceneView nearestCamp = null;
            float nearestDistance = float.MaxValue;
            for (int index = 0; index < hits.Length; index++)
            {
                PersonBillboardView candidate = hits[index].collider.GetComponentInParent<PersonBillboardView>();
                if (candidate != null && hits[index].distance < nearestDistance)
                {
                    nearest = candidate;
                    nearestCamp = null;
                    nearestDistance = hits[index].distance;
                }

                CampSceneView camp = hits[index].collider.GetComponentInParent<CampSceneView>();
                if (camp != null && hits[index].distance < nearestDistance)
                {
                    nearest = null;
                    nearestCamp = camp;
                    nearestDistance = hits[index].distance;
                }
            }

            if (nearestCamp != null)
            {
                campSelected?.Invoke();
            }
            else
            {
                selectionChanged?.Invoke(nearest == null ? (StableEntityId?)null : nearest.Id);
            }
        }

        private static bool IsPointerOverHud(Vector3 pointer)
        {
            bool inTopBand = pointer.y >= Screen.height - 380f;
            return inTopBand && (pointer.x <= 380f || pointer.x >= Screen.width - 360f);
        }
    }
}
