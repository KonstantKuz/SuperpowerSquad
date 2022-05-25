using Survivors.Squad.Progress;
using Survivors.Squad.Service;
using Survivors.Squad.Upgrade;
using Survivors.Squad.UpgradeSelection;
using Zenject;

namespace Survivors.Squad
{
    public class SquadServicesInstaller
    {
        public static void Install(DiContainer container)
        {
            container.Bind<SquadProgressService>().AsSingle();
            container.Bind<SquadProgressRepository>().AsSingle();
            container.Bind<UpgradeService>().AsSingle();      
            container.Bind<SquadUpgradeRepository>().AsSingle();         
            container.Bind<UpgradeSelectionService>().AsSingle();            
        }
    }
}
