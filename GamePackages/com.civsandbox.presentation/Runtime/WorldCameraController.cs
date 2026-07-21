using CivSandbox.World;
using UnityEngine;

namespace CivSandbox.Presentation
{
    [RequireComponent(typeof(Camera))]
    public sealed class WorldCameraController : MonoBehaviour
    {
        private const float MinimumZoom = 7f;
        private const float MaximumZoom = 31f;
        private Camera controlledCamera;
        private float minimumEast;
        private float maximumEast;
        private float minimumNorth;
        private float maximumNorth;
        private Vector3 previousPointer;

        public float Zoom => controlledCamera == null ? 0f : controlledCamera.orthographicSize;

        public void Configure(WorldBounds bounds)
        {
            controlledCamera = GetComponent<Camera>();
            minimumEast = bounds.MinimumEastMillimeters / 1000f;
            maximumEast = bounds.MaximumEastMillimeters / 1000f;
            minimumNorth = bounds.MinimumNorthMillimeters / 1000f;
            maximumNorth = bounds.MaximumNorthMillimeters / 1000f;
            ClampFocusToBounds();
        }

        private void Update()
        {
            if (controlledCamera == null)
            {
                return;
            }

            HandleKeyboardPan();
            HandlePointerDrag();
            HandlePointerZoom();
        }

        public void SetViewForVerification(Vector3 cameraPosition, float orthographicSize)
        {
            if (controlledCamera == null)
            {
                controlledCamera = GetComponent<Camera>();
            }

            transform.position = cameraPosition;
            controlledCamera.orthographicSize = Mathf.Clamp(orthographicSize, MinimumZoom, MaximumZoom);
            ClampFocusToBounds();
        }

        private void HandleKeyboardPan()
        {
            float eastInput = 0f;
            float northInput = 0f;
            if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow)) eastInput -= 1f;
            if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow)) eastInput += 1f;
            if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow)) northInput -= 1f;
            if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow)) northInput += 1f;
            if (Mathf.Approximately(eastInput, 0f) && Mathf.Approximately(northInput, 0f))
            {
                return;
            }

            Vector3 right = transform.right;
            right.y = 0f;
            right.Normalize();
            Vector3 forward = transform.forward;
            forward.y = 0f;
            forward.Normalize();
            Vector3 direction = (right * eastInput + forward * northInput).normalized;
            transform.position += direction * (controlledCamera.orthographicSize * 0.85f * Time.unscaledDeltaTime);
            ClampFocusToBounds();
        }

        private void HandlePointerDrag()
        {
            if (Input.GetMouseButtonDown(2))
            {
                previousPointer = Input.mousePosition;
            }

            if (!Input.GetMouseButton(2))
            {
                return;
            }

            Vector3 pointer = Input.mousePosition;
            Vector3 delta = pointer - previousPointer;
            previousPointer = pointer;
            float unitsPerPixel = controlledCamera.orthographicSize * 2f / Mathf.Max(1f, Screen.height);
            Vector3 right = transform.right;
            right.y = 0f;
            right.Normalize();
            Vector3 forward = transform.forward;
            forward.y = 0f;
            forward.Normalize();
            transform.position += (-right * delta.x - forward * delta.y) * unitsPerPixel;
            ClampFocusToBounds();
        }

        private void HandlePointerZoom()
        {
            float wheel = Input.mouseScrollDelta.y;
            if (Mathf.Approximately(wheel, 0f) || Input.mousePosition.x < 380f)
            {
                return;
            }

            bool hadBefore = TryGroundPoint(Input.mousePosition, out Vector3 before);
            controlledCamera.orthographicSize = Mathf.Clamp(controlledCamera.orthographicSize * Mathf.Exp(-wheel * 0.14f), MinimumZoom, MaximumZoom);
            if (hadBefore && TryGroundPoint(Input.mousePosition, out Vector3 after))
            {
                transform.position += before - after;
            }

            ClampFocusToBounds();
        }

        private bool TryGroundPoint(Vector3 screenPoint, out Vector3 point)
        {
            var ground = new Plane(Vector3.up, Vector3.zero);
            Ray ray = controlledCamera.ScreenPointToRay(screenPoint);
            if (ground.Raycast(ray, out float distance))
            {
                point = ray.GetPoint(distance);
                return true;
            }

            point = Vector3.zero;
            return false;
        }

        private void ClampFocusToBounds()
        {
            if (controlledCamera == null || !TryGroundPoint(new Vector3(Screen.width * 0.5f, Screen.height * 0.5f, 0f), out Vector3 focus))
            {
                return;
            }

            float clampedEast = Mathf.Clamp(focus.x, minimumEast, maximumEast);
            float clampedNorth = Mathf.Clamp(focus.z, minimumNorth, maximumNorth);
            transform.position += new Vector3(clampedEast - focus.x, 0f, clampedNorth - focus.z);
        }
    }
}
