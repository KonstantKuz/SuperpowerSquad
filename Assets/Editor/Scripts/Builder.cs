using System;
using System.Linq;
using Editor.Scripts;
using UnityEditor;
using UnityEditor.Build.Reporting;
using UnityEngine;

namespace Editor
{
    public static class Builder
    {
        private static readonly string[] DefaultSceneList = { "Assets/Scenes/MainScene.unity" };
        
        [MenuItem("Build/Build Android")]        
        public static void BuildAndroid()
        {
            ApplyCommonParams();
            
            var buildAab = HasCmdLineKey("-buildAab");
            var outputFileName = GetCmdLineArgumentValue("-outputFileName") ?? "game";
            var outputPath = $"build/{outputFileName}.{(buildAab ? "aab" : "apk")}";
            
            var options = new BuildPlayerOptions
            {
                scenes = GetSceneList(),
                locationPathName = outputPath,
                target = BuildTarget.Android,
                options = BuildOptions.None
            };

            PlayerSettings.Android.keyaliasPass = PlayerSettings.Android.keystorePass = GetCmdLineArgumentValue("-keyStorePassword");
            var androidSdkRoot = GetCmdLineArgumentValue("-androidSdkRoot");
            if (androidSdkRoot != null)
            {
                EditorPrefs.SetString("AndroidSdkRoot", androidSdkRoot);
            }

            if (HasCmdLineKey("-noUnityLogo"))
            {
                PlayerSettings.SplashScreen.showUnityLogo = false;
            }
            
            EditorUserBuildSettings.buildAppBundle = buildAab;
            EditorUserBuildSettings.androidCreateSymbolsZip = buildAab;

            FileUtil.DeleteFileOrDirectory(outputPath);
            var report = BuildPipeline.BuildPlayer(options);
            Report(report.summary);
        }

        private static void ApplyCommonParams()
        {
            BuildPreprocessor.Prepare(HasCmdLineKey("-debugConsole"));
        }

        private static string[] GetSceneList() => DefaultSceneList;

        [MenuItem("Build/Build Ios")]
        public static void BuildIos()
        {
            ApplyCommonParams();        
        
            var options = new BuildPlayerOptions
            {
                scenes = GetSceneList(),
                locationPathName = "build/xcode",
                target = BuildTarget.iOS,
                options = BuildOptions.None
            };

            PlayerSettings.iOS.appleEnableAutomaticSigning = false;
            PlayerSettings.iOS.iOSManualProvisioningProfileType = HasCmdLineKey("-distribution")
                ? ProvisioningProfileType.Distribution
                : ProvisioningProfileType.Development;
            PlayerSettings.iOS.iOSManualProvisioningProfileID = GetCmdLineArgumentValue("-provisionProfileId");
            PlayerSettings.iOS.buildNumber = PlayerSettings.Android.bundleVersionCode.ToString();

            EditorUserBuildSettings.iOSBuildConfigType = iOSBuildType.Release;

            var report = BuildPipeline.BuildPlayer(options);
            Report(report.summary);
        }

        private static void Report(BuildSummary summary)
        {
            switch (summary.result)
            {
                case BuildResult.Succeeded:
                    Debug.Log("Build succeeded");
                    break;
                case BuildResult.Failed:
                case BuildResult.Cancelled:                    
                case BuildResult.Unknown:                    
                    Debug.Log("Build failed");
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            if (HasCmdLineKey("-quit"))
            {
                EditorApplication.Exit(summary.result == BuildResult.Succeeded ? 0 : 1);
            }
        }

        private static string GetCmdLineArgumentValue(string key)
        {
            var args = Environment.GetCommandLineArgs();
            for (var i = 0; i < args.Length - 1; i++)
            {
                if (args[i] == key)
                {
                    return args[i + 1];
                }
            }
            return null;
        }

        private static bool HasCmdLineKey(string key)
        {
            var args = Environment.GetCommandLineArgs();
            return args.Any(it => it == key);
        }
    }
}