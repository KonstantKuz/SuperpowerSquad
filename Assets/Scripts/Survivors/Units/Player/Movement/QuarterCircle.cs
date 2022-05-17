using System;

namespace Survivors.Units.Player.Movement
{
    public enum QuarterCircle
    {
        First,
        Second,
        Third,
        Fourth
    }

    public static class QuarterCircleExt
    {
        public static QuarterCircle GetQuarterCircle(float signedAngle)
        {
            if (signedAngle >= -90 && signedAngle <= 0) {
                return QuarterCircle.First;
            }
            if (signedAngle <= 90 && signedAngle >= 0) {
                return QuarterCircle.Second;
            }
            if (signedAngle >= 90 && signedAngle <= 180) {
                return QuarterCircle.Third;
            }
            if (signedAngle <= -90 && signedAngle >= -180) {
                return QuarterCircle.Fourth;
            }
            throw new ArgumentException("Unexpected quarter circle");
        }
    }
}