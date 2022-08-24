using UnityEngine;
using UnityEditor;
using UnityEngine.Networking;
using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using AppLovinMax.Scripts.IntegrationManager.Editor;
using Network = AppLovinMax.Scripts.IntegrationManager.Editor.Network;
using System.Net;

namespace YsoCorp {
    namespace GameUtils {

        public class YCMenuItemManager {

            static string WEBSITE_URL = "https://gameutils.ysocorp.com/";

            static YCConfig YCCONFIGDATA;


            [InitializeOnLoadMethod]
            private static void Init() {
                if (YCCONFIGDATA == null) {
                    YCCONFIGDATA = Resources.Load<YCConfig>("YCConfigData");
                }
            }


            [MenuItem("GameUtils/Open game's page", false, 1001)]
            static void MenuOpenGamePage() {
                if (YCCONFIGDATA.gameYcId != "") {
                    Application.OpenURL(WEBSITE_URL + YCCONFIGDATA.gameYcId + "/settings");
                } else {
                    EditorUtility.DisplayDialog("Game's web page", "Please enter your Game Yc ID in the YCConfigData.", "Ok");
                }
            }

            [MenuItem("GameUtils/Update GameUtils", false, 2001)]
            static void MenuUpdateGameutils() {
                YCUpdatesHandler.UpdateGameutils();
            }

            [MenuItem("GameUtils/Upgrade Applovin MAX", false, 2002)]
            static void MenuUpgradeMax() {
                if (YCUpdatesHandler.MAX_UPGRADING == false) {
                    YCUpdatesHandler.StartUpgradeMax();
                } else {
                    Debug.Log("Applovin is currently upgrading");
                }
            }

            [MenuItem("GameUtils/Import Config", false, 2003)]
            static void MenuImportConfig() {
                YCCONFIGDATA.EditorImportConfig();
            }

            [MenuItem("GameUtils/Tools/Display Debug Window", false, 3001)]
            static void MenuToolDebugWindow() {
                YCDebugWindow window = EditorWindow.GetWindow<YCDebugWindow>(false, "GameUtils Debug Window");
                YCDebugWindow.Init();
                window.Show();
            }
        }
    }
}