using Survivors.Scope;

namespace Survivors.Enemy.Spawn
{
    public interface IEnemySpawner
    {
        void Init(IUpdatableScope updatableScope);
        void StartSpawn();
    }
}