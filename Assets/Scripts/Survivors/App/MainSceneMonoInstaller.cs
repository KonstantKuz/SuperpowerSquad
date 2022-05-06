using Feofun.Config;
using Feofun.Config.Serializers;
using Feofun.Localization.Config;
using SuperMaxim.Messaging;
using Survivors.Config;
using UnityEngine;
using Zenject;

namespace Survivors.App
{
    public class MainSceneMonoInstaller : MonoInstaller
    {
        [SerializeField]
        private GameApplication _gameApplication;

        public override void InstallBindings()
        {
            Container.BindInterfacesTo<MainSceneMonoInstaller>().FromInstance(this).AsSingle();
            Container.Bind<GameApplication>().FromInstance(_gameApplication).AsSingle();
            Container.Bind<IMessenger>().FromInstance(Messenger.Default).AsSingle();

            RegisterConfigs(Container);
        }

        private static void RegisterConfigs(DiContainer container, string configOverrideFolder = null)
        {
            new ConfigLoader(container, new CsvConfigDeserializer(), configOverrideFolder)
                .RegisterSingle<LocalizationConfig>(Configs.LOCALIZATION);
        }
    }
}