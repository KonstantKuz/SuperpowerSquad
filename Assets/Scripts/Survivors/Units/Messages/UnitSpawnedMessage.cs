namespace Survivors.Units.Messages
{
    public struct UnitSpawnedMessage
    {
        public IUnit Unit;

        public UnitSpawnedMessage(IUnit unit)
        {
            Unit = unit;
        }
    }
}