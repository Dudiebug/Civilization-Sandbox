using System;
using System.Collections.Generic;
using CivSandbox.People;
using CivSandbox.Simulation;
using UnityEngine;
using UnityEngine.UIElements;

namespace CivSandbox.UI
{
    public sealed class WorldViewerHud : MonoBehaviour
    {
        private readonly Dictionary<SimulationSpeed, Button> speedButtons = new Dictionary<SimulationSpeed, Button>();
        private IWorldViewerSession session;
        private PanelSettings panelSettings;
        private GameObject documentObject;
        private Label worldTimeLabel;
        private Label companyLabel;
        private Label speedLabel;
        private Label clockLabel;
        private Label inspectorTitle;
        private Label inspectorBody;
        private TextField seedField;

        public void Initialize(IWorldViewerSession worldViewerSession)
        {
            session = worldViewerSession ?? throw new ArgumentNullException(nameof(worldViewerSession));
            CreateRuntimeDocument();
            Refresh();
        }

        private void Update()
        {
            if (session != null && worldTimeLabel != null)
            {
                Refresh();
            }
        }

        private void OnDestroy()
        {
            if (panelSettings != null)
            {
                Destroy(panelSettings);
            }
        }

        private void CreateRuntimeDocument()
        {
            ThemeStyleSheet runtimeTheme = Resources.Load<ThemeStyleSheet>("WorldViewerRuntimeTheme");
            if (runtimeTheme == null)
            {
                throw new InvalidOperationException(
                    "World Viewer runtime theme is missing. Expected Resources/WorldViewerRuntimeTheme.tss.");
            }

            panelSettings = ScriptableObject.CreateInstance<PanelSettings>();
            panelSettings.name = "World Viewer runtime panel settings";
            panelSettings.themeStyleSheet = runtimeTheme;

            documentObject = new GameObject("Visible runtime HUD document");
            documentObject.SetActive(false);
            documentObject.transform.SetParent(transform, false);
            UIDocument document = documentObject.AddComponent<UIDocument>();
            document.panelSettings = panelSettings;
            document.sortingOrder = 1000;
            documentObject.SetActive(true);

            VisualElement root = document.rootVisualElement;
            root.name = "WorldViewerHudRoot";
            root.pickingMode = PickingMode.Ignore;
            root.style.position = Position.Absolute;
            root.style.left = 0f;
            root.style.right = 0f;
            root.style.top = 0f;
            root.style.bottom = 0f;

            root.Add(CreateMainPanel());
            root.Add(CreateInspectorPanel());
        }

        private VisualElement CreateMainPanel()
        {
            VisualElement panel = CreatePanel(18f, null, 360f);
            panel.name = "WorldViewerMainPanel";
            AddLabel(panel, "CIVILIZATION SANDBOX", 22, new Color(0.96f, 0.86f, 0.58f), true);
            AddLabel(panel, "FOUNDING CAMP  •  SURVIVAL SLICE 1", 12, new Color(0.76f, 0.70f, 0.52f), true);
            AddSpacer(panel, 10f);

            worldTimeLabel = AddLabel(panel, string.Empty, 14, BodyColor());
            companyLabel = AddLabel(panel, string.Empty, 14, BodyColor());
            speedLabel = AddLabel(panel, string.Empty, 14, BodyColor());
            clockLabel = AddLabel(panel, string.Empty, 12, new Color(0.76f, 0.70f, 0.52f), true);
            AddSpacer(panel, 8f);

            VisualElement speedRow = new VisualElement();
            speedRow.style.flexDirection = FlexDirection.Row;
            speedRow.style.marginBottom = 8f;
            panel.Add(speedRow);
            AddSpeedButton(speedRow, "Pause", SimulationSpeed.Paused);
            AddSpeedButton(speedRow, "1x", SimulationSpeed.Normal);
            AddSpeedButton(speedRow, "2x", SimulationSpeed.Double);
            AddSpeedButton(speedRow, "5x", SimulationSpeed.Fast);
            AddSpeedButton(speedRow, "10x", SimulationSpeed.VeryFast);

            AddLabel(panel, "Deterministic world seed", 13, BodyColor());
            seedField = new TextField { value = session.Seed.ToString(), isDelayed = true };
            seedField.style.height = 28f;
            seedField.style.marginBottom = 6f;
            seedField.style.color = BodyColor();
            seedField.style.backgroundColor = new Color(0.10f, 0.075f, 0.05f, 1f);
            panel.Add(seedField);

            Button reset = CreateButton("Reset this company", () =>
            {
                if (ulong.TryParse(seedField.value, out ulong parsedSeed))
                {
                    session.Reset(parsedSeed);
                }
                else
                {
                    seedField.value = session.Seed.ToString();
                }
            });
            reset.style.height = 31f;
            panel.Add(reset);

            AddSpacer(panel, 8f);
            AddLabel(panel, "Pan  WASD / arrows / middle-drag", 12, new Color(0.76f, 0.70f, 0.52f), true);
            AddLabel(panel, "Zoom  mouse wheel", 12, new Color(0.76f, 0.70f, 0.52f), true);
            return panel;
        }

