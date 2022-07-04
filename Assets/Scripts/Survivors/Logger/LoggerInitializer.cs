using Logger;
using UnityEngine;

namespace Survivors.Logger
{
    public static class LoggerInitializer
    {
        public const string EDITOR_LOGGER_CONFIG_PATH = "Logger/LoggerConfig";    
        public const string BUILD_LOGGER_CONFIG_PATH = "Logger/LoggerConfig.build";
        
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void Configure()
        {
            var configured = LoggerConfigurator.Configure(BUILD_LOGGER_CONFIG_PATH);
            LoggerFactory.GetLogger(typeof(LoggerInitializer)).Info($"Logger has configured:= {configured}, Actiive logger:={LoggerConfigurator.ActiveLogger}");
        }
    }
}