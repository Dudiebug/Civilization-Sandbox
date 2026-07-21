using System;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEditor.Build.Reporting;
using UnityEngine;

namespace CivSandbox.Tooling.Editor
{
    public static class BaselineBuild
    {
        public static void Run()
        {
            const string performanceRunInfo = "Assets/Resources/PerformanceTestRunInfo.json";
            const string performanceRunSettings = "Assets/Resources/PerformanceTestRunSettings.json";
            string projectRoot = Directory.GetParent(Application.dataPath)!.FullName;
            string resourcesPath = Path.Combine(projectRoot, "Assets", "Resources");
            string performanceRunInfoPath = Path.Combine(projectRoot, performanceRunInfo);
            string performanceRunSettingsPath = Path.Combine(projectRoot, performanceRunSettings);
            bool resourcesFolderExisted = AssetDatabase.IsValidFolder("Assets/Resources");
            bool runInfoExisted = File.Exists(performanceRunInfoPath);
            bool runSettingsExisted = File.Exists(performanceRunSettingsPath);
            try
            {
                BaselineProjectSetup.Configure();
                string targetName = ReadArgument("-civSandboxBuildTarget");
                BuildTarget target;
                string relativeOutput;
                switch (targetName)
                {
                    case "Windows":
                        target = BuildTarget.StandaloneWindows64;
                        relativeOutput = "Artifacts/build/windows/CivilizationSandboxBootstrap.exe";
                        break;
                    case "Linux":
                        target = BuildTarget.StandaloneLinux64;
                        relativeOutput = "Artifacts/build/linux/CivilizationSandboxBootstrap.x86_64";
                        break;
                    default:
                        throw new ArgumentException("CIV001-BUILD-003: -civSandboxBuildTarget must be Windows or Linux.");
                }

                string output = Path.GetFullPath(Path.Combine(Directory.GetParent(Application.dataPath)!.FullName, relativeOutput));
                Directory.CreateDirectory(Path.GetDirectoryName(output)!);
                var options = new BuildPlayerOptions
                {
                    scenes = new[] { BaselineProjectSetup.ScenePath },
                    locationPathName = output,
                    target = target,
                    options = BuildOptions.None
                };
                BuildReport report = BuildPipeline.BuildPlayer(options);
                if (report.summary.result != BuildResult.Succeeded)
                {
                    throw new InvalidOperationException($"CIV001-BUILD-004: Build result was {report.summary.result} with {report.summary.totalErrors} errors.");
                }
                Debug.Log($"CIV001-BUILD-000: {targetName} build completed in {report.summary.totalTime} at {output}.");
            }
            catch (Exception exception)
            {
                Debug.LogException(exception);
                EditorApplication.Exit(1);
            }
            finally
            {
                DeleteBuildScratchAssetIfCreated(performanceRunInfo, performanceRunInfoPath, runInfoExisted);
                DeleteBuildScratchAssetIfCreated(performanceRunSettings, performanceRunSettingsPath, runSettingsExisted);
                if (!resourcesFolderExisted && AssetDatabase.IsValidFolder("Assets/Resources") &&
                    Directory.GetFiles(resourcesPath).Length == 0 && Directory.GetDirectories(resourcesPath).Length == 0)
                {
                    AssetDatabase.DeleteAsset("Assets/Resources");
                }
            }
        }

        private static void DeleteBuildScratchAssetIfCreated(string assetPath, string fullPath, bool existedBeforeBuild)
        {
            if (!existedBeforeBuild && File.Exists(fullPath))
            {
                AssetDatabase.DeleteAsset(assetPath);
            }
        }

        private static string ReadArgument(string name)
        {
            string[] args = Environment.GetCommandLineArgs();
            int index = Array.IndexOf(args, name);
            return index >= 0 && index + 1 < args.Length ? args[index + 1] : string.Empty;
        }
    }
}
