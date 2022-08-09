using Survivors.Enemy.Spawn;
using Survivors.Location.Service;
using Survivors.Loot.Service;
using Survivors.ObjectPool.Service;
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
        [SerializeField] private PoolManager _poolManager;
        public void Install(DiContainer container)
        {
            _worldObjectFactory.Init();
            container.BindInterfacesAndSelfTo<WorldObjectFactory>().FromInstance(_worldObjectFactory).AsSingle();
            container.Bind<PoolManager>().FromInstance(_poolManager).AsSingle();
            
            container.Bind<World>().FromInstance(_world);
            
            container.BindInterfacesAndSelfTo<SessionService>().AsSingle();
            container.BindInterfacesAndSelfTo<ReviveService>().AsSingle();
            container.Bind<SessionRepository>().AsSingle();
            
            container.Bind<EnemyWavesSpawner>().FromInstance(_enemyWavesSpawner);
            container.Bind<EnemyHpsSpawner>().FromInstance(_enemyHpsSpawner).AsSingle();
            container.BindInterfacesAndSelfTo<DroppingLootService>().AsSingle();
        }
    }
}