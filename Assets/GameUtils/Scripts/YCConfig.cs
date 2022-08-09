using UnityEngine;
using UnityEditor;
using System;
using System.IO;
using System.Net;
using Facebook.Unity.Settings;
using System.Collections.Generic;
using System.Text.RegularExpressions;
#if UNITY_EDITOR
using UnityEditor.PackageManager;
#endif

namespace YsoCorp {
    namespace GameUtils {
#if UNITY_EDITOR
        [CustomEditor(typeof(YCConfig))]
        public class YCConfigEditor : Editor {
            public override void OnInspectorGUI() {
                this.DrawDefaultInspector();
                GUILayout.Space(10);
                YCConfig myTarget = (YCConfig)this.target;
                if (GUILayout.Button("Import Config")) {
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
                if (GUILayout.Button("Activate In App Purchases")) {
                    myTarget.AddDefineSymbolsForGroup("IN_APP_PURCHASING");
                    Client.Add("com.unity.purchasing@4.0.3");
                }
#endif

#if FIREBASE
                if (GUILayout.Button("Disactivate Firebase")) {
                    myTarget.RemoveDefineSymbolsForGroup("FIREBASE");
                }
#else
                if (GUILayout.Button("Activate Firebase")) {
                    if (Directory.Exists("Assets/Firebase")) {
                        myTarget.AddDefineSymbolsForGroup("FIREBASE");
                    } else {
                        myTarget.DisplayDialog("Error", "This only for validate game.\nPlease import Firebase Analytics before.", "Ok");
                    }
                }
#endif

            }
        }
#endif

        [CreateAssetMenu(fileName = "YCConfigData", menuName = "YsoCorp/Configuration", order = 1)]
        public class YCConfig : ScriptableObject {

            public static string VERSION = "1.28.0";

            [Serializable]
            public struct DataData {
                public InfosData data;
            }

            [Serializable]
            public struct InfosData {
                public string key;
                public bool isYsocorp;
                public string name;
                public string android_key;
                public string ios_bundle_id;
                public string ios_key;
                public string facebook_app_id;
                public string admob_android_app_id;
                public string admob_ios_app_id;
                public string google_services_ios;
                public string google_services_android;
                public ApplovinData applovin;
                public MmpsData mmps;
            }

            // APPLOVIN
            [Serializable]
            public struct ApplovinData {
                public ApplovinAdUnitsData adunits;
            }
            [Serializable]
            public struct ApplovinAdUnitsData {
                public ApplovinAdUnitsOsData ios;
                public ApplovinAdUnitsOsData android;
            }
            [Serializable]
            public struct ApplovinAdUnitsOsData {
                public string interstitial;
                public string rewarded;
                public string banner;
            }

            [Serializable]
            public struct MmpData {
                public bool active;
            }
            [Serializable]
            public struct AdjustMmpData {
                public bool active;
                public string app_token;
            }

            [Serializable]
            public struct MmpsData {
                public AdjustMmpData adjust;
                public MmpData tenjin;
            }

            [Flags]
            public enum YCLogCategories {
                GUResourceLoad = 1,
                GURequests = 2,
                ApplovinMax = 4
            } 

            [Header("------------------------------- CONFIG")]
            public string gameYcId;

#if IN_APP_PURCHASING
            [Header("InApp")]
            public string InAppRemoveAds;
            public bool InAppRemoveAdsCanRemoveInBanner = true;
            public string[] InAppConsumables;
            public bool InappDebug = true;
#else
            [Header("InApp (Enable & Import InApp in Service)")]
            [YcReadOnly] public string InAppRemoveAds;
            [YcReadOnly] public bool InAppRemoveAdsCanRemoveInBanner;
            public string[] InAppConsumables { get; set; } = { };
            [YcReadOnly] public bool InappDebug = true;
#endif

            [Header("A/B Tests")]
            public int ABVersion = 1;
            public string ABForcedSample = "";
            public string[] ABSamples = { };
            public bool ABDebugLog = true;

            [Header("Ads")]
            public bool BannerDisplayOnInit = true;
            [YcBoolHide("BannerDisplayOnInit", true)]
            public bool BannerDisplayOnInitEditor = true;
            public float InterstitialInterval = 20;

            [Header("System Logs")]
            public YCLogCategories activeLogs;

            [Header("------------------------------- AUTO IMPORT")]

            [YcReadOnly] public string Name;
            [Space(10)]
            [YcReadOnly] public string FbAppId;
            [YcReadOnly] public string appleId = "";

            [Header("Mmp")]
            [YcReadOnly] public bool MmpAdjust = true;
            [YcReadOnly] public string MmpAdjustAppToken;
            [YcReadOnly] public bool MmpTenjin = true;

            [Header("Google AdMob")]
            [YcReadOnly] public string AdMobAndroidAppId = "";
            [YcReadOnly] public string AdMobIosAppId = "";

            [Header("Max AppLovin")]
            [YcReadOnly] public string IosInterstitial;
            [YcReadOnly] public string IosRewarded;
            [YcReadOnly] public string IosBanner;
            [Space(5)]
            [YcReadOnly] public string AndroidInterstitial;
            [YcReadOnly] public string AndroidRewarded;
            [YcReadOnly] public string AndroidBanner;

