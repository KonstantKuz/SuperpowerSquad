using Survivors.Enemy.Spawn;
using Survivors.Enemy.Spawn.Spawners;
using UnityEngine;
using Zenject;

namespace Survivors.Enemy.Installer
{
    public class EnemyServicesInstaller : MonoBehaviour
    {

        [SerializeField] private EnemyWavesSpawner _enemyWavesSpawner;
        
        public void Install(DiContainer container)
        {
           
            container.BindInterfacesAndSelfTo<EnemySpawnService>().AsSingle();
            container.Bind<EnemyWavesSpawner>().FromInstance(_enemyWavesSpawner);
            container.Bind<EnemyHpsSpawner>().AsSingle();     
            container.Bind<BossSpawner>().AsSingle();
        }
        
    }
}