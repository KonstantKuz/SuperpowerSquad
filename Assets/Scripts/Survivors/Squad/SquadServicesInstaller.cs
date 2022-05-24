using Survivors.Squad.Progress;
using Survivors.Squad.Service;
using UnityEngine;
using Zenject;

namespace Survivors.Squad
{
    public class SquadServicesInstaller
    {
        public static void Install(DiContainer container)
        {
            container.Bind<SquadUpgradeService>().AsSingle();
            container.Bind<SquadProgressRepository>().AsSingle();
        }
    }
}
