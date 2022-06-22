using System;
using log4net;
using log4net.Core;

namespace Logger.Assets.Scripts.Log4net
{
    internal class Log4NetLogger : ILogger
    {
        private readonly ILog _internalLogger;
        private readonly Type _type;

        public Log4NetLogger(ILog internalLogger, Type type)
        {
            _internalLogger = internalLogger;
            _type = type;
        }
        
        public void Trace(string message)
        {
            Write(Level.Trace, LogRecord.Create(message));
        }

        public void Trace(string message, Exception e)
        {
            Write(Level.Trace, LogRecord.Create(message).Exception(e));
        }
        
        public void Debug(string message)
        {
            Write(Level.Debug, LogRecord.Create(message));
        }

        public void Debug(string message, Exception e)
        {
            Write(Level.Debug, LogRecord.Create(message).Exception(e));
        }

        public void Info(string message)
        {
            Write(Level.Info, LogRecord.Create(message));
        }

        public void Info(string message, Exception e)
        {
            Write(Level.Info, LogRecord.Create(message).Exception(e));
        }
        
        
        public void Warn(string message)
        {
            Write(Level.Warn, LogRecord.Create(message));
        }

        public void Warn(string message, Exception e)
        {
            Write(Level.Warn, LogRecord.Create(message).Exception(e));
        }
        
        public void Error(string message)
        {
            Write(Level.Error, LogRecord.Create(message));
        }

        public void Error(string message, Exception e)
        {
            Write(Level.Error, LogRecord.Create(message).Exception(e));
        }
        private void Write(Level logLevel, LogRecord logRecord)
        {

            /*Dictionary<string, string> data = new Dictionary<string, string>() {
                    {"message", logRecord.Message}
            };
            if (logLevel == Level.Warn || logLevel == Level.Error) {
                data.Add("currentStackTrace", new StackTrace(2) + "\n");
            }*/
            _internalLogger.Logger.Log(_type, logLevel, logRecord.Message, logRecord.Exception1);
        }
    }
}