using Feofun.Localization.Service;
using SuperMaxim.Messaging;
using Survivors.Location;
using Survivors.Squad;
using Survivors.UI;
using Survivors.Units.Installer;
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
        private WorldServicesInstaller _worldServicesInstaller;  
        [SerializeField]
        private UIInstaller _uiInstaller;
     
        public override void InstallBindings()
        {
            Container.BindInterfacesTo<MainSceneMonoInstaller>().FromInstance(this).AsSingle();
            Container.Bind<GameApplication>().FromInstance(_gameApplication).AsSingle();
            Container.Bind<IMessenger>().FromInstance(Messenger.Default).AsSingle();     
            Container.Bind<LocalizationService>().AsSingle();


            ConfigsInstaller.Install(Container);
            UnitServicesInstaller.Install(Container);
            SquadServicesInstaller.Install(Container);
            _worldServicesInstaller.Install(Container);
            _uiInstaller.Install(Container);
            ModifiersInstaller.Install(Container);
        }
    }
}