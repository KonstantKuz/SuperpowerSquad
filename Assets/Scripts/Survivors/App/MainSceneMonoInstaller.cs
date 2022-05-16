using Feofun.Config;
using Feofun.Config.Serializers;
using Feofun.Localization.Config;
using SuperMaxim.Messaging;
using Survivors.Config;
using Survivors.GameWorld;
using Survivors.Units.Installer;
using Survivors.Units.Player.Config;
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


        public override void InstallBindings()
        {
            Container.BindInterfacesTo<MainSceneMonoInstaller>().FromInstance(this).AsSingle();
            Container.Bind<GameApplication>().FromInstance(_gameApplication).AsSingle();
            Container.Bind<IMessenger>().FromInstance(Messenger.Default).AsSingle();
            Container.Bind<Joystick>().FromInstance(_joystick).AsSingle();

            RegisterConfigs(Container);
            _worldServicesInstaller.Install(Container);
            UnitServicesInstaller.Install(Container);    

        }

        private static void RegisterConfigs(DiContainer container)
        {
            new ConfigLoader(container, new CsvConfigDeserializer())
                    .RegisterSingle<LocalizationConfig>(Configs.LOCALIZATION)
                    .RegisterStringKeyedCollection<PlayerUnitConfig>(Configs.PLAYER_UNIT);
        }
    }
}