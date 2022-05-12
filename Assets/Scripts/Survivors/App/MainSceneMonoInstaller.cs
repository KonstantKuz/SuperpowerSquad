using Feofun.Config;
using Feofun.Config.Serializers;
using Feofun.Localization.Config;
using SuperMaxim.Messaging;
using Survivors.Config;
using Survivors.EnemySpawn;
using Survivors.Location;
using Survivors.Location.Service;
using Survivors.Session;
using Survivors.Units;
using Survivors.Units.Service;
using UnityEngine;
using Zenject;

namespace Survivors.App
{
    public class MainSceneMonoInstaller : MonoInstaller
    {
        [SerializeField]
        private GameApplication _gameApplication;
        [SerializeField]
        private Joystick _joystick;
        [SerializeField] 
        private LocationWorld _locationWorld;
        [SerializeField]
        private LocationObjectFactory _locationObjectFactory;
        [SerializeField]
        private EnemyWavesSpawner _enemyWavesSpawner;
        
        public override void InstallBindings()
        {
            Container.BindInterfacesTo<MainSceneMonoInstaller>().FromInstance(this).AsSingle();
            Container.Bind<GameApplication>().FromInstance(_gameApplication).AsSingle();
            Container.Bind<IMessenger>().FromInstance(Messenger.Default).AsSingle();
            Container.Bind<Joystick>().FromInstance(_joystick).AsSingle();

            ConfigsInstaller.Install(Container);
            LocationServicesInstaller.Install(Container, _locationObjectFactory, _locationWorld);
            EnemyServicesInstaller.Install(Container, _enemyWavesSpawner);
            UnitServicesInstaller.Install(Container);            
        }
    }
}