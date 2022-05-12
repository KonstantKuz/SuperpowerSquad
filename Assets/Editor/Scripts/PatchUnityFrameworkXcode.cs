using UnityEditor;
using UnityEditor.Callbacks;
using UnityEditor.iOS.Xcode;
using UnityEngine;

#if UNITY_EDITOR
namespace Editor
{
    public class PatchUnityFrameworkXcode
    {
        [PostProcessBuild]
        public static void OnPostprocessBuild(BuildTarget target, string pathToBuiltProject)
        {
#if UNITY_IOS            
            SetUnityFrameworkCodeSignToManual(pathToBuiltProject);
#endif
        }

        private static void SetUnityFrameworkCodeSignToManual(string pathToBuiltProject)
        {
            Debug.Log("Patching xcode project to set manual signing");
            var projectPath = PBXProject.GetPBXProjectPath(pathToBuiltProject);
            var project = new PBXProject();
            project.ReadFromFile(projectPath);

            FixCodeSign(project);
            FixFirebase(project);

            project.WriteToFile(projectPath);
            Debug.Log("Xcode project is patched");
        }

        private static void FixCodeSign(PBXProject project)
        {
            var unityFrameworkTarget = project.GetUnityFrameworkTargetGuid();
            project.SetBuildProperty(unityFrameworkTarget, "CODE_SIGN_STYLE", "Manual");
            project.SetBuildProperty(unityFrameworkTarget, "CODE_SIGNING_REQUIRED", "NO");
            project.SetBuildProperty(unityFrameworkTarget, "CODE_SIGNING_ALLOWED", "NO");
            project.SetBuildProperty(unityFrameworkTarget, "PROVISIONING_PROFILE", "");
            project.SetBuildProperty(unityFrameworkTarget, "EXPANDED_CODE_SIGN_IDENTITY", "");
            project.SetBuildProperty(unityFrameworkTarget, "PROVISIONING_PROFILE_SPECIFIER", "");

            var target = project.GetUnityMainTargetGuid();
            project.SetBuildProperty(target, "CODE_SIGN_STYLE", "Manual");
        }

        private static void FixFirebase(PBXProject project)
        {
            var target = project.TargetGuidByName("Unity-iPhone");
            project.SetBuildProperty(target, "OTHER_LDFLAGS", "$(inherited)");
            project.AddBuildProperty(target, "LIBRARY_SEARCH_PATHS", "$(inherited)");
        }
    }
}
#endif