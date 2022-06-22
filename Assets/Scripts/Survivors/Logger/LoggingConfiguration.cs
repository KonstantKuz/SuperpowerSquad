using UnityEngine;
using Logger.Assets.Scripts;

namespace Survivors.Logger
{
    public static class LoggingConfiguration
    {
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void Configure()
        {
            LoggerConfigurator.Configure(LoggerType.Log4Net);
        }
    }
}