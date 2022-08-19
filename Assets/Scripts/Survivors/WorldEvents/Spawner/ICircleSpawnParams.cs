namespace Survivors.WorldEvents.Spawner
{
    public interface ICircleSpawnParams
    {
        int InitialSpawnCountOnCircle { get; }
        int SpawnCountIncrementStepOnCircle { get; }
        float InitialSpawnDistance { get; }
        float SpawnDistanceStep { get; }
        float MaxSpawnDistance { get; set; }
    }
}