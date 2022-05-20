using SuperMaxim.Messaging;
using Survivors.EnemySpawn;
using Survivors.Location;
using Survivors.Units;
using Survivors.Units.Modifiers;
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
        private WorldServicesInstaller _worldServicesInstaller;
        [SerializeField]
        private EnemyWavesSpawner _enemyWavesSpawner;
        
        public override void InstallBindings()
        {
            Container.BindInterfacesTo<MainSceneMonoInstaller>().FromInstance(this).AsSingle();
            Container.Bind<GameApplication>().FromInstance(_gameApplication).AsSingle();
            Container.Bind<IMessenger>().FromInstance(Messenger.Default).AsSingle();
            Container.Bind<Joystick>().FromInstance(_joystick).AsSingle();

            ConfigsInstaller.Install(Container);
            UnitServicesInstaller.Install(Container);

            _worldServicesInstaller.Install(Container);
            Container.Bind<EnemyWavesSpawner>().FromInstance(_enemyWavesSpawner);
            ModifiersInstaller.Install(Container);
        }
    }
}