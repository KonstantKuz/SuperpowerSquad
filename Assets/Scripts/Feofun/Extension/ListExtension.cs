using System;
using System.Collections.Generic;
using UnityRandom = UnityEngine.Random;

namespace Feofun.Extension
{
    public static class ListExtension
    {
        public static HashSet<T> ToHashSet<T>(this IEnumerable<T> source)
        {
            return new HashSet<T>(source);
        }
        public static T Random<T>(this IReadOnlyList<T> collection)
        {
            int randomNumber = UnityRandom.Range(0, collection.Count);
            return collection[randomNumber];
        }
   
        public static IEnumerable<T> RandomizedRange<T>(this List<T> collection, int randomCount)
        {
            if (randomCount < 0) {
                throw new ArgumentOutOfRangeException(nameof(randomCount), "Random count is out of range, randomCount < 0");
            }
            if (randomCount > collection.Count) {
                throw new ArgumentOutOfRangeException(nameof(randomCount), "Random count is out of range, randomCount > collection.Count");
            }
            var random = new Random();
            var availableCount = collection.Count;
            var neededCount = randomCount;
            foreach (var item in collection)
            { 
                if (random.Next(availableCount) < neededCount)
                {
                    neededCount--;
                    yield return item;
                    if (neededCount == 0) {
                        break;
                    }
                }
                availableCount--;
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