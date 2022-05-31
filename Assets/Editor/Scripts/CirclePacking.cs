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
            var dict = new Dictionary<int, List<Pos>>();
            foreach (var guid in guids)
            {
                var list = new List<Pos>();
                var path = AssetDatabase.GUIDToAssetPath(guid);
                var text = AssetDatabase.LoadAssetAtPath<TextAsset>(path).text;
                using var reader = new StringReader(text);
                foreach (var line in text.Split('\n'))
                {
                    if (string.IsNullOrWhiteSpace(line)) continue;
                    var items = line.Split(' ').Where(it => !string.IsNullOrEmpty(it)).ToList();
                    var x = float.Parse(items[1], CultureInfo.InvariantCulture);
                    var y = float.Parse(items[2], CultureInfo.InvariantCulture);
                    list.Add(new Pos
                    {
                        X = x,
                        Y = y
                    });
                }
                dict[list.Count] = list;
            }
            
            var asset = ScriptableObject.CreateInstance<CirclePackingData>();
            asset.Packings = dict.OrderBy(it => it.Key).SelectMany(it => it.Value).ToList();
            AssetDatabase.CreateAsset(asset, "Assets/Resources/CirclePacking.asset");
            AssetDatabase.SaveAssets();
        }
    }
}