using System.IO;
using UnityEditor;
using UnityEditor.PackageManager;
using UnityEngine;

namespace YsoCorp
{
    namespace GameUtils
    {
#if UNITY_EDITOR
        [CustomEditor(typeof(YCConfig))]
        public class YCConfigEditor : UnityEditor.Editor
        {
            public override void OnInspectorGUI()
            {
                this.DrawDefaultInspector();
                GUILayout.Space(10);
                YCConfig myTarget = (YCConfig)this.target;
                if (GUILayout.Button("Import Config"))
                {
                    myTarget.EditorImportConfig();
                    EditorUtility.SetDirty(myTarget);
                }

                GUILayout.Space(10);

#if IN_APP_PURCHASING
                if (GUILayout.Button("Deactivate In App Purchases")) {
                    myTarget.RemoveDefineSymbolsForGroup("IN_APP_PURCHASING");
                    Client.Remove("com.unity.purchasing");
                }
#else
                if (GUILayout.Button("Activate In App Purchases"))
                {
                    myTarget.AddDefineSymbolsForGroup("IN_APP_PURCHASING");
                    Client.Add("com.unity.purchasing@4.0.3");
                }
#endif

#if FIREBASE
                if (GUILayout.Button("Disactivate Firebase")) {
                    myTarget.RemoveDefineSymbolsForGroup("FIREBASE");
                }
#else
                if (GUILayout.Button("Activate Firebase"))
                {
                    if (Directory.Exists("Assets/Firebase"))
                    {
                        myTarget.AddDefineSymbolsForGroup("FIREBASE");
                    }
                    else
                    {
                        myTarget.DisplayDialog("Error",
                            "This only for validate game.\nPlease import Firebase Analytics before.", "Ok");
                    }
                }
#endif

            }
        }
#endif
    }
}