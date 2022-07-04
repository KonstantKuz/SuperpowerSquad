using JetBrains.Annotations;
using log4net.Appender;
using log4net.Core;
using UnityEngine;

namespace Logger.Log4net.Appender
{
    [UsedImplicitly]
    public class UnityDebugAppender : AppenderSkeleton
    {
        protected override void Append(LoggingEvent loggingEvent)
        {
            var message = RenderLoggingEvent(loggingEvent);
            switch (loggingEvent.Level.Name) {
                case "WARN": {
                    Debug.LogWarning(message);
                    break;
                }
                case "ERROR": {
                    Debug.LogError(message);
                    break;
                }
                default: {
                    Debug.Log(message);
                    break;
                }
            }
        }
    }
}