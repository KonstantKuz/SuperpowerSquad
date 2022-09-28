namespace Survivors.Units.Messages
{
    public readonly struct UnitDamagedMessage
    {
        public readonly Unit Unit;
        public readonly float Damage;

        public UnitDamagedMessage(Unit unit, float damage)
        {
            Unit = unit;
            Damage = damage;
        }
    }
}