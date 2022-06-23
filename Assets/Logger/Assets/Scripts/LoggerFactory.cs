using System;
using System.Reflection;
using Logger.Assets.Scripts.Log4net;
using Logger.Assets.Scripts.Unity;

namespace Logger.Assets.Scripts
{
    public static class LoggerFactory
    {
        private static readonly UnityLogger _unityLogger = new UnityLogger();
        
        public static ILogger GetLogger<T>()
        {
            return GetLogger(typeof(T));
        } 
        public static ILogger GetLogger(Type clazz)
        {
            return LoggerConfigurator.ActiveLogger switch {
                    LoggerType.Log4Net => new Log4NetLogger(log4net.LogManager.GetLogger(clazz), clazz),
                    _ => _unityLogger
            };
        }
        public static ILogger GetLogger(string name)
        {
            return LoggerConfigurator.ActiveLogger switch {
                    LoggerType.Log4Net => new Log4NetLogger(log4net.LogManager.GetLogger(name), Assembly.GetCallingAssembly().GetType()),
                    _ => _unityLogger
            };
        }
    }
}