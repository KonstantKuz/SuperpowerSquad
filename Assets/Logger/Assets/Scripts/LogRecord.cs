using System;

namespace Logger.Assets.Scripts
{
    public class LogRecord
    {
        
        private readonly string _message;
        
        private Exception _exception;

        private LogRecord(string message)
        {
            _message = message;
        }

        public static LogRecord Create(string message)
        {
            return new LogRecord(message);
        }
        public LogRecord Exception(Exception e)
        {
            _exception = e;
            return this;
        }

        public string Message
        {
            get { return _message; }
        }
        public Exception Exception1
        {
            get { return _exception; }
        }
    }
}