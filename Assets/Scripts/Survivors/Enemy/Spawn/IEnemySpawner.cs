using Survivors.Scope;

namespace Survivors.Enemy.Spawn
{
    public interface IEnemySpawner
    {
        void Init(IScopeUpdatable scopeUpdatable);
        void StartSpawn();
    }
}