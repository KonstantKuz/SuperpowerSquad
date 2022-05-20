﻿using Survivors.EnemySpawn;
using Survivors.Location.Service;
using Survivors.Session;
using UnityEngine;
using Zenject;

namespace Survivors.Location
{
    public class WorldServicesInstaller : MonoBehaviour
    {
        [SerializeField] private World _world;
        [SerializeField] private WorldObjectFactory _worldObjectFactory;
        [SerializeField] private EnemyWavesSpawner _enemyWavesSpawner;
        public void Install(DiContainer container)
        {
            _worldObjectFactory.Init();
            container.BindInterfacesAndSelfTo<WorldObjectFactory>().FromInstance(_worldObjectFactory).AsSingle();
            container.Bind<World>().FromInstance(_world);
            container.BindInterfacesAndSelfTo<SessionService>().AsSingle();
            container.BindInterfacesAndSelfTo<EnemyWavesSpawner>().FromInstance(_enemyWavesSpawner);
        }
    }
}