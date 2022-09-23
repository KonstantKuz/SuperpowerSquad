using Survivors.Scope;

namespace Survivors.Enemy.Spawn.Spawners
{
    public interface IEnemySpawner
    {
        void Init(IScopeUpdatable scopeUpdatable);
        void StartSpawn();
    }
}