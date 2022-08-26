//
//  MaxGoogleAdManagerInitialize.cs
//  AppLovin MAX Unity Plugin
//
//  Created by Santosh Bagadi on 10/5/20.
//  Copyright © 2020 AppLovin. All rights reserved.
//

using System.IO;
using UnityEditor;
using UnityEngine;

namespace AppLovinMax.Mediation.GoogleAdManager.Editor
{
    [InitializeOnLoad]
    public class MaxGoogleAdManagerInitialize
    {
        private static readonly string LegacyMaxMediationGoogleAdManagerDir = Path.Combine("Assets", "Plugins/Android/MaxMediationGoogleAdManager");

        static MaxGoogleAdManagerInitialize()
        {
            // Check if the MaxMediationGoogleAdManager directory exists and append .androidlib to it.
            if (Directory.Exists(LegacyMaxMediationGoogleAdManagerDir))
            {
                Debug.Log("[AppLovin MAX] Updating Google Ad Manager Android library directory name to make it compatible with Unity 2020+ versions.");

                FileUtil.MoveFileOrDirectory(LegacyMaxMediationGoogleAdManagerDir, LegacyMaxMediationGoogleAdManagerDir + ".androidlib");
                FileUtil.MoveFileOrDirectory(LegacyMaxMediationGoogleAdManagerDir + ".meta", LegacyMaxMediationGoogleAdManagerDir + ".androidlib" + ".meta");
                AssetDatabase.Refresh();
            }
        }
    }
}
