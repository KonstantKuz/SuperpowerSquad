using System.IO;
using System.Text.RegularExpressions;
using Survivors.Logger;
using UnityEngine;

namespace Editor.Scripts.PreProcess
{
    public static class ConfigPreprocessor
    {
        private const string LOGGER_REPLACE_PATTERN = "(TRACE|DEBUG|INFO|WARN|ERROR)";
        private const string LOGGER_CONFIG_PATH_PATTERN = "Resources/{0}.xml";

        public static void BuildLoggerConfig(string loggerLevel)
        {
            var editorPath = Path.Combine(Application.dataPath, string.Format(LOGGER_CONFIG_PATH_PATTERN, LoggerInitializer.EDITOR_LOGGER_CONFIG_PATH));
            string config = File.ReadAllText(editorPath);
            config = Regex.Replace(config, LOGGER_REPLACE_PATTERN, loggerLevel);
            var buildPath = Path.Combine(Application.dataPath, string.Format(LOGGER_CONFIG_PATH_PATTERN, LoggerInitializer.BUILD_LOGGER_CONFIG_PATH));
            File.WriteAllText(buildPath, config);
        }
    }
}