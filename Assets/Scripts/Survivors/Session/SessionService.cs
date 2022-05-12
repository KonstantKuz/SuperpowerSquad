using Survivors.EnemySpawn;
using Survivors.EnemySpawn.Config;
using Zenject;

namespace Survivors.Session
{
    public class SessionService
    {
        [Inject] private EnemyWavesSpawner _enemyWavesSpawner;
        [Inject] private MatchEnemyWavesConfig _matchEnemyWavesConfig;

        public void Start()
        {
            _enemyWavesSpawner.Init(_matchEnemyWavesConfig);
        }
    }
}