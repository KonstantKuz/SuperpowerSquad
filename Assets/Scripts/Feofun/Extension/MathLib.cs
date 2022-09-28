namespace Feofun.Extension
{
    public static class MathLib
    {
        /// <summary>
        /// Returns the value in the range [targetRangeMin, targetRangeMax] equivalent to source value in the range [sourceRangeMin, sourceRangeMax].
        /// </summary>
        public static float Remap(float sourceValue, float sourceRangeMin, float sourceRangeMax, float targetRangeMin, float targetRangeMax) 
        {
            return (sourceValue - sourceRangeMin) / (sourceRangeMax - sourceRangeMin) * (targetRangeMax - targetRangeMin) + targetRangeMin;
        }
    }
}