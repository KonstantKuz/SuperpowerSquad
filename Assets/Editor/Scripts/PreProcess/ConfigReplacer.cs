using System.IO;
using System.Text.RegularExpressions;
using Survivors.Logger;
using UnityEngine;

namespace Editor.Scripts.PreProcess
{
    public static class ConfigReplacer
    {
        private const string LOGGER_REPLACE_PATTERN = "(TRACE|DEBUG|INFO|WARN|ERROR)";
        private static readonly string LOGGER_CONFIG_PATH = $"Resources/{LoggerInitializer.LOGGER_CONFIG_PATH}.xml";
        
        public static void ReplaceLoggerLevel(string loggerLevel)
        {
            var fullPath = Path.Combine(Application.dataPath, LOGGER_CONFIG_PATH);
            string config = File.ReadAllText(fullPath);
            config = Regex.Replace(config, LOGGER_REPLACE_PATTERN, loggerLevel);
            File.WriteAllText(fullPath, config);

        }
    }
}