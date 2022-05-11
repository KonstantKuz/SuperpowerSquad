using Facebook.Unity.Editor.iOS.Xcode;
using UnityEditor;
using UnityEditor.Callbacks;

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
            var projectPath = PBXProject.GetPBXProjectPath(pathToBuiltProject);
            var project = new PBXProject();
            project.ReadFromFile(projectPath);

            FixCodeSign(project);
            FixFirebase(project);

            project.WriteToFile(projectPath);
        }

        private static void FixCodeSign(PBXProject project)
        {
            var unityFrameworkTarget = project.TargetGuidByName("UnityFramework");
            project.SetBuildProperty(unityFrameworkTarget, "CODE_SIGN_STYLE", "Manual");
            project.SetBuildProperty(unityFrameworkTarget, "CODE_SIGNING_REQUIRED", "NO");
            project.SetBuildProperty(unityFrameworkTarget, "CODE_SIGNING_ALLOWED", "NO");
            project.SetBuildProperty(unityFrameworkTarget, "PROVISIONING_PROFILE", "");
            project.SetBuildProperty(unityFrameworkTarget, "EXPANDED_CODE_SIGN_IDENTITY", "");
            project.SetBuildProperty(unityFrameworkTarget, "PROVISIONING_PROFILE_SPECIFIER", "");

            var target = project.TargetGuidByName("Unity-iPhone");
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