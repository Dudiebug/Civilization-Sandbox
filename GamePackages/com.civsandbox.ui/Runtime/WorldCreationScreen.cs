using System;
using System.Collections.Generic;
using CivSandbox.World;
using UnityEngine;
using UnityEngine.UIElements;

namespace CivSandbox.UI
{
    public sealed class WorldCreationScreen : MonoBehaviour
    {
        private readonly Dictionary<WorldSizePreset, Button> sizeButtons = new Dictionary<WorldSizePreset, Button>();
        private Action<WorldGenerationSettings> generationRequested;
        private Action<GeneratedWorld, GeneratedWorldCell> foundingRequested;
        private Action backRequested;
        private Action cameraViewRequested;
        private PanelSettings panelSettings;
        private GameObject documentObject;
        private VisualElement root;
        private VisualElement titleScreen;
        private VisualElement setupScreen;
        private VisualElement generatedInfo;
        private TextField seedField;
        private SliderInt landSlider;
        private SliderInt temperatureSlider;
        private SliderInt rainfallSlider;
        private SliderInt mountainSlider;
        private SliderInt forestSlider;
        private SliderInt resourceSlider;
        private Label validationLabel;
        private Label generationSummary;
        private Label selectionSummary;
        private Button foundButton;
        private WorldSizePreset selectedSize = WorldSizePreset.Standard;
        private GeneratedWorld generatedWorld;
        private GeneratedWorldCell? selectedCell;

        public void Initialize(
            Action<WorldGenerationSettings> onGenerationRequested,
            Action<GeneratedWorld, GeneratedWorldCell> onFoundingRequested,
            Action onBackRequested,
            Action onCameraViewRequested = null)
        {
            generationRequested = onGenerationRequested ?? throw new ArgumentNullException(nameof(onGenerationRequested));
            foundingRequested = onFoundingRequested ?? throw new ArgumentNullException(nameof(onFoundingRequested));
            backRequested = onBackRequested ?? throw new ArgumentNullException(nameof(onBackRequested));
            cameraViewRequested = onCameraViewRequested;
            CreateRuntimeDocument();
            ShowTitle();
        }

        public ulong CurrentSeed => ulong.TryParse(seedField?.value, out ulong seed) ? seed : 0UL;

        public void RandomizeSeed()
        {
            ulong previous = CurrentSeed;
            byte[] bytes = Guid.NewGuid().ToByteArray();
            ulong candidate = BitConverter.ToUInt64(bytes, 0);
            if (candidate == previous)
            {
                candidate++;
            }

            seedField.value = candidate.ToString();
            validationLabel.text = string.Empty;
        }

        public bool CanPickWorld(Vector3 screenPoint)
        {
            return generatedWorld != null &&
                   setupScreen.style.display == DisplayStyle.Flex &&
                   screenPoint.x > 420f &&
                   screenPoint.y > 124f;
        }

        public void ShowGeneratedWorld(GeneratedWorld world)
        {
            generatedWorld = world ?? throw new ArgumentNullException(nameof(world));
            selectedCell = null;
            root.style.backgroundColor = Color.clear;
            generatedInfo.style.display = DisplayStyle.Flex;
            generationSummary.text =
                $"{world.Settings.Size.Definition().Label} world  •  {world.Columns} × {world.Rows} person-scale tiles\n" +
                $"World {world.WorldId:x8}  •  checksum {world.Checksum:x8}";
            selectionSummary.text = "Choose an inland tile with enough walkable ground for the founding company.";
            foundButton.SetEnabled(false);
            validationLabel.text = string.Empty;
        }

        public void SelectCell(GeneratedWorldCell cell)
        {
            if (generatedWorld == null || !generatedWorld.IsFoundingSite(cell))
            {
                return;
            }

            selectedCell = cell;
            selectionSummary.text =
                $"FOUNDING TILE  {cell.Column + 1}, {cell.Row + 1}\n" +
                $"{cell.Biome}  •  fertility {Percent(cell.FertilityPermille)}%  •  resources {Percent(cell.ResourcePermille)}%\n" +
                $"Water {Percent(cell.Resources.FreshWaterPermille)}%  •  food {Percent(Math.Max(cell.Resources.StapleFoodPermille, cell.Resources.ProteinFoodPermille))}%  •  timber {Percent(cell.Resources.TimberPermille)}%  •  stone {Percent(cell.Resources.StonePermille)}%\n" +
                $"Elevation {cell.ElevationPermille}  •  rainfall {Percent(cell.MoisturePermille)}%  •  warmth {Percent(cell.TemperaturePermille)}%";
            foundButton.SetEnabled(true);
        }

