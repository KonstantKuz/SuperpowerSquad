using UnityEngine;
using Logger.Assets.Scripts;
using ILogger = Logger.Assets.Scripts.ILogger;

namespace Survivors.Logger
{
    public static class LoggerInitializer
    {
        private static readonly ILogger _logger = LoggerFactory.GetLogger("LoggerInitializer");

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void Configure()
        {
            var configured = LoggerConfigurator.Configure("Logger/LoggerConfig");
            _logger.Info($"Logger has configured:= {configured}, Actiive logger:={LoggerConfigurator.ActiveLogger}");
        }
    }
}