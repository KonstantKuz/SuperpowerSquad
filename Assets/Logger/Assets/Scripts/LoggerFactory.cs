using System.Reflection;
using Logger.Assets.Scripts.Log4net;
using NullLogger = Logger.Assets.Scripts.Null.NullLogger;

namespace Logger.Assets.Scripts
{
    public static class LoggerFactory
    {
        public static ILogger GetLogger<T>()
        {
            switch (LoggerConfigurator.ActiveLogger) {
                case LoggerType.Log4Net:
                    return new Log4NetLogger(log4net.LogManager.GetLogger(typeof(T)), typeof(T));
                default:
                    return new NullLogger();
            }
        }

        public static ILogger GetLogger(string name)
        {
            switch (LoggerConfigurator.ActiveLogger) {
                case LoggerType.Log4Net:
                    return new Log4NetLogger(log4net.LogManager.GetLogger(name), Assembly.GetCallingAssembly().GetType());
                default:
                    return new NullLogger();
            }
        }
    }
}