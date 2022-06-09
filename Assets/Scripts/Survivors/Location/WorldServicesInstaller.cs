using Survivors.EnemySpawn;
using Survivors.Location.Service;
using Survivors.Loot.Service;
using Survivors.Session.Service;
using UnityEngine;
using Zenject;

namespace Survivors.Location
{
    public class WorldServicesInstaller : MonoBehaviour
    {
        [SerializeField] private World _world;
        [SerializeField] private WorldObjectFactory _worldObjectFactory;
        [SerializeField] private EnemyWavesSpawner _enemyWavesSpawner;
        [SerializeField] private EnemyHpsSpawner _enemyHpsSpawner;
        
        public void Install(DiContainer container)
        {
            _worldObjectFactory.Init();
            container.BindInterfacesAndSelfTo<WorldObjectFactory>().FromInstance(_worldObjectFactory).AsSingle();
            container.Bind<World>().FromInstance(_world);
            
            container.BindInterfacesAndSelfTo<SessionService>().AsSingle();   
            container.Bind<SessionRepository>().AsSingle();
            
            container.BindInterfacesAndSelfTo<EnemyWavesSpawner>().FromInstance(_enemyWavesSpawner);
            container.BindInterfacesAndSelfTo<DroppingLootService>().AsSingle();
            container.BindInterfacesAndSelfTo<EnemyHpsSpawner>().FromInstance(_enemyHpsSpawner).AsSingle();
        }
    }
}