using System;
using System.Reflection;
using Logger.Assets.Scripts.Log4net;

namespace Logger.Assets.Scripts
{
    public static class LoggerFactory
    {
        public static ILogger GetLogger<T>()
        {
            return GetLogger(typeof(T));
        } 
        public static ILogger GetLogger(Type clazz)
        {
            return LoggerConfigurator.ActiveLogger switch {
                    LoggerType.Log4Net => new Log4NetLogger(log4net.LogManager.GetLogger(clazz), clazz),
                    _ => throw new ArgumentOutOfRangeException()
            };
        }
        public static ILogger GetLogger(string name)
        {
            return LoggerConfigurator.ActiveLogger switch {
                    LoggerType.Log4Net => new Log4NetLogger(log4net.LogManager.GetLogger(name), Assembly.GetCallingAssembly().GetType()),
                    _ => throw new ArgumentOutOfRangeException()
            };
        }
    }
}