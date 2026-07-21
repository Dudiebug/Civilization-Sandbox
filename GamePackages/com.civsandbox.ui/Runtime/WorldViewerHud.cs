using System;
using CivSandbox.People;
using CivSandbox.Simulation;
using UnityEngine;

namespace CivSandbox.UI
{
    public sealed class WorldViewerHud : MonoBehaviour
    {
        private IWorldViewerSession session;
        private string seedText;
        private GUIStyle panelStyle;
        private GUIStyle titleStyle;
        private GUIStyle headingStyle;
        private GUIStyle bodyStyle;
        private GUIStyle buttonStyle;
        private Texture2D parchment;

        public void Initialize(IWorldViewerSession worldViewerSession)
        {
            session = worldViewerSession ?? throw new ArgumentNullException(nameof(worldViewerSession));
            seedText = session.Seed.ToString();
        }

        private void OnGUI()
        {
            if (session == null)
            {
                return;
            }

            EnsureStyles();
            const float width = 340f;
            GUILayout.BeginArea(new Rect(18f, 18f, width, 278f), panelStyle);
            GUILayout.Label("CIVILIZATION SANDBOX", titleStyle);
            GUILayout.Label("WORLD VIEWER  •  EARLY-MODERN COMPANY", headingStyle);
            GUILayout.Space(8f);

            WorldSnapshot snapshot = session.Snapshot;
            GUILayout.Label($"World time   {FormatTime(snapshot.Time.Seconds)}", bodyStyle);
            GUILayout.Label($"Company      {snapshot.Count} named people", bodyStyle);
            GUILayout.Label($"State        {session.Speed.Label()}", bodyStyle);
            GUILayout.Space(6f);
            GUILayout.BeginHorizontal();
            DrawSpeedButton("Pause", SimulationSpeed.Paused);
            DrawSpeedButton("1x", SimulationSpeed.Normal);
            DrawSpeedButton("2x", SimulationSpeed.Double);
            DrawSpeedButton("5x", SimulationSpeed.Fast);
            DrawSpeedButton("10x", SimulationSpeed.VeryFast);
            GUILayout.EndHorizontal();
            GUILayout.Space(8f);
            GUILayout.Label("Deterministic world seed", bodyStyle);
            seedText = GUILayout.TextField(seedText, 20, GUILayout.Height(24f));
            if (GUILayout.Button("Reset this company", buttonStyle, GUILayout.Height(30f)))
            {
                if (ulong.TryParse(seedText, out ulong parsedSeed))
                {
                    session.Reset(parsedSeed);
                }
                else
                {
                    seedText = session.Seed.ToString();
                }
            }

            GUILayout.Space(6f);
            GUILayout.Label("WASD / arrows pan  •  middle-drag  •  wheel zoom", headingStyle);

            GUILayout.EndArea();
        }

        private void DrawSpeedButton(string label, SimulationSpeed speed)
        {
            Color previous = GUI.backgroundColor;
            if (session.Speed == speed)
            {
                GUI.backgroundColor = new Color(0.84f, 0.67f, 0.32f);
            }

            if (GUILayout.Button(label, buttonStyle, GUILayout.Height(27f)))
            {
                session.SetSpeed(speed);
            }

            GUI.backgroundColor = previous;
        }

        private void EnsureStyles()
        {
            if (panelStyle != null)
            {
                return;
            }

            parchment = new Texture2D(1, 1, TextureFormat.RGBA32, false)
            {
                name = "World Viewer parchment"
            };
            parchment.SetPixel(0, 0, new Color(0.17f, 0.12f, 0.075f, 0.94f));
            parchment.Apply();

            panelStyle = new GUIStyle(GUI.skin.box)
            {
                padding = new RectOffset(18, 18, 15, 15),
                normal = { background = parchment }
            };
            titleStyle = new GUIStyle(GUI.skin.label)
            {
                fontSize = 22,
                fontStyle = FontStyle.Bold,
                normal = { textColor = new Color(0.96f, 0.86f, 0.58f) }
            };
            headingStyle = new GUIStyle(GUI.skin.label)
            {
                fontSize = 12,
                fontStyle = FontStyle.Bold,
                normal = { textColor = new Color(0.73f, 0.67f, 0.49f) }
            };
            bodyStyle = new GUIStyle(GUI.skin.label)
            {
                fontSize = 14,
                normal = { textColor = new Color(0.93f, 0.88f, 0.72f) }
            };
            buttonStyle = new GUIStyle(GUI.skin.button)
            {
                fontSize = 14,
                fontStyle = FontStyle.Bold,
                normal = { textColor = new Color(0.18f, 0.12f, 0.07f) }
            };
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
