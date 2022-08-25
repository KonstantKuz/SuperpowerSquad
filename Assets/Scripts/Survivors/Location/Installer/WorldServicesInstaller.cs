using Survivors.Enemy.Spawn;
using Survivors.Location.ObjectFactory;
using Survivors.Loot.Service;
using Survivors.ObjectPool.Service;
using Survivors.Session.Service;
using UnityEngine;
using Zenject;

namespace Survivors.Location.Installer
{
    public class WorldServicesInstaller : MonoBehaviour
    {
        [SerializeField] private World _world; 
        [SerializeField] private ObjectInstancingFactory _objectInstancingFactory;
        [SerializeField] private EnemyWavesSpawner _enemyWavesSpawner;
        [SerializeField] private EnemyHpsSpawner _enemyHpsSpawner;
        [SerializeField] private PoolManager _poolManager;

        public void Install(DiContainer container)
        {
            container.Bind<ObjectResourceService>().AsSingle();
            
            container.Bind<IObjectFactory>().WithId(ObjectFactoryType.Instancing).To<ObjectInstancingFactory>()
                     .FromInstance(_objectInstancingFactory).AsSingle();
            container.Bind<IObjectFactory>().WithId(ObjectFactoryType.Pool).To<ObjectPoolFactory>().AsSingle();
            container.BindInterfacesAndSelfTo<WorldObjectRemover>().AsSingle();
            container.Bind<PoolManager>().FromInstance(_poolManager).AsSingle();


            container.Bind<World>().FromInstance(_world);

            container.BindInterfacesAndSelfTo<SessionService>().AsSingle();
            container.Bind<SessionRepository>().AsSingle();
            container.BindInterfacesAndSelfTo<ReviveService>().AsSingle();


            container.Bind<EnemyWavesSpawner>().FromInstance(_enemyWavesSpawner);
            container.Bind<EnemyHpsSpawner>().FromInstance(_enemyHpsSpawner).AsSingle();
            container.BindInterfacesAndSelfTo<DroppingLootService>().AsSingle();
        }
    }
}