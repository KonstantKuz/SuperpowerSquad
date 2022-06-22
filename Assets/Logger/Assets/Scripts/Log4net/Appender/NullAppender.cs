using log4net.Appender;
using log4net.Core;

namespace Logger.Assets.Scripts.Log4net.Appender
{
    public class NullAppender : AppenderSkeleton
    {
        protected override void Append(LoggingEvent loggingEvent)
        { 
            return;
        }
    }
}