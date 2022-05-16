using System;
using System.Collections.Generic;
using System.Linq;
using Random = UnityEngine.Random;

namespace LegionMaster.Extension
{
    public static class EnumExt
    {
        public static IEnumerable<T> Values<T>() where T : struct // enum
            => Enum.GetValues(typeof(T)).Cast<T>();
        
        public static T ValueOf<T>(string name) => (T) Enum.Parse(typeof(T), name, true);

        public static T GetRandom<T>() where T : struct
        {
            var names = Values<T>().ToArray();
            return names[Random.Range(0, names.Count())];
        }
    }
}