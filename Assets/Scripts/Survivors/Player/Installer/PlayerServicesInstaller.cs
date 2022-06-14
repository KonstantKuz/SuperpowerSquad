using Survivors.Player.Service;
using Zenject;

namespace Survivors.Player.Installer
{
    public class PlayerServicesInstaller
    {
        public static void Install(DiContainer container)
        {
            container.Bind<PlayerProgressService>().AsSingle();
            container.Bind<PlayerProgressRepository>().AsSingle();
        }
    }
}
