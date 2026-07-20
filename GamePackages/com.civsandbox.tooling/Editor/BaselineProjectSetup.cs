using System;
using System.IO;
using UnityEditor;
using UnityEditor.Build;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace CivSandbox.Tooling.Editor
{
    public static class BaselineProjectSetup
    {
        public const string ScenePath = "Assets/Bootstrap/Bootstrap.unity";

        [MenuItem("Civilization Sandbox/Configure TASK-001 Baseline")]
        public static void Configure()
        {
            VersionControlSettings.mode = "Visible Meta Files";
            EditorSettings.serializationMode = SerializationMode.ForceText;
            PlayerSettings.companyName = "Civilization Sandbox";
            PlayerSettings.productName = "Civilization Sandbox Bootstrap";
            PlayerSettings.bundleVersion = "0.1.0";
            PlayerSettings.SetScriptingBackend(NamedBuildTarget.Standalone, ScriptingImplementation.Mono2x);

            EnsureScene();
            EditorBuildSettings.scenes = new[] { new EditorBuildSettingsScene(ScenePath, true) };
            AssetDatabase.SaveAssets();
            Debug.Log("CIV001-PROJECT-000: TASK-001 baseline project settings are configured.");
        }

        private static void EnsureScene()
        {
            if (AssetDatabase.LoadAssetAtPath<SceneAsset>(ScenePath) != null)
            {
                return;
            }

            string projectRoot = Directory.GetParent(Application.dataPath)!.FullName;
            Directory.CreateDirectory(Path.GetDirectoryName(Path.Combine(projectRoot, ScenePath)) ?? throw new InvalidOperationException("Scene path has no parent."));
            Scene scene = EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);
            var exitObject = new GameObject("Bootstrap Player Exit");
            exitObject.AddComponent<BootstrapPlayerExit>();
            if (!EditorSceneManager.SaveScene(scene, ScenePath))
            {
                throw new InvalidOperationException($"Could not save bootstrap scene at {ScenePath}.");
            }
        }
    }
}
