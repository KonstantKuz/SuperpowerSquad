using Survivors.Squad.Progress;
using Survivors.Squad.Service;
using Survivors.Squad.Upgrade;
using Zenject;

namespace Survivors.Squad.Installer
{
    public class SquadServicesInstaller
    {
        public static void Install(DiContainer container)
        {

            container.BindInterfacesAndSelfTo<SquadProgressService>().AsSingle();
            container.BindInterfacesAndSelfTo<UpgradeService>().AsSingle();
            //container.BindInterfacesAndSelfTo<UpgradeSelectionService>().AsSingle();   
            
            container.Bind<SquadProgressRepository>().AsSingle();
            container.Bind<SquadUpgradeRepository>().AsSingle();  
        }
    }
}
