namespace Survivors.WorldEvents.Spawner
{
    public interface ICircleSpawnParams
    {
        int InitialSpawnCount { get; }
        int SpawnCountIncrementStep { get; }
        float InitialSpawnDistance { get; }
        float SpawnDistanceStep { get; }
        float MaxSpawnDistance { get; set; }
    }
}