        private void OnDestroy()
        {
            if (panelSettings != null)
            {
                if (Application.isPlaying) Destroy(panelSettings);
                else DestroyImmediate(panelSettings);
            }
        }

        private void CreateRuntimeDocument()
        {
            ThemeStyleSheet runtimeTheme = Resources.Load<ThemeStyleSheet>("WorldViewerRuntimeTheme");
            if (runtimeTheme == null)
            {
                throw new InvalidOperationException("World creation requires Resources/WorldViewerRuntimeTheme.tss.");
            }

            panelSettings = ScriptableObject.CreateInstance<PanelSettings>();
            panelSettings.name = "World creation runtime panel settings";
            panelSettings.themeStyleSheet = runtimeTheme;

            documentObject = new GameObject("World creation UI document");
            documentObject.SetActive(false);
            documentObject.transform.SetParent(transform, false);
            UIDocument document = documentObject.AddComponent<UIDocument>();
            document.panelSettings = panelSettings;
            document.sortingOrder = 1100;
            documentObject.SetActive(true);

            root = document.rootVisualElement;
            root.name = "WorldCreationRoot";
            root.pickingMode = PickingMode.Ignore;
            root.style.position = Position.Absolute;
            root.style.left = 0f;
            root.style.right = 0f;
            root.style.top = 0f;
            root.style.bottom = 0f;

            StyleSheet runtimeStyles = Resources.Load<StyleSheet>("WorldViewerRuntimeStyles");
            if (runtimeStyles == null)
            {
                throw new InvalidOperationException("World creation requires Resources/WorldViewerRuntimeStyles.uss.");
            }

            root.styleSheets.Add(runtimeStyles);

            titleScreen = CreateTitleScreen();
            setupScreen = CreateSetupScreen();
            generatedInfo = CreateGeneratedInfo();
            root.Add(titleScreen);
            root.Add(setupScreen);
            root.Add(generatedInfo);
        }

        private VisualElement CreateTitleScreen()
        {
            var screen = new VisualElement { pickingMode = PickingMode.Position };
            Fill(screen);
            screen.style.alignItems = Align.Center;
            screen.style.justifyContent = Justify.Center;

            VisualElement panel = CreatePanel(600f);
            panel.style.alignItems = Align.Center;
            panel.style.paddingTop = 42f;
            panel.style.paddingBottom = 42f;
            AddLabel(panel, "CIVILIZATION SANDBOX", 36, new Color(0.96f, 0.84f, 0.53f), true);
            AddLabel(panel, "FOUNDING WORLDS", 14, new Color(0.72f, 0.66f, 0.47f), true);
            AddSpacer(panel, 22f);
            Label promise = AddLabel(
                panel,
                "Create an early-modern world and watch history begin\nat a seeded starting place within the living map.",
                16,
                BodyColor());
            promise.style.unityTextAlign = TextAnchor.MiddleCenter;
            AddSpacer(panel, 26f);
            Button newGame = CreateButton("NEW GAME", ShowSetup);
            newGame.style.width = 260f;
            newGame.style.height = 46f;
            panel.Add(newGame);
            AddSpacer(panel, 12f);
            AddLabel(panel, "Build 02  •  deterministic world-generation prototype", 12, MutedColor(), true);
            screen.Add(panel);
            return screen;
        }

