#if UNITY_EDITOR

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using Feofun.Config.Csv;
using Feofun.Config.Serializers;
using JetBrains.Annotations;
using UnityEditor;
using UnityEngine;

namespace Editor.Scripts.Config
{
    public class ConfigDownloader
    {
        private static readonly string[] ExcludedSheets = {"Sheets"};
        
        private readonly string _url;
        private readonly int _sheetListId;
        private readonly WebClient _webClient;

        public ConfigDownloader(string url, int sheetListId)
        {
            _url = url;
            _sheetListId = sheetListId;
            _webClient = CreateWebClient();
        }

        public void Download(string outputFolder, [CanBeNull] IEnumerable<string> configs = null)
        {
            Debug.Log($"Starting download from {_url}");

            var sheetList = LoadSheetList();

            foreach (var config in configs ?? sheetList.Keys.Except(ExcludedSheets))
            {
                var csvUrl = BuildSheetUrl(_url, sheetList[config]);
                var outputFilename = Path.Combine(Application.dataPath, outputFolder, $"{config}.csv");                
                Debug.Log($"Downloading: {csvUrl} to {outputFilename}");
                _webClient.DownloadFile(csvUrl,  outputFilename);
            }
            AssetDatabase.ImportAsset(Path.Combine("Assets", outputFolder), ImportAssetOptions.ImportRecursive);

            Debug.Log($"Finished downloading");
        }

        private static string BuildSheetUrl(string url, string sheetId)
        {
            return $"{url}/export?format=csv&gid={sheetId}";
        }

        private static WebClient CreateWebClient()
        {
            var webClient = new WebClient();
            webClient.Headers.Add("Accept", "text/csv");
            return webClient;
        }

        private Dictionary<string, string> LoadSheetList()
        {
            Debug.Log($"Loading sheet list...");
            var csvUrl = BuildSheetUrl(_url, _sheetListId.ToString());
            Debug.Log($"Downloading: {csvUrl}");
            var data = _webClient.DownloadString(csvUrl);
            try
            {
                return new CsvSerializer().ReadDictionary(CsvConfigDeserializer.ToMemoryStream(data));
            }
            catch (Exception e)
            {
                Debug.LogError("Failed to parse sheet list. Probably sheet list id is wrong...");
                Debug.LogException(e);
                throw;
            }
        }
    }
}

#endif