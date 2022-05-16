using Survivors.EnemySpawn;
using Survivors.EnemySpawn.Config;
using Zenject;

namespace Survivors.Session
{
    public class SessionService
    {
        [Inject] private EnemyWavesSpawner _enemyWavesSpawner;
        [Inject] private EnemyWavesConfig _enemyWavesConfig;

        public void Start()
        {
            _enemyWavesSpawner.StartSpawn(_enemyWavesConfig);
        }
    }
}