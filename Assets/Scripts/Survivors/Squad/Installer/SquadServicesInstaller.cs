using Survivors.Squad.Progress;
using Survivors.Squad.Service;
using Survivors.Upgrade;
using Survivors.Upgrade.MetaUpgrade;
using Survivors.Upgrade.UpgradeSelection;
using Zenject;

namespace Survivors.Squad.Installer
{
    public class SquadServicesInstaller
    {
        public static void Install(DiContainer container)
        {
            container.Bind<SquadFactory>().AsSingle();
            container.BindInterfacesAndSelfTo<SquadProgressService>().AsSingle();
            container.Bind<SquadProgressRepository>().AsSingle();       
            
            
            container.BindInterfacesAndSelfTo<UpgradeService>().AsSingle();
            container.Bind<SquadUpgradeRepository>().AsSingle();  
            
            container.BindInterfacesAndSelfTo<UpgradeSelectionService>().AsSingle();           
            
            container.BindInterfacesAndSelfTo<MetaUpgradeService>().AsSingle(); 
            container.Bind<MetaUpgradeRepository>().AsSingle();
     
        }
    }
}