        private VisualElement CreateSetupScreen()
        {
            var screen = new VisualElement { pickingMode = PickingMode.Ignore };
            Fill(screen);
            screen.style.display = DisplayStyle.None;

            var panel = new ScrollView { pickingMode = PickingMode.Position };
            panel.name = "WorldSetupPanel";
            panel.style.position = Position.Absolute;
            panel.style.left = 16f;
            panel.style.top = 16f;
            panel.style.bottom = 16f;
            panel.style.width = 390f;
            ApplyPanelStyle(panel);

            AddLabel(panel, "CREATE A WORLD", 24, AccentColor(), true);
            AddLabel(panel, "EARLY-MODERN STARTING CONDITIONS", 11, MutedColor(), true);
            AddSpacer(panel, 14f);
            AddLabel(panel, "World size", 13, BodyColor(), true);
            VisualElement sizeRow = new VisualElement();
            sizeRow.style.flexDirection = FlexDirection.Row;
            panel.Add(sizeRow);
            AddSizeButton(sizeRow, WorldSizePreset.Small);
            AddSizeButton(sizeRow, WorldSizePreset.Standard);
            AddSizeButton(sizeRow, WorldSizePreset.Large);
            RefreshSizeButtons();

            AddSpacer(panel, 10f);
            AddLabel(panel, "World seed", 13, BodyColor(), true);
            seedField = new TextField { value = WorldGenerationSettings.Default.Seed.Value.ToString(), isDelayed = true };
            seedField.name = "WorldSeedField";
            seedField.style.height = 36f;
            seedField.style.flexGrow = 1f;
            seedField.style.color = BodyColor();
            seedField.style.backgroundColor = new Color(0.08f, 0.065f, 0.045f, 1f);
            var seedRow = new VisualElement();
            seedRow.style.flexDirection = FlexDirection.Row;
            seedRow.Add(seedField);
            Button randomizeSeed = CreateButton("RANDOMIZE", RandomizeSeed);
            randomizeSeed.name = "RandomizeSeedButton";
            randomizeSeed.style.width = 112f;
            randomizeSeed.style.height = 36f;
            randomizeSeed.style.marginLeft = 6f;
            seedRow.Add(randomizeSeed);
            panel.Add(seedRow);

            WorldGenerationSettings defaults = WorldGenerationSettings.Default;
            landSlider = AddSlider(panel, "Land mass", defaults.LandPercent);
            temperatureSlider = AddSlider(panel, "Temperature", defaults.TemperaturePercent);
            rainfallSlider = AddSlider(panel, "Rainfall", defaults.RainfallPercent);
            mountainSlider = AddSlider(panel, "Mountains", defaults.MountainPercent);
            forestSlider = AddSlider(panel, "Forests", defaults.ForestPercent);
            resourceSlider = AddSlider(panel, "Resource abundance", defaults.ResourcePercent);

            validationLabel = AddLabel(panel, string.Empty, 12, new Color(0.93f, 0.49f, 0.34f), true);
            Button generate = CreateButton("GENERATE WORLD", RequestGeneration);
            generate.style.height = 40f;
            panel.Add(generate);
            AddSpacer(panel, 8f);
            Button back = CreateButton("Back to title", ReturnToTitle);
            back.style.height = 30f;
            panel.Add(back);
            AddSpacer(panel, 10f);
            AddLabel(panel, "All controls are deterministic. The same generation version, seed, and settings recreate the same semantic world.", 11, MutedColor());
            screen.Add(panel);
            return screen;
        }

        private VisualElement CreateGeneratedInfo()
        {
            VisualElement panel = CreatePanel(430f);
            panel.name = "FoundingSelectionPanel";
            panel.style.position = Position.Absolute;
            panel.style.right = 16f;
            panel.style.bottom = 16f;
            panel.style.display = DisplayStyle.None;
            generationSummary = AddLabel(panel, string.Empty, 12, MutedColor(), true);
            AddSpacer(panel, 5f);
            selectionSummary = AddLabel(panel, string.Empty, 13, BodyColor());
            selectionSummary.style.whiteSpace = WhiteSpace.Normal;
            AddSpacer(panel, 7f);
            Button cameraView = CreateButton("TOGGLE TOP-DOWN / ANGLED VIEW", () => cameraViewRequested?.Invoke());
            cameraView.name = "CameraViewButton";
            cameraView.style.height = 32f;
            cameraView.SetEnabled(cameraViewRequested != null);
            panel.Add(cameraView);
            AddLabel(panel, "Pan: WASD / arrows / middle-drag  •  Zoom: wheel  •  Rotate: Q / E  •  View: V", 11, MutedColor());
            AddSpacer(panel, 7f);
            foundButton = CreateButton("FOUND THE FIRST CAMP", RequestFounding);
            foundButton.style.height = 38f;
            foundButton.SetEnabled(false);
            panel.Add(foundButton);
            return panel;
        }

        private void ShowTitle()
        {
            generatedWorld = null;
            selectedCell = null;
            titleScreen.style.display = DisplayStyle.Flex;
            setupScreen.style.display = DisplayStyle.None;
            generatedInfo.style.display = DisplayStyle.None;
            root.style.backgroundColor = new Color(0.075f, 0.11f, 0.13f, 1f);
        }

