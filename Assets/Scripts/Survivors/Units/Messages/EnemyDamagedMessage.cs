namespace Survivors.Units.Messages
{
    public readonly struct EnemyDamagedMessage
    {
        public readonly Unit Unit;
        public readonly float Damage;

        public EnemyDamagedMessage(Unit unit, float damage)
        {
            Unit = unit;
            Damage = damage;
        }
    }
}