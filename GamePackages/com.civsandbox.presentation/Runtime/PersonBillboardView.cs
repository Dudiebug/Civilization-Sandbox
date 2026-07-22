using System;
using CivSandbox.People;
using CivSandbox.Simulation;
using UnityEngine;

namespace CivSandbox.Presentation
{
    public sealed class PersonBillboardView : MonoBehaviour
    {
        private Camera worldCamera;
        private Vector3 worldOffset;
        private float positionScale = 1f;
        private Func<Vector3, float> terrainHeight;
        private Vector3 targetPosition;
        private bool hasPosition;
        private GameObject selectionMarker;
        private Transform spriteVisual;

        public StableEntityId Id { get; private set; }

        public void Initialize(PersonSnapshot person, Camera camera)
        {
            Initialize(person, camera, Vector3.zero);
        }

        public void Initialize(PersonSnapshot person, Camera camera, Vector3 positionOffset)
        {
            Initialize(person, camera, positionOffset, 1f);
        }

        public void Initialize(PersonSnapshot person, Camera camera, Vector3 positionOffset, float localPositionScale)
        {
            Initialize(person, camera, positionOffset, localPositionScale, null);
        }

        public void Initialize(
            PersonSnapshot person,
            Camera camera,
            Vector3 positionOffset,
            float localPositionScale,
            Func<Vector3, float> surfaceHeight)
        {
            Id = person.Id;
            worldCamera = camera;
            worldOffset = positionOffset;
            positionScale = localPositionScale;
            terrainHeight = surfaceHeight;
            gameObject.name = $"Person - {person.Name}";

            var spriteObject = new GameObject("Crisp camera-facing pixel person");
            spriteObject.transform.SetParent(transform, false);
            spriteVisual = spriteObject.transform;
            var spriteRenderer = spriteObject.AddComponent<SpriteRenderer>();
            spriteRenderer.sprite = EarlyModernSpriteFactory.Create(person.AppearanceVariant);
            spriteRenderer.sortingOrder = 10;

            var collider = gameObject.AddComponent<BoxCollider>();
            collider.center = new Vector3(0f, 0.9f, 0f);
            collider.size = new Vector3(0.9f, 1.9f, 0.35f);

            CreateShadow();
            CreateSelectionMarker();
            Apply(person, true);
        }

        public void Apply(PersonSnapshot person, bool snap)
        {
            Vector3 horizontalPosition = worldOffset + new Vector3(
                person.Position.EastMillimeters / 1000f * positionScale,
                0f,
                person.Position.NorthMillimeters / 1000f * positionScale);
            targetPosition = new Vector3(
                horizontalPosition.x,
                terrainHeight == null ? worldOffset.y + 0.03f : terrainHeight(horizontalPosition) + 0.03f,
                horizontalPosition.z);
            if (snap || !hasPosition)
            {
                transform.position = targetPosition;
                hasPosition = true;
            }
        }

        public void SetSelected(bool selected)
        {
            if (selectionMarker != null)
            {
                selectionMarker.SetActive(selected);
            }
        }

        private void LateUpdate()
        {
            transform.position = Vector3.Lerp(transform.position, targetPosition, 1f - Mathf.Exp(-14f * Time.unscaledDeltaTime));
            if (worldCamera == null)
            {
                return;
            }

            if (spriteVisual != null)
            {
                spriteVisual.rotation = worldCamera.transform.rotation;
            }
        }

        private void CreateShadow()
        {
            GameObject shadow = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
            shadow.name = "Ground shadow";
            shadow.transform.SetParent(transform, false);
            shadow.transform.localPosition = new Vector3(0f, -0.01f, 0f);
            shadow.transform.localScale = new Vector3(0.45f, 0.01f, 0.25f);
            RuntimeObjectLifecycle.Destroy(shadow.GetComponent<Collider>());
            shadow.GetComponent<MeshRenderer>().sharedMaterial = EraMaterialFactory.CreateUnlit(new Color(0.12f, 0.10f, 0.07f, 0.55f));
        }

        private void CreateSelectionMarker()
        {
            selectionMarker = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
            selectionMarker.name = "Selected person marker";
            selectionMarker.transform.SetParent(transform, false);
            selectionMarker.transform.localPosition = new Vector3(0f, 0f, 0f);
            selectionMarker.transform.localScale = new Vector3(0.72f, 0.015f, 0.52f);
            RuntimeObjectLifecycle.Destroy(selectionMarker.GetComponent<Collider>());
            selectionMarker.GetComponent<MeshRenderer>().sharedMaterial = EraMaterialFactory.CreateUnlit(new Color(0.95f, 0.57f, 0.10f, 0.9f));
            selectionMarker.SetActive(false);
        }
    }
}
