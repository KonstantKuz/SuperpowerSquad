using UnityEngine;
using Logger.Assets.Scripts;

namespace Survivors.Logger
{
    public static class LoggerInitializer
    {
        private const string LOGGER_CONFIG_PATH = "Logger/LoggerConfig";
        
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void Configure()
        {
            var configured = LoggerConfigurator.Configure(LOGGER_CONFIG_PATH);
            LoggerFactory.GetLogger(typeof(LoggerInitializer)).Info($"Logger has configured:= {configured}, Actiive logger:={LoggerConfigurator.ActiveLogger}");
        }
    }
}