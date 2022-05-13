namespace Editor.Scripts
{
    public static class BuildPreprocessor
    {
        private const string DEBUG_CONSOLE_DEFINE = "DEBUG_CONSOLE_ENABLED";
        public static void Prepare(bool debugConsoleEnabled)
        {
            SetDebugConsoleEnabled(debugConsoleEnabled);
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
    }
}