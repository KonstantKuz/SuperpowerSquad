using JetBrains.Annotations;

namespace Editor.Scripts.PreProcess
{
    public static class BuildPreprocessor
    {
        private const string DEBUG_CONSOLE_DEFINE = "DEBUG_CONSOLE_ENABLED";


        public static void Prepare(bool debugConsoleEnabled, [CanBeNull] string loggerLevel)
        {
            SetDebugConsoleEnabled(debugConsoleEnabled);
            if (!string.IsNullOrEmpty(loggerLevel)) {
                SetLoggerLevel(loggerLevel);
            }
        }

        private static void SetDebugConsoleEnabled(bool value)
        {
            if (value)
            {
                DefineSymbolsUtil.Add(DEBUG_CONSOLE_DEFINE);
            }
            else
            {
                DefineSymbolsUtil.Remove(DEBUG_CONSOLE_DEFINE);
            }
        }  
        private static void SetLoggerLevel(string loggerLevel)
        {
            ConfigReplacer.ReplaceLoggerLevel(loggerLevel);
        }
    }
}