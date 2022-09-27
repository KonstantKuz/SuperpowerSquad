namespace Survivors.Units.Messages
{
    public readonly struct BossSpawnedMessage
    {
        public readonly IUnit Unit;

        public BossSpawnedMessage(IUnit unit)
        {
            Unit = unit;
        }
    }
}