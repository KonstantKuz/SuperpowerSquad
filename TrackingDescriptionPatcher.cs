using System.IO;
using JetBrains.Annotations;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEditor.iOS.Xcode;
#if UNITY_IOS
using UnityEditor.iOS.Xcode;
#endif

[PublicAPI]
public class TrackingDescriptionPatcher {
    const string TRACKING_DESCRIPTION = "Your data will be used to provide you a better and personalized ad experience.";
 
    [PostProcessBuild]
    public static void OnPostProcessBuild(BuildTarget buildTarget, string pathToXcode) {
        if (buildTarget == BuildTarget.iOS) {
            AddPListValues(pathToXcode);
        }
    }
 
    static void AddPListValues(string pathToXcode) {
        var plistPath = pathToXcode + "/Info.plist";
        var plistObj = new PlistDocument();
        plistObj.ReadFromString(File.ReadAllText(plistPath));
 
        var plistRoot = plistObj.root;
        plistRoot.SetString("NSUserTrackingUsageDescription", TRACKING_DESCRIPTION);
        File.WriteAllText(plistPath, plistObj.WriteToString());
    }
}