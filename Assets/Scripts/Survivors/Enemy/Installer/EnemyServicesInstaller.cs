using Survivors.Enemy.Spawn;
using Survivors.Enemy.Spawn.Spawners;
using UnityEngine;
using Zenject;

namespace Survivors.Enemy.Installer
{
    public class EnemyServicesInstaller : MonoBehaviour
    {

        [SerializeField] private EnemyWaveSpawner _enemyWaveSpawner;
        
        public void Install(DiContainer container)
        {
           
            container.BindInterfacesAndSelfTo<EnemySpawnService>().AsSingle();
            container.Bind<EnemyWaveSpawner>().FromInstance(_enemyWaveSpawner).AsSingle();
            container.Bind<TimeEnemySpawner>().AsSingle();
            container.Bind<EnemyHpsSpawner>().AsSingle();     
            container.Bind<BossSpawner>().AsSingle();
        }
        
    }
}