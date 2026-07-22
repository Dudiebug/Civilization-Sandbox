using System;
using System.IO;
using UnityEditor;
using UnityEditor.Build.Reporting;
using UnityEngine;

namespace CivSandbox.WorldViewer.Editor
{
    public static class WorldViewerBuild
    {
        public static void Run()
        {
            try
            {
                WorldViewerProjectSetup.Configure();
                string projectRoot = Directory.GetParent(Application.dataPath)?.FullName ?? throw new InvalidOperationException("Project root was not found.");
                string output = Path.Combine(projectRoot, "Artifacts", "build", "world-viewer", "CivilizationSandboxWorldViewer.exe");
                Directory.CreateDirectory(Path.GetDirectoryName(output) ?? throw new InvalidOperationException("Build output directory was not found."));
                var options = new BuildPlayerOptions
                {
                    scenes = new[] { WorldViewerProjectSetup.ScenePath },
                    locationPathName = output,
                    target = BuildTarget.StandaloneWindows64,
                    options = BuildOptions.None
                };

                BuildReport report = BuildPipeline.BuildPlayer(options);
                if (report.summary.result != BuildResult.Succeeded)
                {
                    throw new InvalidOperationException($"World Viewer build failed with {report.summary.totalErrors} errors.");
                }

                Debug.Log($"CIV-BUILD02-BUILD-000: Founding Worlds player built at {output}.");
            }
            catch (Exception exception)
            {
                Debug.LogException(exception);
                EditorApplication.Exit(1);
            }
        }
    }
}
