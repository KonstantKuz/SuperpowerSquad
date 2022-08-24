using UnityEditor;
using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

namespace YsoCorp {
    namespace GameUtils {

        public class YCDebugWindow : EditorWindow {

            // ----------------------------------- Base -----------------------------------
            private static YCConfig YCCONFIGDATA;
            private bool _isGUInit = false;
            private bool _isYCInit = false;

            [UnityEditor.Callbacks.DidReloadScripts]
            public static void Init() {
                YCCONFIGDATA = Resources.Load<YCConfig>("YCConfigData");
            }

            private void OnGUI() {
                if (Application.isPlaying) {
                    this.OnGUIGU();
                    this.OnGUICustom();
                } else {
                    GUILayout.Label("");
                    GUILayout.Label("Please enter play mode");
                }
            }

            private void UpdateData() {
                if (Application.isPlaying) {
                    if (this._isGUInit == false) {
                        this.InitDataGU();
                    }
                    if (this._isYCInit == false) {
                        this.InitDataGU();
                    }
                    this.UpdateDataGU();
                }
            }

            private void OnInspectorUpdate() {
                this.UpdateData();
                Repaint();
            }

            // ----------------------------------- GameUtils Data -----------------------------------

            public static string TIME = "";
            public static string ABTEST_DATA = "";
            public static string INTERDELAY_DATA = "";

            private void InitDataGU() {
                if (YCManager.instance != null) {
                    ABTEST_DATA = YCManager.instance.abTestingManager.GetPlayerSample() != "" ? YCManager.instance.abTestingManager.GetPlayerSample() : "Control";
                    this._isGUInit = true;
                }
            }

            private void UpdateDataGU() {
                TIME = TimeSpan.FromSeconds(Time.time).ToString("hh':'mm':'ss");
                INTERDELAY_DATA = this.GetCurrentInterstitialDelay();
            }

            private void OnGUIGU() {
                GUILayout.Label("------ GameUtils data ------");
                GUILayout.Label("Time since start : " + TIME);
                GUILayout.Label("Current AB test : " + ABTEST_DATA);
                GUILayout.Label("Delay before next interstitial : " + INTERDELAY_DATA);
            }

            private string GetCurrentInterstitialDelay() {
#if UNITY_ANDROID || UNITY_IOS
                if (YCManager.instance.adsManager.delayInterstitial != 0 && (string.IsNullOrEmpty(YCCONFIGDATA.AndroidInterstitial) == false || string.IsNullOrEmpty(YCCONFIGDATA.IosInterstitial) == false)) {
                    return YCManager.instance.adsManager.delayInterstitial > 0 ? YCManager.instance.adsManager.delayInterstitial.ToString("F2") + " seconds" : "Ready";
                } else {
                    return "Empty ad unit";
                }
#endif
                return "Not in Android or iOS platform";
            }

            // ----------------------------------- Custom Data -----------------------------------


            private void OnGUICustom() {
                GUILayout.Label("");
                GUILayout.Label("------ Custom data ------");
                if (YCEditorUtils.debugWindowCustomDatas.Count > 0) {
                    foreach (KeyValuePair<string, string> data in YCEditorUtils.debugWindowCustomDatas) {
                        GUILayout.Label(data.Key + " : " + data.Value);
                    }
                } else {
                    GUILayout.Label("You do not track any custom data. To start tracking custom data, use this line of code :");
                    EditorGUILayout.SelectableLabel("YsoCorp.GameUtils.YCEditorUtils.debugWindowCustomDatas[\"My unique title\"] = \"My string value\";");
                    GUILayout.Label("Since it's a dictionary, you can reassign the value to keep track of the changes.");
                }
            }
        }
    }
}