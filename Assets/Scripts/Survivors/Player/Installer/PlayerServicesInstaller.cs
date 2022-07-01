using Survivors.Player.Progress.Service;
using Survivors.Player.Wallet;
using Zenject;

namespace Survivors.Player.Installer
{
    public class PlayerServicesInstaller
    {
        public static void Install(DiContainer container)
        {
            container.Bind<PlayerProgressService>().AsSingle();
            container.Bind<PlayerProgressRepository>().AsSingle();    
            
            container.Bind<WalletService>().AsSingle();            
            container.Bind<WalletRepository>().AsSingle();
        }
    }
}
