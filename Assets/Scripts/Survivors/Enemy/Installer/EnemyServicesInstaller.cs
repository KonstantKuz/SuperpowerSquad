using Survivors.Enemy.Spawn.Service;
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
            container.Bind<TimedEnemySpawner>().AsSingle();
            container.Bind<EnemyHpsSpawner>().AsSingle();     
            container.Bind<BossSpawner>().AsSingle();      
            container.Bind<SeparatedWavesSpawner>().AsSingle();      
            
            container.Bind<EnemyWaves>().AsSingle();
        }
        
    }
}