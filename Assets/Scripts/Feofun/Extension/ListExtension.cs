using System;
using System.Collections.Generic;
using System.Linq;
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
        /// <summary>
        /// The algorithm is taken from: https://stackoverflow.com/questions/48087/select-n-random-elements-from-a-listt-in-c-sharp
        /// </summary>
        public static IEnumerable<T> SelectRandomElements<T>(this List<T> collection, int randomCount)
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

        public static T SelectRandomWithChance<T>(this IReadOnlyList<Tuple<T,float>> collection)
        {
            var chanceSum = collection.Sum(it => it.Item2);
            var randomChance = UnityRandom.Range(0f, chanceSum);
            foreach (var tuple in collection) 
            {
                if (randomChance <= tuple.Item2)
                {
                    return tuple.Item1;
                }
                randomChance -= tuple.Item2;
            }
            
            throw new ArgumentException("Can't find suitable item.");
        }
    }
}