            public string deviceKey {
                get { return SystemInfo.deviceUniqueIdentifier; }
                set { }
            }

            public static YCConfig Create() {
                return Resources.Load<YCConfig>("YCConfigData");
            }

            public void LogWarning(string msg) {
                Debug.LogWarning("[GameUtils][CONFIG]" + msg);
            }

            public bool HasInApps() {
#if IN_APP_PURCHASING
                if (this.InAppRemoveAds != null && this.InAppRemoveAds != "") {
                    return true;
                }
                if (this.InAppConsumables.Length > 0) {
                    return true;
                }
#endif
                return false;
            }

            public string GetAndroidId() {
                return Application.identifier;
            }

            public void DisplayDialog(string title, string msg, string ok) {
#if UNITY_EDITOR
                EditorUtility.DisplayDialog(title, msg, ok);
#endif
            }

            public void DisplayImportConfigDialog(bool succeeded, InfosData infos, string msg = "") {
#if UNITY_EDITOR
                string okButton = "Ok";
                string title = "";
                string msgAnd = "";
                string msgIos = "";

                if (succeeded && infos.name != "") {
                    title = "Success";
                    if (infos.facebook_app_id == "") msg += "Facebook ID is empty\n";

                    // Android
                    if (infos.android_key != "") {
                        if (infos.android_key != PlayerSettings.GetApplicationIdentifier(BuildTargetGroup.Android)) msgAnd += "- Package name is different\n";
                        if (infos.isYsocorp) {
                            if (infos.applovin.adunits.android.interstitial == "") msgAnd += "- Ad Unit : Interstitial is empty\n";
                            if (infos.applovin.adunits.android.rewarded == "") msgAnd += "- Ad Unit : Rewarded is empty\n";
                            if (infos.applovin.adunits.android.banner == "") msgAnd += "- Ad Unit : Banner is empty\n";
                        }
                        if (Directory.Exists("Assets/MaxSdk/Mediation/Google") && infos.admob_android_app_id == "") msgAnd += "- Google AdMob ID is Empty\n";
                        if (Directory.Exists("Assets/MaxSdk/Mediation/Google") == false && infos.admob_android_app_id != "") msgAnd += "- Google AdMob ID found but the network is not installed\n";
                        if (msgAnd != "") {
                            msgAnd = "\n-------- Android --------\n" + msgAnd;
                        }
                    }

                    // iOS
                    if (infos.ios_bundle_id != "") {
                        if (infos.ios_bundle_id != PlayerSettings.GetApplicationIdentifier(BuildTargetGroup.iOS)) msgIos += "- Bundle identifier is different\n";
                        if (infos.ios_key == "") msgIos += "- Apple ID is empty\n";
                        if (infos.isYsocorp) {
                            if (infos.applovin.adunits.ios.interstitial == "") msgIos += "- Ad Unit : Interstitial is empty\n";
                            if (infos.applovin.adunits.ios.rewarded == "") msgIos += "- Ad Unit : Rewarded is empty\n";
                            if (infos.applovin.adunits.ios.banner == "") msgIos += "- Ad Unit : Banner is empty\n";
                        }
                        if (Directory.Exists("Assets/MaxSdk/Mediation/Google") && infos.admob_ios_app_id == "") msgIos += "- Google AdMob ID is Empty\n";
                        if (Directory.Exists("Assets/MaxSdk/Mediation/Google") == false && infos.admob_ios_app_id != "") msgIos += "- Google AdMob ID found but the network is not installed\n";
                        if (msgIos != "") {
                            msgIos = "\n---------- iOS ----------\n" + msgIos;
                        }
                    }

                    // Full Error
                    msg += msgAnd + msgIos;
                    if (msg != "") {
                        msg = "\n/!\\ Warning /!\\\n" + msg;
                    } else {
                        msg = "Import config Succeeded!";
                    }
                } else {
                    title = "Error";
                }
                EditorUtility.DisplayDialog(title, msg, okButton);
#endif
                }

            public void DisplayImportConfigDialog(bool succeeded, string msg = "") {
#if UNITY_EDITOR
                InfosData data = new InfosData();
                this.DisplayImportConfigDialog(succeeded, data, msg);
#endif
            }

