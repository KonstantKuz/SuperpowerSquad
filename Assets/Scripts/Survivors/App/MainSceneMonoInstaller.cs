using Feofun.Config;
using Feofun.Config.Serializers;
using Feofun.Localization.Config;
using SuperMaxim.Messaging;
using Survivors.Config;
using Survivors.Location;
using Survivors.Location.EnemySpawn;
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

            RegisterConfigs(Container);
            RegisterLocation();
            RegisterUnitServices();
            RegisterEnemyServices();
            UnitServicesInstaller.Install(Container);            
        }

        private void RegisterLocation()
        {
            _locationObjectFactory.Init();
            Container.Bind<LocationObjectFactory>().FromInstance(_locationObjectFactory).AsSingle();
            Container.Bind<LocationWorld>().FromInstance(_locationWorld);
            Container.Bind<SessionService>().AsSingle();
        }

        private void RegisterUnitServices()
        {
            Container.Bind<UnitFactory>().AsSingle();
        }

        private void RegisterEnemyServices()
        {
            Container.Bind<EnemyWavesSpawner>().FromInstance(_enemyWavesSpawner);
        }

        private static void RegisterConfigs(DiContainer container)
        {
            new ConfigLoader(container, new CsvConfigDeserializer())
                .RegisterSingle<LocalizationConfig>(Configs.LOCALIZATION)
                .RegisterSingle<MatchEnemyWavesConfig>(Configs.ENEMY_WAVES);
        }
    }
}