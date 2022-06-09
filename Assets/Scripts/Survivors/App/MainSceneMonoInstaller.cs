using Feofun.Localization.Service;
using SuperMaxim.Messaging;
using Survivors.Analytics;
using Survivors.Location;
using Survivors.Modifiers;
using Survivors.Squad.Installer;
using Survivors.UI;
using Survivors.Units.Installer;
using UnityEngine;
using Zenject;

namespace Survivors.App
{
    public class MainSceneMonoInstaller : MonoInstaller
    {
        [SerializeField]
        private GameApplication _gameApplication;
        [SerializeField]
        private WorldServicesInstaller _worldServicesInstaller;  
        [SerializeField]
        private UIInstaller _uiInstaller;
     
        public override void InstallBindings()
        {
            AnalyticsInstaller.Install(Container);
            Container.BindInterfacesTo<MainSceneMonoInstaller>().FromInstance(this).AsSingle();
            Container.Bind<GameApplication>().FromInstance(_gameApplication).AsSingle();
            Container.Bind<IMessenger>().FromInstance(Messenger.Default).AsSingle();     
            Container.Bind<LocalizationService>().AsSingle();


            ConfigsInstaller.Install(Container);
            ModifiersInstaller.Install(Container);  
            
            
            UnitServicesInstaller.Install(Container);
            SquadServicesInstaller.Install(Container);
            _worldServicesInstaller.Install(Container);
            _uiInstaller.Install(Container);
        }
    }
}