            public void EditorImportConfig() {
                if (this.gameYcId != "") {
                    string url = RequestManager.GetUrlEmptyStatic("games/setting/" + this.gameYcId + "/" + Application.identifier, true);
                    HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(url);
                    request.Method = "Get";
                    request.ContentType = "application/json";
                    try {
                        using (HttpWebResponse response = (HttpWebResponse)request.GetResponse()) {
                            using (var reader = new StreamReader(response.GetResponseStream())) {
                                InfosData infos = Newtonsoft.Json.JsonConvert.DeserializeObject<DataData>(reader.ReadToEnd()).data;
                                if (infos.name != "") {
                                    this.Name = infos.name;
                                    this.FbAppId = infos.facebook_app_id;
                                    this.appleId = infos.ios_key;

                                    this.AdMobAndroidAppId = infos.admob_android_app_id;
                                    this.AdMobIosAppId = infos.admob_ios_app_id;

                                    this.IosInterstitial = infos.applovin.adunits.ios.interstitial;
                                    this.IosRewarded = infos.applovin.adunits.ios.rewarded;
                                    this.IosBanner = infos.applovin.adunits.ios.banner;
                                    this.AndroidInterstitial = infos.applovin.adunits.android.interstitial;
                                    this.AndroidRewarded = infos.applovin.adunits.android.rewarded;
                                    this.AndroidBanner = infos.applovin.adunits.android.banner;
                                    // MMPs
                                    this.MmpAdjust = infos.mmps.adjust.active;
                                    this.MmpAdjustAppToken = infos.mmps.adjust.active ? infos.mmps.adjust.app_token : "";
                                    this.MmpTenjin = infos.mmps.tenjin.active;
                                    this.DisplayImportConfigDialog(true, infos);
                                    this.InitFacebook();
                                    this.InitMax();
                                    this.InitFirebase(infos);
                                } else {
                                    this.DisplayImportConfigDialog(false, "Impossible to import config. Check your Game Yc Id or your connection.");
                                }
                            }
                        }
                    } catch (Exception) {

                        this.DisplayImportConfigDialog(false, "Impossible to import config. Check your Game Yc Id or your connection.");
                    }
                } else {
                    this.DisplayImportConfigDialog(false, "Please enter Game Yc Id.");
                }
            }

            public void AddDefineSymbolsForGroup(string def) {
#if UNITY_EDITOR
                PlayerSettings.SetScriptingDefineSymbolsForGroup(BuildTargetGroup.Standalone, PlayerSettings.GetScriptingDefineSymbolsForGroup(BuildTargetGroup.Standalone) + ";" + def);
                PlayerSettings.SetScriptingDefineSymbolsForGroup(BuildTargetGroup.iOS, PlayerSettings.GetScriptingDefineSymbolsForGroup(BuildTargetGroup.iOS) + ";" + def);
                PlayerSettings.SetScriptingDefineSymbolsForGroup(BuildTargetGroup.Android, PlayerSettings.GetScriptingDefineSymbolsForGroup(BuildTargetGroup.Android) + ";" + def);
                AssetDatabase.SaveAssets();
#endif
            }

            public void RemoveDefineSymbolsForGroup(string def) {
#if UNITY_EDITOR
                PlayerSettings.SetScriptingDefineSymbolsForGroup(BuildTargetGroup.Standalone, PlayerSettings.GetScriptingDefineSymbolsForGroup(BuildTargetGroup.Standalone).Replace(";" + def, ""));
                PlayerSettings.SetScriptingDefineSymbolsForGroup(BuildTargetGroup.iOS, PlayerSettings.GetScriptingDefineSymbolsForGroup(BuildTargetGroup.iOS).Replace(";" + def, ""));
                PlayerSettings.SetScriptingDefineSymbolsForGroup(BuildTargetGroup.Android, PlayerSettings.GetScriptingDefineSymbolsForGroup(BuildTargetGroup.Android).Replace(";" + def, ""));
                AssetDatabase.SaveAssets();
#endif
            }

            public void InitFacebook() {
#if UNITY_EDITOR
                FacebookSettings.AppIds = new List<string> { this.FbAppId };
                FacebookSettings.AppLabels = new List<string> { Application.productName };
                EditorUtility.SetDirty(FacebookSettings.Instance);
                AssetDatabase.SaveAssets();

                string destManifestPath = Path.Combine(Application.dataPath, "Plugins/Android/AndroidManifest.xml");
                string content = File.ReadAllText(destManifestPath);
                content = new Regex("fb[0-9]+").Replace(content, "fb" + this.FbAppId);
                content = new Regex("FacebookContentProvider[0-9]+").Replace(content, "FacebookContentProvider" + this.FbAppId);
                File.WriteAllText(destManifestPath, content);
#endif
            }

            public void InitMax() {
#if UNITY_EDITOR
                AppLovinSettings.Instance.AdMobIosAppId = this.AdMobIosAppId;
                AppLovinSettings.Instance.AdMobAndroidAppId = this.AdMobAndroidAppId;
                EditorUtility.SetDirty(AppLovinSettings.Instance);
                AssetDatabase.SaveAssets();
#endif
            }

            public void InitFirebase(InfosData infos) {
#if FIREBASE
                if (infos.google_services_android != "") {
                    this.CreateOrUpdateFileInAssets("GameUtils/google-services.json", infos.google_services_android);
                }
                if (infos.google_services_ios != "") {
                    this.CreateOrUpdateFileInAssets("GameUtils/GoogleService-Info.plist", infos.google_services_ios);
                }
#if UNITY_EDITOR
                AssetDatabase.Refresh(ImportAssetOptions.ForceUpdate);
#endif
#endif
            }

            public void CreateOrUpdateFileInAssets(string path, string content) {
                path = Application.dataPath + "/" + path;
                File.Delete(path);
                StreamWriter sw = File.CreateText(path);
                sw.Write(content + "\n");
                sw.Close();
            }

        }

    }

}
