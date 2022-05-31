using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using Survivors.Squad.Formation;
using UnityEditor;
using UnityEngine;

namespace Editor.Scripts
{
    public class CirclePacking
    {
        [MenuItem("Survivors/GenerateCirclePacking")]
        static void GenerateCirclePacking()
        {
            var guids = AssetDatabase.FindAssets("t:TextAsset", new [] {"Assets/CirclePacking"});
            var dict = new Dictionary<int, List<CirclePackingData.Pos>>();
            foreach (var guid in guids)
            {
                var path = AssetDatabase.GUIDToAssetPath(guid);
                var text = AssetDatabase.LoadAssetAtPath<TextAsset>(path).text;
                using var reader = new StringReader(text);
                var list = text.Split('\n')
                    .Where(line => !string.IsNullOrWhiteSpace(line))
                    .Select(ParseLine)
                    .ToList();
                dict[list.Count] = list;
            }
            
            var asset = ScriptableObject.CreateInstance<CirclePackingData>();
            asset.SetData(dict);
            AssetDatabase.CreateAsset(asset, "Assets/Resources/CirclePacking.asset");
            AssetDatabase.SaveAssets();
        }

        private static CirclePackingData.Pos ParseLine(string line)
        {
            var items = line.Split(' ').Where(it => !string.IsNullOrEmpty(it)).ToList();
            var x = float.Parse(items[1], CultureInfo.InvariantCulture);
            var y = float.Parse(items[2], CultureInfo.InvariantCulture);
            var pos = new CirclePackingData.Pos
            {
                X = x,
                Y = y
            };
            return pos;
        }
    }
}