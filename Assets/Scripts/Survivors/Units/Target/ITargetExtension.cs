namespace Survivors.Units.Target
{
    public static class TargetExtension
    {
        public static bool IsTargetValidAndAlive(this ITarget target)
        {
            return target is { IsAlive: true };
        }
    }
}