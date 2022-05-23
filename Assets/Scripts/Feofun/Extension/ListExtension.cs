using System;
using System.Collections.Generic;
using UnityRandom = UnityEngine.Random;

namespace LegionMaster.Extension
{
    public static class ListExtension
    {
        public static T Random<T>(this IReadOnlyList<T> collection)
        {
            int randomNumber = UnityRandom.Range(0, collection.Count);
            return collection[randomNumber];
        }
        public static T Random<T>(this IReadOnlyList<T> collection, int minInclusive, int maxExclusive, int seed = 0)
        {
            if (maxExclusive > collection.Count) {
                throw new ArgumentOutOfRangeException(nameof(maxExclusive), "max value is out of range, max > count");
            }
            if (minInclusive < 0) {
                throw new ArgumentOutOfRangeException(nameof(minInclusive), "min value is out of range, min < 0");
            }
            int randomNumber = new Random(seed).Next(minInclusive, maxExclusive);
            return collection[randomNumber];
        }
    }
}