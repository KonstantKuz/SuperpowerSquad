using JetBrains.Annotations;

namespace Editor.Scripts.PreProcess
{
    public static class BuildPreprocessor
    {
        private const string DEBUG_CONSOLE_DEFINE = "DEBUG_CONSOLE_ENABLED"; 
        private const string JENKINS_BUILD_DEFINE = "JENKINS_BUILD";
        
        public static void Prepare(bool debugConsoleEnabled, [CanBeNull] string loggerLevel)
        {
            SetDebugConsoleEnabled(debugConsoleEnabled);
            if (!string.IsNullOrEmpty(loggerLevel)) {
                BuildLoggerConfig(loggerLevel);
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
        private static void BuildLoggerConfig(string loggerLevel)
        {
            ConfigPreprocessor.BuildLoggerConfig(loggerLevel);
        }
    }
}