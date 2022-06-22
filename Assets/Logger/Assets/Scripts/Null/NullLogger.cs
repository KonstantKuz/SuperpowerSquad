using System;

namespace Logger.Assets.Scripts.Null
{
    public class NullLogger : ILogger
    {
        public void Trace(string message)
        {
        }

        public void Trace(string message, Exception e)
        {
        }
        
        public void Debug(string message)
        {
        }

        public void Debug(string message, Exception e)
        {
        }
        public void Info(string message)
        {
        }

        public void Info(string message, Exception e)
        {
        }
        public void Warn(string message)
        {
        }

        public void Warn(string message, Exception e)
        {
        }

        public void Error(string message)
        {
        }

        public void Error(string message, Exception e)
        {
        }
        
    }
}