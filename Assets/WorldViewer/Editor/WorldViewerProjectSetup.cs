using System;
using System.IO;
using UnityEditor;
using UnityEditor.Build;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace CivSandbox.WorldViewer.Editor
{
    public static class WorldViewerProjectSetup
    {
        public const string ScenePath = "Assets/WorldViewer/WorldViewer.unity";

        [MenuItem("Civilization Sandbox/Build 01/Configure World Viewer")]
        public static void Configure()
        {
            PlayerSettings.companyName = "Civilization Sandbox";
            PlayerSettings.productName = "Civilization Sandbox - Founding Worlds";
            PlayerSettings.bundleVersion = "0.2.0";
            PlayerSettings.SetScriptingBackend(NamedBuildTarget.Standalone, ScriptingImplementation.Mono2x);

            EnsureScene();
            EditorBuildSettings.scenes = new[] { new EditorBuildSettingsScene(ScenePath, true) };
            AssetDatabase.SaveAssets();
            Debug.Log("CIV-BUILD02-SCENE-000: Founding Worlds scene configured.");
        }

        private static void EnsureScene()
        {
            if (AssetDatabase.LoadAssetAtPath<SceneAsset>(ScenePath) != null)
            {
                return;
            }

            string projectRoot = Directory.GetParent(Application.dataPath)?.FullName ?? throw new InvalidOperationException("Project root was not found.");
            Directory.CreateDirectory(Path.GetDirectoryName(Path.Combine(projectRoot, ScenePath)) ?? throw new InvalidOperationException("Scene parent was not found."));
            Scene scene = EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);
            if (!EditorSceneManager.SaveScene(scene, ScenePath))
            {
                throw new InvalidOperationException($"Could not save World Viewer scene at {ScenePath}.");
            }
        }
    }
}
