using Feofun.Components;
using Feofun.Localization.Service;
using SuperMaxim.Messaging;
using Survivors.Advertisment.Installer;
using Survivors.Analytics;
using Survivors.App.Config;
using Survivors.Cheats.Installer;
using Survivors.Location;
using Survivors.Modifiers;
using Survivors.Player.Installer;
using Survivors.Reward.Installer;
using Survivors.Squad.Installer;
using Survivors.UI;
using Survivors.Units.Installer;
using UnityEngine;
using Zenject;

namespace Survivors.App.IoC
{
    public class MainSceneMonoInstaller : MonoInstaller
    {
        [SerializeField]
        private GameApplication _gameApplication;
        [SerializeField]
        private UpdateManager _updateManager;
        [SerializeField]
        private WorldServicesInstaller _worldServicesInstaller;  
        [SerializeField]
        private UIInstaller _uiInstaller;     
        [SerializeField]
        private CheatsInstaller _cheatsInstaller;

        public override void InstallBindings()
        {
            AnalyticsInstaller.Install(Container);
            
            Container.BindInterfacesTo<MainSceneMonoInstaller>().FromInstance(this).AsSingle();
            Container.BindInterfacesAndSelfTo<GameApplication>().FromInstance(_gameApplication).AsSingle();
            Container.Bind<UpdateManager>().FromInstance(_updateManager).AsSingle();
            Container.Bind<IMessenger>().FromInstance(Messenger.Default).AsSingle();     
            Container.Bind<LocalizationService>().AsSingle();
            Container.Bind<VibrationManager>().AsSingle();   
            Container.Bind<ABTest.ABTest>().AsSingle();
            

            ConfigsInstaller.Install(Container);
            ModifiersInstaller.Install(Container);  
            
            UnitServicesInstaller.Install(Container);
            SquadServicesInstaller.Install(Container);
            PlayerServicesInstaller.Install(Container);
            RewardServicesInstaller.Install(Container); 
            AdsServicesInstaller.Install(Container);
            
            _worldServicesInstaller.Install(Container);
            _uiInstaller.Install(Container);
            _cheatsInstaller.Install(Container);
            
        }
    }
}