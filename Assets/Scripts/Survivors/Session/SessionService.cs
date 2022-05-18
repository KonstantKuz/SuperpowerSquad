using Survivors.EnemySpawn;
using Survivors.EnemySpawn.Config;
using Survivors.Units.Service;
using Zenject;

namespace Survivors.Session
{
    public class SessionService
    {
        [Inject] private EnemyWavesSpawner _enemyWavesSpawner;
        [Inject] private EnemyWavesConfig _enemyWavesConfig;
        [Inject] private UnitFactory _unitFactory;
        public void Start()
        {
            _unitFactory.LoadPlayerUnit(UnitFactory.SIMPLE_PLAYER_ID); 
            _enemyWavesSpawner.StartSpawn(_enemyWavesConfig);
        }

        public void Term()
        {
            
        }
    }
}