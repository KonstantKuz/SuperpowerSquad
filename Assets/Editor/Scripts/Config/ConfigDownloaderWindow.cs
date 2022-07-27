using Survivors.Config;
using UnityEditor;
using UnityEngine;

namespace Editor.Scripts.Config
{
    public class ConfigDownloaderWindow : EditorWindow
    {
        private string _mainUrl = "https://docs.google.com/spreadsheets/d/1YuICZZpsBsQgW38m_gBa_I7YGjVk4GRw6dNZzWCqDKg";

        private const int MAIN_SHEET_ID_LIST = 515831250; //id of sheet that contains list of all other sheets
        private const string MAIN_CONFIG_PATH = "Resources/Configs";

        [MenuItem("Survivors/Download Configs")]
        public static void ShowWindow()
        {
            GetWindow(typeof(ConfigDownloaderWindow));
        }
        
        private void OnGUI () {
            GUILayout.Label("URL of google sheet with main configs", EditorStyles.boldLabel);
            GUILayout.Label("Should have format: https://docs.google.com/spreadsheets/d/KEY", EditorStyles.label);
            GUILayout.Label("without last / and anything after it", EditorStyles.label);
            _mainUrl = EditorGUILayout.TextField ("Url", _mainUrl);

            if (GUILayout.Button("Download all"))
            {
                DownloadAll();
            }
            if (GUILayout.Button("Download localization only"))
            {
                DownloadLocalization();
            }
        }

        private void DownloadAll()
        {
            new ConfigDownloader(_mainUrl, MAIN_SHEET_ID_LIST).Download(MAIN_CONFIG_PATH);
        }
        
        private void DownloadLocalization()
        {
            new ConfigDownloader(_mainUrl, MAIN_SHEET_ID_LIST).Download(MAIN_CONFIG_PATH,new[] { Configs.LOCALIZATION });
        }
    }
}