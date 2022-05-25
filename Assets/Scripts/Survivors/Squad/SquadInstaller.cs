using Survivors.Squad.Upgrade;
using Zenject;

namespace Survivors.Squad
{
    public static class SquadInstaller
    {
        public static void Install(DiContainer container)
        {
            container.Bind<UpgradeService>().AsSingle();    
        }
    }
}