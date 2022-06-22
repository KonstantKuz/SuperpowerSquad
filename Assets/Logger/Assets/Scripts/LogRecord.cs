using System;

namespace Logger.Assets.Scripts
{
    public class LogRecord
    {
        private readonly string _message;
        private Exception _exception;
        
        public string Message => _message;
        public Exception Exception => _exception;
        
        private LogRecord(string message)
        {
            _message = message;
        }
        public static LogRecord Create(string message)
        {
            return new LogRecord(message);
        }
        public LogRecord AddException(Exception e)
        {
            _exception = e;
            return this;
        }

    }
}