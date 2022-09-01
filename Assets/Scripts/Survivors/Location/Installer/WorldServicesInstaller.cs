using Survivors.Enemy.Spawn;
using Survivors.Location.ObjectFactory;
using Survivors.Location.ObjectFactory.Factories;
using Survivors.Location.Service;
using Survivors.Loot.Service;
using Survivors.Session.Service;
using Survivors.Units.Component.FrustrumCulling;
using Survivors.WorldEvents.Service;
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
        [SerializeField] private WorldEventFactory _worldEventFactory;
        [SerializeField] private FrustrumCullingSystem _frustrumCullingSystem;
        
        public void Install(DiContainer container)
        {
            container.Bind<ObjectResourceService>().AsSingle();
            container.BindInterfacesAndSelfTo<WorldObjectRemover>().AsSingle();
            InstallObjectFactory(container);
      
            
            container.Bind<World>().FromInstance(_world);
            
            container.BindInterfacesAndSelfTo<SessionService>().AsSingle();     
            container.Bind<SessionRepository>().AsSingle();
            container.BindInterfacesAndSelfTo<ReviveService>().AsSingle();
            
            container.Bind<EnemyWavesSpawner>().FromInstance(_enemyWavesSpawner);
            container.Bind<EnemyHpsSpawner>().FromInstance(_enemyHpsSpawner).AsSingle();
            container.BindInterfacesAndSelfTo<DroppingLootService>().AsSingle();
            
            
            container.BindInterfacesAndSelfTo<WorldEventService>().AsSingle();  
            container.Bind<WorldEventFactory>().FromInstance(_worldEventFactory);
            container.Bind<FrustrumCullingSystem>().FromInstance(_frustrumCullingSystem);
        }

        private void InstallObjectFactory(DiContainer container)
        {
            container.Bind<IObjectFactory>()
                     .WithId(ObjectFactoryType.Instancing)
                     .To<ObjectInstancingFactory>()
                     .FromInstance(_objectInstancingFactory)
                     .AsSingle();
            
            container.Bind<IObjectFactory>()
                     .WithId(ObjectFactoryType.Pool)
                     .To<ObjectPoolFactory>()
                     .AsSingle();
            
        
        }
    }
}