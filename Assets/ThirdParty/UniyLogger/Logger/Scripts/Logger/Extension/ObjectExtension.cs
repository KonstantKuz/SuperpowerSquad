
namespace Logger.Extension
{
    public static class ObjectExtension
    {
        public static ILogger Logger(this object obj)
        {
            return LoggerFactory.GetLogger(obj.GetType());
        }
    }
}