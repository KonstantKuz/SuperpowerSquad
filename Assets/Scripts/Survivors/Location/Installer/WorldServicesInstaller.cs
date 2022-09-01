﻿using Survivors.Enemy.Spawn;
using Survivors.Location.ObjectFactory;
using Survivors.Location.ObjectFactory.Factories;
using Survivors.Location.Service;
using Survivors.Loot.Service;
using Survivors.Session.Service;
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
        }

        private void InstallObjectFactory(DiContainer container)
        {
            container.Bind<ObjectInstancingFactory>()
                     .FromInstance(_objectInstancingFactory)
                     .AsSingle();
            container.Bind<ObjectPoolFactory>().AsSingle();

            container.Bind<IObjectFactory>()
                     .WithId(ObjectFactoryType.Instancing)
                     .To<ObjectInstancingFactory>()
                     .FromResolve();   
            container.Bind<IObjectFactory>()
                     .WithId(ObjectFactoryType.Pool)
                     .To<ObjectPoolFactory>()
                     .FromResolve();



        }
    }
}