        private void ShowSetup()
        {
            titleScreen.style.display = DisplayStyle.None;
            setupScreen.style.display = DisplayStyle.Flex;
            root.style.backgroundColor = new Color(0.075f, 0.11f, 0.13f, 1f);
        }

        private void ReturnToTitle()
        {
            backRequested();
            ShowTitle();
        }

        private void RequestGeneration()
        {
            if (!ulong.TryParse(seedField.value, out ulong seed))
            {
                validationLabel.text = "Enter a whole-number seed.";
                return;
            }

            validationLabel.text = string.Empty;
            var settings = new WorldGenerationSettings(
                seed,
                selectedSize,
                landSlider.value,
                temperatureSlider.value,
                rainfallSlider.value,
                mountainSlider.value,
                forestSlider.value,
                resourceSlider.value);
            generationRequested(settings);
        }

        private void RequestFounding()
        {
            if (generatedWorld != null && selectedCell.HasValue)
            {
                foundingRequested(generatedWorld, selectedCell.Value);
            }
        }

        private void AddSizeButton(VisualElement row, WorldSizePreset size)
        {
            WorldSizeDefinition definition = size.Definition();
            Button button = CreateButton($"{definition.Label}\n{definition.Columns}×{definition.Rows}", () =>
            {
                selectedSize = size;
                RefreshSizeButtons();
            });
            button.style.flexGrow = 1f;
            button.style.height = 47f;
            button.style.marginRight = 3f;
            row.Add(button);
            sizeButtons.Add(size, button);
        }

        private void RefreshSizeButtons()
        {
            foreach (KeyValuePair<WorldSizePreset, Button> pair in sizeButtons)
            {
                pair.Value.style.backgroundColor = pair.Key == selectedSize
                    ? new Color(0.57f, 0.38f, 0.13f, 1f)
                    : new Color(0.22f, 0.15f, 0.085f, 1f);
            }
        }

        private static SliderInt AddSlider(VisualElement parent, string label, int value)
        {
            var slider = new SliderInt(label, 0, 100)
            {
                value = value,
                showInputField = true
            };
            slider.style.height = 34f;
            slider.style.color = BodyColor();
            slider.style.marginTop = 3f;
            parent.Add(slider);
            return slider;
        }

        private static VisualElement CreatePanel(float width)
        {
            var panel = new VisualElement { pickingMode = PickingMode.Position };
            panel.style.width = width;
            ApplyPanelStyle(panel);
            return panel;
        }

        private static void ApplyPanelStyle(VisualElement panel)
        {
            panel.style.paddingTop = 18f;
            panel.style.paddingRight = 18f;
            panel.style.paddingBottom = 18f;
            panel.style.paddingLeft = 18f;
            panel.style.backgroundColor = new Color(0.14f, 0.095f, 0.055f, 0.97f);
            panel.style.borderTopWidth = 2f;
            panel.style.borderRightWidth = 2f;
            panel.style.borderBottomWidth = 2f;
            panel.style.borderLeftWidth = 2f;
            Color border = new Color(0.51f, 0.36f, 0.17f, 1f);
            panel.style.borderTopColor = border;
            panel.style.borderRightColor = border;
            panel.style.borderBottomColor = border;
            panel.style.borderLeftColor = border;
        }

        private static Button CreateButton(string text, Action clicked)
        {
            var button = new Button(clicked) { text = text };
            button.style.color = new Color(0.96f, 0.85f, 0.61f);
            button.style.backgroundColor = new Color(0.22f, 0.15f, 0.085f, 1f);
            button.style.unityFontStyleAndWeight = FontStyle.Bold;
            return button;
        }

        private static Label AddLabel(VisualElement parent, string text, int fontSize, Color color, bool bold = false)
        {
            var label = new Label(text);
            label.style.fontSize = fontSize;
            label.style.color = color;
            label.style.whiteSpace = WhiteSpace.Normal;
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

        private static void Fill(VisualElement element)
        {
            element.style.position = Position.Absolute;
            element.style.left = 0f;
            element.style.right = 0f;
            element.style.top = 0f;
            element.style.bottom = 0f;
        }

        private static int Percent(int permille) => (permille + 5) / 10;

        private static Color AccentColor() => new Color(0.96f, 0.84f, 0.53f);

        private static Color BodyColor() => new Color(0.92f, 0.87f, 0.72f);

        private static Color MutedColor() => new Color(0.72f, 0.66f, 0.47f);
    }
}
