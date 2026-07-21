using System;
using CivSandbox.Simulation;
using UnityEngine;

namespace CivSandbox.Presentation
{
    public sealed class WorldSelectionController : MonoBehaviour
    {
        private Camera worldCamera;
        private Action<StableEntityId?> selectionChanged;

        public void Configure(Camera camera, Action<StableEntityId?> onSelectionChanged)
        {
            worldCamera = camera;
            selectionChanged = onSelectionChanged;
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
            float nearestDistance = float.MaxValue;
            for (int index = 0; index < hits.Length; index++)
            {
                PersonBillboardView candidate = hits[index].collider.GetComponentInParent<PersonBillboardView>();
                if (candidate != null && hits[index].distance < nearestDistance)
                {
                    nearest = candidate;
                    nearestDistance = hits[index].distance;
                }
            }

            selectionChanged?.Invoke(nearest == null ? (StableEntityId?)null : nearest.Id);
        }

        private static bool IsPointerOverHud(Vector3 pointer)
        {
            bool inTopBand = pointer.y >= Screen.height - 380f;
            return inTopBand && (pointer.x <= 380f || pointer.x >= Screen.width - 360f);
        }
    }
}
