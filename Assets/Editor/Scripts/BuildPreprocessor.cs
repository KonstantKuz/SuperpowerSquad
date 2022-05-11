using System.Collections.Generic;
using System.Linq;
using UnityEditor;

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
                AddDefineSymbol(DEBUG_CONSOLE_DEFINE);
            }
            else
            {
                RemoveDefineSymbol(DEBUG_CONSOLE_DEFINE);
            }
        }

        private static void AddDefineSymbol(string symbol)
        {
            var current = GetDefineSymbols();
            if (current.Contains(symbol))
            {
                return;
            }

            current.Add(symbol);
            SetDefineSymbols(current);
        }

        private static void RemoveDefineSymbol(string symbol)
        {
            var current = GetDefineSymbols();
            if (!current.Contains(symbol))
            {
                return;
            }

            current.Remove(symbol);
            SetDefineSymbols(current);
        }

        private static List<string> GetDefineSymbols()
        {
            string definesString = PlayerSettings.GetScriptingDefineSymbolsForGroup(
                EditorUserBuildSettings.selectedBuildTargetGroup);
            return definesString.Split(';').ToList();
        }

        private static void SetDefineSymbols(List<string> symbols)
        {
            PlayerSettings.SetScriptingDefineSymbolsForGroup(
                EditorUserBuildSettings.selectedBuildTargetGroup, 
                string.Join(";", symbols.ToArray()));
        }
    }
}