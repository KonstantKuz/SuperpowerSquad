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
            var configPath = EDITOR_LOGGER_CONFIG_PATH;
#if PLATFORM_BUILD
            configPath = BUILD_LOGGER_CONFIG_PATH;
#endif
            var configured = LoggerConfigurator.Configure(configPath);
            LoggerFactory.GetLogger(typeof(LoggerInitializer)).Info($"Logger has configured:= {configured}, Actiive logger:={LoggerConfigurator.ActiveLogger}");
        }
    }
}