//
//  PostProcessor.cs
//  AppLovin MAX Unity Plugin
//
//  Created by Santosh Bagadi on 12/7/19.
//  Copyright © 2019 AppLovin. All rights reserved.
//

#if UNITY_IPHONE || UNITY_IOS

using System.IO;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEditor.iOS.Xcode;

namespace AppLovinMax.Mediation.GoogleAdManager.Editor
{
    public class PostProcessor
    {
        [PostProcessBuild]
        public static void OnPostProcessBuild(BuildTarget buildTarget, string buildPath)
        {
            var plistPath = Path.Combine(buildPath, "Info.plist");
            var plist = new PlistDocument();
            plist.ReadFromFile(plistPath);

            // Set that Google Ad Manager app is enabled to Info.plist.
            plist.root.SetBoolean("GADIsAdManagerApp", true);

            // Write the file with the updated settings.
            File.WriteAllText(plistPath, plist.WriteToString());
        }
    }
}

#endif
