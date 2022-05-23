namespace Survivors.Extension
{
    public static class MathExtensions
    {
        public static float Remap(this float value, float originRangeX, float originRangeY, float targetRangeX, float targetRangeY) 
        {
            value = (value - originRangeX) / (originRangeY - originRangeX) * (targetRangeY - targetRangeX) + targetRangeX;
            return value;
        }
    }
}