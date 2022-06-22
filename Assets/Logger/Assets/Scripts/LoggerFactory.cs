using System;
using System.Reflection;
using Logger.Assets.Scripts.Log4net;
using Logger.Assets.Scripts.Unity;

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
            switch (LoggerConfigurator.ActiveLogger) {
                case LoggerType.Log4Net:
                    return new Log4NetLogger(log4net.LogManager.GetLogger(clazz), clazz);
                default:
                    return new UnityLogger();
            }
        }
        public static ILogger GetLogger(string name)
        {
            switch (LoggerConfigurator.ActiveLogger) {
                case LoggerType.Log4Net:
                    return new Log4NetLogger(log4net.LogManager.GetLogger(name), Assembly.GetCallingAssembly().GetType());
                default:
                    return new UnityLogger();
            }
        }
    }
}