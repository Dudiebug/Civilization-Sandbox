using CivSandbox.World;
using UnityEngine;

namespace CivSandbox.Presentation
{
    public enum WorldCameraView : byte
    {
        Angled = 0,
        TopDown = 1,
        Free = 2
    }

    [RequireComponent(typeof(Camera))]
    public sealed class WorldCameraController : MonoBehaviour
    {
        private float minimumZoom = 7f;
        private float maximumZoom = 31f;
        private Camera controlledCamera;
        private float minimumEast;
        private float maximumEast;
        private float minimumNorth;
        private float maximumNorth;
        private Vector3 previousPointer;
        private Vector3 previousOrbitPointer;
        private float orbitDistance = 100f;
        private float yawDegrees = 45f;
        private float pitchDegrees = 43f;

        public float Zoom => controlledCamera == null ? 0f : controlledCamera.orthographicSize;

        public WorldCameraView ViewMode { get; private set; } = WorldCameraView.Angled;

        public void Configure(WorldBounds bounds)
        {
            Configure(
                bounds.MinimumEastMillimeters / 1000f,
                bounds.MaximumEastMillimeters / 1000f,
                bounds.MinimumNorthMillimeters / 1000f,
                bounds.MaximumNorthMillimeters / 1000f,
                7f,
                31f);
        }

        public void Configure(float eastMinimum, float eastMaximum, float northMinimum, float northMaximum, float zoomMinimum, float zoomMaximum)
        {
            controlledCamera = GetComponent<Camera>();
            minimumEast = eastMinimum;
            maximumEast = eastMaximum;
            minimumNorth = northMinimum;
            maximumNorth = northMaximum;
            minimumZoom = zoomMinimum;
            maximumZoom = Mathf.Max(zoomMinimum, zoomMaximum);
            controlledCamera.orthographicSize = Mathf.Clamp(controlledCamera.orthographicSize, minimumZoom, maximumZoom);
            CaptureOrbitFromCurrentTransform();
            ClampFocusToBounds();
        }

        public void ToggleView()
        {
            SetViewMode(ViewMode == WorldCameraView.TopDown ? WorldCameraView.Angled : WorldCameraView.TopDown);
        }

        public void SetViewMode(WorldCameraView mode)
        {
            if (mode == WorldCameraView.Free)
            {
                ViewMode = mode;
                return;
            }

            if (controlledCamera == null)
            {
                controlledCamera = GetComponent<Camera>();
            }

            if (!TryCenterGroundPoint(out Vector3 focus))
            {
                focus = Vector3.zero;
            }

            pitchDegrees = mode == WorldCameraView.TopDown ? 78f : 43f;
            yawDegrees = mode == WorldCameraView.TopDown ? 0f : 45f;
            ViewMode = mode;
            ApplyOrbit(focus);
            ClampFocusToBounds();
        }

        public void Focus(Vector3 groundPosition, float orthographicSize)
        {
            if (controlledCamera == null)
            {
                controlledCamera = GetComponent<Camera>();
            }

            controlledCamera.orthographicSize = Mathf.Clamp(orthographicSize, minimumZoom, maximumZoom);
            if (TryGroundPoint(new Vector3(Screen.width * 0.5f, Screen.height * 0.5f, 0f), out Vector3 currentFocus))
            {
                transform.position += groundPosition - currentFocus;
            }

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
            HandleRotation();
            HandlePointerZoom();
        }

        public void SetViewForVerification(Vector3 cameraPosition, float orthographicSize)
        {
            if (controlledCamera == null)
            {
                controlledCamera = GetComponent<Camera>();
            }

            transform.position = cameraPosition;
            controlledCamera.orthographicSize = Mathf.Clamp(orthographicSize, minimumZoom, maximumZoom);
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
            controlledCamera.orthographicSize = Mathf.Clamp(controlledCamera.orthographicSize * Mathf.Exp(-wheel * 0.14f), minimumZoom, maximumZoom);
            if (hadBefore && TryGroundPoint(Input.mousePosition, out Vector3 after))
            {
                transform.position += before - after;
            }

            ClampFocusToBounds();
        }

        private void HandleRotation()
        {
            if (Input.GetKeyDown(KeyCode.V))
            {
                ToggleView();
                return;
            }

            if (Input.GetKeyDown(KeyCode.Q) || Input.GetKeyDown(KeyCode.E))
            {
                yawDegrees += Input.GetKeyDown(KeyCode.Q) ? -45f : 45f;
                ViewMode = WorldCameraView.Free;
                OrbitAroundCurrentFocus();
            }

            if (Input.GetMouseButtonDown(1))
            {
                previousOrbitPointer = Input.mousePosition;
            }

            if (!Input.GetMouseButton(1) || Input.mousePosition.x < 420f)
            {
                return;
            }

            Vector3 pointer = Input.mousePosition;
            Vector3 delta = pointer - previousOrbitPointer;
            previousOrbitPointer = pointer;
            if (delta.sqrMagnitude < 0.01f)
            {
                return;
            }

            yawDegrees += delta.x * 0.25f;
            pitchDegrees = Mathf.Clamp(pitchDegrees - delta.y * 0.20f, 35f, 82f);
            ViewMode = WorldCameraView.Free;
            OrbitAroundCurrentFocus();
        }

        private void OrbitAroundCurrentFocus()
        {
            if (TryCenterGroundPoint(out Vector3 focus))
            {
                ApplyOrbit(focus);
                ClampFocusToBounds();
            }
        }

        private void ApplyOrbit(Vector3 focus)
        {
            float pitchRadians = pitchDegrees * Mathf.Deg2Rad;
            float yawRadians = yawDegrees * Mathf.Deg2Rad;
            float horizontal = Mathf.Cos(pitchRadians) * orbitDistance;
            Vector3 offset = new Vector3(
                Mathf.Sin(yawRadians) * horizontal,
                Mathf.Sin(pitchRadians) * orbitDistance,
                -Mathf.Cos(yawRadians) * horizontal);
            transform.position = focus + offset;
            transform.rotation = Quaternion.LookRotation(focus - transform.position, Vector3.up);
        }

        private void CaptureOrbitFromCurrentTransform()
        {
            if (!TryCenterGroundPoint(out Vector3 focus))
            {
                return;
            }

            Vector3 offset = transform.position - focus;
            orbitDistance = Mathf.Max(10f, offset.magnitude);
            float horizontal = new Vector2(offset.x, offset.z).magnitude;
            pitchDegrees = Mathf.Atan2(offset.y, horizontal) * Mathf.Rad2Deg;
            yawDegrees = Mathf.Atan2(offset.x, -offset.z) * Mathf.Rad2Deg;
            ViewMode = pitchDegrees >= 65f ? WorldCameraView.TopDown : WorldCameraView.Angled;
        }

        private bool TryCenterGroundPoint(out Vector3 point)
        {
            return TryGroundPoint(new Vector3(Screen.width * 0.5f, Screen.height * 0.5f, 0f), out point);
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
            if (controlledCamera == null || !TryCenterGroundPoint(out Vector3 focus))
            {
                return;
            }

            float clampedEast = Mathf.Clamp(focus.x, minimumEast, maximumEast);
            float clampedNorth = Mathf.Clamp(focus.z, minimumNorth, maximumNorth);
            transform.position += new Vector3(clampedEast - focus.x, 0f, clampedNorth - focus.z);
        }
    }
}
