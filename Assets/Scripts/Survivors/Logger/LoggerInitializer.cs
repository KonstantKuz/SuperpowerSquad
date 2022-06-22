using UnityEngine;
using Logger.Assets.Scripts;

namespace Survivors.Logger
{
    public static class LoggerInitializer
    {
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void Configure()
        {
            var configured = LoggerConfigurator.Configure("Logger/LoggerConfig");
            LoggerFactory.GetLogger("LoggerInitializer").Info($"Logger has configured:= {configured}, Actiive logger:={LoggerConfigurator.ActiveLogger}");
        }
    }
}