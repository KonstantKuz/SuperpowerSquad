using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using Survivors.Squad.Formation;
using UnityEditor;
using UnityEngine;

namespace Editor.Scripts
{
    public class CirclePackingAssetBuilder
    {
        [MenuItem("Survivors/GenerateCirclePacking")]
        static void GenerateCirclePacking()
        {
            /*
             *  Файлы с данными для оптимальной упаковки круга взяты отсюда - http://hydra.nat.uni-magdeburg.de/packing/cci/
             *  Они содержат координаты центров для каждого количества кругов
             *  Пока я скопировал в проект наборы до 40 кругов. Потребуется больше - можно добавить...
             */
            var guids = AssetDatabase.FindAssets("t:TextAsset", new [] {"Assets/CirclePacking"});
            var dict = new Dictionary<int, CirclePackingData.CirclePacking>();
            foreach (var guid in guids)
            {
                var path = AssetDatabase.GUIDToAssetPath(guid);
                var text = AssetDatabase.LoadAssetAtPath<TextAsset>(path).text;
                using var reader = new StringReader(text);
                var packing = new CirclePackingData.CirclePacking
                {
                    Positions = text.Split('\n')
                        .Where(line => !string.IsNullOrWhiteSpace(line))
                        .Select(ParseLine)
                        .ToList()
                };
                dict[packing.Positions.Count] = packing;
            }
            
            var asset = ScriptableObject.CreateInstance<CirclePackingData>();
            asset.SetData(dict.OrderBy(it => it.Key).Select(it => it.Value).ToList());
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