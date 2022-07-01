using System;
using System.Collections.Generic;
using Logger.Log4net;

namespace Logger
{
    public static class LoggerFactory
    {
        private static readonly Dictionary<Type, ILogger> _loggers = new Dictionary<Type, ILogger>();
     
        public static ILogger GetLogger<T>()
        {
            return GetLogger(typeof(T));
        } 
        public static ILogger GetLogger(Type clazz)
        {
            if (_loggers.ContainsKey(clazz)) {
                return _loggers[clazz];
            }
            var logger = CreateLogger(clazz);
            _loggers[clazz] = logger;
            return logger;
        }
        private static ILogger CreateLogger(Type clazz)
        {
            return LoggerConfigurator.ActiveLogger switch {
                    LoggerType.Log4Net => new Log4NetLogger(log4net.LogManager.GetLogger(clazz), clazz),
                    _ => throw new ArgumentOutOfRangeException()
            };
        }
    }
}