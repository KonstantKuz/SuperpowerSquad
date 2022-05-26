using System;
using System.Collections.Generic;
using UnityRandom = UnityEngine.Random;

namespace Feofun.Extension
{
    public static class ListExtension
    {
        public static T Random<T>(this IReadOnlyList<T> collection)
        {
            int randomNumber = UnityRandom.Range(0, collection.Count);
            return collection[randomNumber];
        }

        public static IEnumerable<T> RandomUnique<T>(this List<T> collection, int randomCount)
        {
            if (randomCount < 0) {
                throw new ArgumentOutOfRangeException(nameof(randomCount), "Random count is out of range, randomCount < 0");
            }
            if (randomCount > collection.Count) {
                throw new ArgumentOutOfRangeException(nameof(randomCount), "Random count is out of range, randomCount > collection.Count");
            }
            for (int i = 0; i < randomCount; i++) {
                var item = collection.Random();
                collection.Remove(item);
                yield return item;
            }
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