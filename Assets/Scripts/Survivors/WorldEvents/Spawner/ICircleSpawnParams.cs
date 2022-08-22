namespace Survivors.WorldEvents.Spawner
{
    public interface ICircleSpawnParams
    {
        float SpawnStepOnPerimeter { get; }
        float InitialSpawnDistance { get; }
        float SpawnDistanceStep { get; }
        float MaxSpawnDistance { get; set; }
    }
}