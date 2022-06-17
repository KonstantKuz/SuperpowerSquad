namespace Survivors.Units.Messages
{
    public readonly struct UnitSpawnedMessage
    {
        public readonly IUnit Unit;

        public UnitSpawnedMessage(IUnit unit)
        {
            Unit = unit;
        }
    }
}