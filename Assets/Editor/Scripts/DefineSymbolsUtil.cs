using System.Collections.Generic;
using System.Linq;
using UnityEditor;

namespace Editor.Scripts
{
    public static class DefineSymbolsUtil
    {
        public static void Add(string symbol)
        {
            var current = GetList();
            if (current.Contains(symbol))
            {
                return;
            }

            current.Add(symbol);
            SetList(current);
        }

        public static void Remove(string symbol)
        {
            var current = GetList();
            if (!current.Contains(symbol))
            {
                return;
            }

            current.Remove(symbol);
            SetList(current);
        }

        private static List<string> GetList()
        {
            var definesString = PlayerSettings.GetScriptingDefineSymbolsForGroup(
                EditorUserBuildSettings.selectedBuildTargetGroup);
            return definesString.Split(';').ToList();
        }

        private static void SetList(List<string> symbols)
        {
            PlayerSettings.SetScriptingDefineSymbolsForGroup(
                EditorUserBuildSettings.selectedBuildTargetGroup, 
                string.Join(";", symbols.ToArray()));
        }
    }
}