        private VisualElement CreateInspectorPanel()
        {
            VisualElement panel = CreatePanel(null, 18f, 360f);
            panel.name = "WorldViewerInspectorPanel";
            AddLabel(panel, "OMNISCIENT INSPECTION", 12, new Color(0.76f, 0.70f, 0.52f), true);
            AddSpacer(panel, 8f);
            inspectorTitle = AddLabel(panel, "Select any person", 20, new Color(0.96f, 0.86f, 0.58f), true);
            inspectorBody = AddLabel(panel, string.Empty, 14, BodyColor());
            inspectorBody.style.whiteSpace = WhiteSpace.Normal;
            inspectorBody.style.minHeight = 170f;
            AddSpacer(panel, 8f);
            AddLabel(panel, "READ-ONLY  •  AUTHORITATIVE SNAPSHOT", 11, new Color(0.76f, 0.70f, 0.52f), true);
            return panel;
        }

        private void Refresh()
        {
            WorldSnapshot snapshot = session.Snapshot;
            worldTimeLabel.text = $"World time   {FormatTime(snapshot.Time.Seconds)}";
            companyLabel.text = $"Company      {snapshot.Count} named people";
            speedLabel.text = $"State        {session.Speed.Label()}";
            clockLabel.text = FormatClockHealth();

            foreach (KeyValuePair<SimulationSpeed, Button> pair in speedButtons)
            {
                pair.Value.style.backgroundColor = pair.Key == session.Speed
                    ? new Color(0.62f, 0.43f, 0.16f, 1f)
                    : new Color(0.25f, 0.18f, 0.10f, 1f);
            }

            if (!session.SelectedPersonId.HasValue || !TryFindPerson(snapshot, session.SelectedPersonId.Value, out PersonSnapshot person))
            {
                inspectorTitle.text = "Select any person";
                inspectorBody.text = "Click a person to inspect survival state.\n\n" +
                    "Nutrition and hydration decline deterministically with world time. " +
                    "Resources and survival decisions enter in the next slice.";
                return;
            }

            inspectorTitle.text = person.Name;
            inspectorBody.text =
                $"Stable ID       {person.Id}\n" +
                $"Position        {person.Position}\n" +
                $"Current action  {person.Action}\n" +
                $"Why             {FormatReason(person.ActionReason)}\n\n" +
                $"Nutrition       {person.Needs.NutritionPercent}%  •  {person.Needs.NutritionUrgency}\n" +
                $"Hydration       {person.Needs.HydrationPercent}%  •  {person.Needs.HydrationUrgency}\n" +
                $"Need pressure   {person.Needs.HighestUrgency}\n" +
                $"World time      {FormatTime(snapshot.Time.Seconds)}";
        }

        private void AddSpeedButton(VisualElement row, string label, SimulationSpeed speed)
        {
            Button button = CreateButton(label, () => session.SetSpeed(speed));
            button.style.flexGrow = 1f;
            button.style.marginRight = 3f;
            button.style.height = 29f;
            row.Add(button);
            speedButtons.Add(speed, button);
        }

