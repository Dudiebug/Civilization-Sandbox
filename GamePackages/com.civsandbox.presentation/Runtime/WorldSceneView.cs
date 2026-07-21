using System.Collections.Generic;
using CivSandbox.People;
using CivSandbox.Simulation;
using UnityEngine;

namespace CivSandbox.Presentation
{
    public sealed class WorldSceneView : MonoBehaviour
    {
        private readonly Dictionary<StableEntityId, PersonBillboardView> people = new Dictionary<StableEntityId, PersonBillboardView>();
        private Camera worldCamera;

        public Camera WorldCamera => worldCamera;

        public void Initialize(WorldSnapshot snapshot)
        {
            worldCamera = CreateCamera();
            CreateLight();
            CreateGround(snapshot);
            CreateBoundaryMarkers(snapshot);
            Apply(snapshot, true);
        }

        public void Apply(WorldSnapshot snapshot, bool snap = false)
        {
            for (int index = 0; index < snapshot.Count; index++)
            {
                PersonSnapshot person = snapshot[index];
                if (!people.TryGetValue(person.Id, out PersonBillboardView view))
                {
                    var personObject = new GameObject();
                    personObject.transform.SetParent(transform, false);
                    view = personObject.AddComponent<PersonBillboardView>();
                    view.Initialize(person, worldCamera);
                    people.Add(person.Id, view);
                }

                view.Apply(person, snap);
            }
        }

        private static Camera CreateCamera()
        {
            var cameraObject = new GameObject("World Camera");
            cameraObject.tag = "MainCamera";
            var camera = cameraObject.AddComponent<Camera>();
            camera.orthographic = true;
            camera.orthographicSize = 24f;
            camera.nearClipPlane = 0.1f;
            camera.farClipPlane = 180f;
            camera.backgroundColor = new Color(0.36f, 0.46f, 0.45f);
            camera.clearFlags = CameraClearFlags.SolidColor;
            camera.transform.position = new Vector3(31f, 36f, -31f);
            camera.transform.rotation = Quaternion.LookRotation(new Vector3(-31f, -36f, 31f), Vector3.up);
            return camera;
        }

        private static void CreateLight()
        {
            var lightObject = new GameObject("Late afternoon sun");
            var light = lightObject.AddComponent<Light>();
            light.type = LightType.Directional;
            light.color = new Color(1f, 0.88f, 0.68f);
            light.intensity = 1.1f;
            light.shadows = LightShadows.Soft;
            lightObject.transform.rotation = Quaternion.Euler(46f, -32f, 0f);
            RenderSettings.ambientLight = new Color(0.34f, 0.35f, 0.28f);
        }

        private void CreateGround(WorldSnapshot snapshot)
        {
            float width = (snapshot.Bounds.MaximumEastMillimeters - snapshot.Bounds.MinimumEastMillimeters) / 1000f;
            float depth = (snapshot.Bounds.MaximumNorthMillimeters - snapshot.Bounds.MinimumNorthMillimeters) / 1000f;
            GameObject ground = GameObject.CreatePrimitive(PrimitiveType.Cube);
            ground.name = "Bounded common ground";
            ground.transform.SetParent(transform, false);
            ground.transform.localPosition = new Vector3(0f, -0.3f, 0f);
            ground.transform.localScale = new Vector3(width, 0.5f, depth);
            ground.GetComponent<MeshRenderer>().sharedMaterial = EraMaterialFactory.CreateLit(new Color(0.29f, 0.39f, 0.20f));
        }

        private void CreateBoundaryMarkers(WorldSnapshot snapshot)
        {
            float east = snapshot.Bounds.MaximumEastMillimeters / 1000f;
            float west = snapshot.Bounds.MinimumEastMillimeters / 1000f;
            float north = snapshot.Bounds.MaximumNorthMillimeters / 1000f;
            float south = snapshot.Bounds.MinimumNorthMillimeters / 1000f;
            CreateRail(new Vector3(0f, 0.18f, north), new Vector3(east - west, 0.35f, 0.25f));
            CreateRail(new Vector3(0f, 0.18f, south), new Vector3(east - west, 0.35f, 0.25f));
            CreateRail(new Vector3(east, 0.18f, 0f), new Vector3(0.25f, 0.35f, north - south));
            CreateRail(new Vector3(west, 0.18f, 0f), new Vector3(0.25f, 0.35f, north - south));
        }

        private void CreateRail(Vector3 position, Vector3 scale)
        {
            GameObject rail = GameObject.CreatePrimitive(PrimitiveType.Cube);
            rail.name = "Timber boundary";
            rail.transform.SetParent(transform, false);
            rail.transform.localPosition = position;
            rail.transform.localScale = scale;
            Object.Destroy(rail.GetComponent<Collider>());
            rail.GetComponent<MeshRenderer>().sharedMaterial = EraMaterialFactory.CreateLit(new Color(0.25f, 0.14f, 0.07f));
        }
    }
}
