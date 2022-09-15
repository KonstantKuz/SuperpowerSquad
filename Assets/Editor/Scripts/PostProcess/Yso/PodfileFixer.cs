#if UNITY_IOS
using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Callbacks;
using System.IO;
using System.Linq;

namespace Editor.Scripts.PostProcess
{
    public class PodfileFixer
    {
        private const int BUILD_ORDER_CHANGE_PODFILE = 46;      //after podfile generation in external dependency manager which hase order 45

        private const string PODFILE_NAME = "Podfile";

        private const string GOOGLE_UTILITIES_POD_NAME = "  pod 'GoogleUtilities', '~> 7.7.0'";
        private const string GOOGLE_MOBILE_ADS_SDK_POD_NAME = "  pod 'Google-Mobile-Ads-SDK', '~> 9.8.0'";

        [PostProcessBuild(BUILD_ORDER_CHANGE_PODFILE)]
        public static void OnPostprocessBuild(BuildTarget target, string pathToBuiltProject)
        {
            var podfilePath = LoadPodfile(pathToBuiltProject, out var lines);
            FixAdMobLinkage(lines);
            SavePodfile(podfilePath, lines);
        }

        private static string LoadPodfile(string pathToBuiltProject, out List<string> lines)
        {
            var podfilePath = Path.Combine(pathToBuiltProject, PODFILE_NAME);
            lines = File.ReadAllLines(podfilePath, System.Text.Encoding.UTF8).ToList();
            return podfilePath;
        }

        private static void SavePodfile(string podfilePath, List<string> lines)
        {
            using var stream = new StreamWriter(podfilePath);
            stream.Write(string.Join(Environment.NewLine, lines));
            stream.Close();
        }

        private static void FixAdMobLinkage(List<string> lines)
        {
            AddLineAfter(lines,
                line => line.Contains("UnityFramework"),
                GOOGLE_UTILITIES_POD_NAME);

            AddLineAfter(lines,
                line => line.Contains("Unity-iPhone"),
                GOOGLE_MOBILE_ADS_SDK_POD_NAME +
                Environment.NewLine +
                GOOGLE_UTILITIES_POD_NAME);
        }

        private static void AddLineAfter(List<string> lines, Predicate<string> condition, string text)
        {
            var lineIdx = lines.FindIndex(condition);
            lines.Insert(lineIdx + 1, text);
        }
    }
}
#endif