using System;
using UnityDebug = UnityEngine.Debug;

namespace Logger.Assets.Scripts.Unity
{
    public class UnityLogger : ILogger
    {
        public void Trace(string message)
        {
            UnityDebug.Log("[TRACE] | " + message);
        }

        public void Trace(string message, Exception e)
        {
            UnityDebug.Log($"[TRACE] | {message} | {e}");
        }
        
        public void Debug(string message)
        {
            UnityDebug.Log("[DEBUG] | " + message);
        }

        public void Debug(string message, Exception e)
        {
            UnityDebug.Log($"[DEBUG] | {message} | {e}");
        }
        public void Info(string message)
        {
            UnityDebug.Log("[INFO] | " + message);
        }

        public void Info(string message, Exception e)
        {
            UnityDebug.Log($"[INFO] | {message} | {e}");
        }
        public void Warn(string message)
        {
            UnityDebug.LogWarning(message);
        }

        public void Warn(string message, Exception e)
        {
            UnityDebug.LogWarning($"{message} | {e}");
        }
        public void Error(string message)
        {
            UnityDebug.LogError(message);
        }
        public void Error(string message, Exception e)
        {
            UnityDebug.LogError($"{message} | {e}");
        }
        public void Exception(Exception e)
        {
            UnityDebug.LogException(e);
        }

    }
}