        private static Button CreateButton(string text, Action clicked)
        {
            var button = new Button(clicked) { text = text };
            button.style.color = new Color(0.96f, 0.86f, 0.64f);
            button.style.backgroundColor = new Color(0.25f, 0.18f, 0.10f, 1f);
            button.style.borderTopColor = new Color(0.50f, 0.38f, 0.20f);
            button.style.borderRightColor = new Color(0.50f, 0.38f, 0.20f);
            button.style.borderBottomColor = new Color(0.50f, 0.38f, 0.20f);
            button.style.borderLeftColor = new Color(0.50f, 0.38f, 0.20f);
            button.style.unityFontStyleAndWeight = FontStyle.Bold;
            return button;
        }

        private static VisualElement CreatePanel(float? left, float? right, float width)
        {
            var panel = new VisualElement { pickingMode = PickingMode.Position };
            panel.style.position = Position.Absolute;
            panel.style.top = 18f;
            if (left.HasValue) panel.style.left = left.Value;
            if (right.HasValue) panel.style.right = right.Value;
            panel.style.width = width;
            panel.style.paddingTop = 16f;
            panel.style.paddingRight = 18f;
            panel.style.paddingBottom = 16f;
            panel.style.paddingLeft = 18f;
            panel.style.backgroundColor = new Color(0.17f, 0.12f, 0.075f, 0.97f);
            panel.style.borderTopWidth = 2f;
            panel.style.borderRightWidth = 2f;
            panel.style.borderBottomWidth = 2f;
            panel.style.borderLeftWidth = 2f;
            Color border = new Color(0.54f, 0.40f, 0.20f, 1f);
            panel.style.borderTopColor = border;
            panel.style.borderRightColor = border;
            panel.style.borderBottomColor = border;
            panel.style.borderLeftColor = border;
            return panel;
        }

        private static Label AddLabel(VisualElement parent, string text, int fontSize, Color color, bool bold = false)
        {
            var label = new Label(text);
            label.style.fontSize = fontSize;
            label.style.color = color;
            label.style.marginBottom = 3f;
            if (bold) label.style.unityFontStyleAndWeight = FontStyle.Bold;
            parent.Add(label);
            return label;
        }

        private static void AddSpacer(VisualElement parent, float height)
        {
            var spacer = new VisualElement();
            spacer.style.height = height;
            parent.Add(spacer);
        }

        private static Color BodyColor() => new Color(0.93f, 0.88f, 0.72f);

        private static bool TryFindPerson(WorldSnapshot snapshot, StableEntityId id, out PersonSnapshot person)
        {
            for (int index = 0; index < snapshot.Count; index++)
            {
                if (snapshot[index].Id == id)
                {
                    person = snapshot[index];
                    return true;
                }
            }

            person = default;
            return false;
        }

        private string FormatClockHealth()
        {
            if (session.IsClockOverloaded && session.TotalDroppedWallTime > TimeSpan.Zero)
            {
                return $"CLOCK OVERLOAD  •  {session.TotalDroppedWallTime.TotalMilliseconds:0} ms wall time dropped";
            }

            if (session.IsClockOverloaded) return "CLOCK CATCHING UP  •  bounded backlog";
            return session.TotalDroppedWallTime > TimeSpan.Zero
                ? $"CLOCK STEADY  •  {session.TotalDroppedWallTime.TotalMilliseconds:0} ms wall time skipped"
                : "CLOCK STEADY  •  fixed 20 Hz";
        }

        private static string FormatReason(PersonActionReason reason)
        {
            switch (reason)
            {
                case PersonActionReason.ExploringBoundedArea: return "Exploring the bounded camp area";
                case PersonActionReason.WaterUnavailable: return "Thirst is urgent; no water source exists yet";
                case PersonActionReason.FoodUnavailable: return "Hunger is urgent; no food source exists yet";
                default: return "Resting briefly between walks";
            }
        }

        private static string FormatTime(long totalSeconds)
        {
            long day = totalSeconds / 86400L + 1L;
            long inDay = totalSeconds % 86400L;
            long hour = inDay / 3600L;
            long minute = inDay % 3600L / 60L;
            long second = inDay % 60L;
            return $"Day {day}, {hour:00}:{minute:00}:{second:00}";
        }
    }
}
