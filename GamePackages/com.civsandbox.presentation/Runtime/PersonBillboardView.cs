using CivSandbox.People;
using CivSandbox.Simulation;
using UnityEngine;

namespace CivSandbox.Presentation
{
    public sealed class PersonBillboardView : MonoBehaviour
    {
        private Camera worldCamera;
        private TextMesh nameLabel;
        private Vector3 targetPosition;
        private bool hasPosition;
        private string canonicalName;
        private GameObject selectionMarker;

        public StableEntityId Id { get; private set; }

        public void Initialize(PersonSnapshot person, Camera camera)
        {
            Id = person.Id;
            canonicalName = person.Name;
            worldCamera = camera;
            gameObject.name = $"Person - {person.Name}";

            var spriteRenderer = gameObject.AddComponent<SpriteRenderer>();
            spriteRenderer.sprite = EarlyModernSpriteFactory.Create(person.AppearanceVariant);
            spriteRenderer.sortingOrder = 10;

            var collider = gameObject.AddComponent<BoxCollider>();
            collider.center = new Vector3(0f, 0.9f, 0f);
            collider.size = new Vector3(0.9f, 1.9f, 0.35f);

            CreateShadow();
            CreateSelectionMarker();
            CreateNameLabel(person.Name);
            Apply(person, true);
        }

        public void Apply(PersonSnapshot person, bool snap)
        {
            targetPosition = new Vector3(person.Position.EastMillimeters / 1000f, 0.03f, person.Position.NorthMillimeters / 1000f);
            if (snap || !hasPosition)
            {
                transform.position = targetPosition;
                hasPosition = true;
            }

            if (nameLabel != null && nameLabel.text != person.Name)
            {
                canonicalName = person.Name;
                nameLabel.text = selectionMarker != null && selectionMarker.activeSelf ? $"> {canonicalName} <" : canonicalName;
            }
        }

        public void SetSelected(bool selected)
        {
            if (selectionMarker != null)
            {
                selectionMarker.SetActive(selected);
            }

            if (nameLabel != null)
            {
                nameLabel.text = selected ? $"> {canonicalName} <" : canonicalName;
                nameLabel.color = selected
                    ? new Color(1f, 0.73f, 0.22f, 1f)
                    : new Color(0.96f, 0.90f, 0.71f, 0.95f);
            }
        }

        private void LateUpdate()
        {
            transform.position = Vector3.Lerp(transform.position, targetPosition, 1f - Mathf.Exp(-14f * Time.unscaledDeltaTime));
            if (worldCamera == null)
            {
                return;
            }

            Vector3 towardCamera = worldCamera.transform.position - transform.position;
            towardCamera.y = 0f;
            if (towardCamera.sqrMagnitude > 0.001f)
            {
                transform.rotation = Quaternion.LookRotation(towardCamera.normalized, Vector3.up);
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

        private void CreateNameLabel(string personName)
        {
            var labelObject = new GameObject("Name");
            labelObject.transform.SetParent(transform, false);
            labelObject.transform.localPosition = new Vector3(0f, 2.05f, 0f);
            nameLabel = labelObject.AddComponent<TextMesh>();
            nameLabel.text = personName;
            nameLabel.anchor = TextAnchor.LowerCenter;
            nameLabel.alignment = TextAlignment.Center;
            nameLabel.fontSize = 32;
            nameLabel.characterSize = 0.045f;
            nameLabel.color = new Color(0.96f, 0.90f, 0.71f, 0.95f);
            nameLabel.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
            labelObject.GetComponent<MeshRenderer>().sharedMaterial = nameLabel.font.material;
        }
    }
}
