using Survivors.Player.Inventory.Service;
using Survivors.Player.Progress.Service;
using Survivors.Player.Wallet;
using Survivors.Shop.Service;
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

            container.Bind<InventoryService>().AsSingle();
            container.Bind<InventoryRepository>().AsSingle();       
            
            container.Bind<UpgradeShopService>().AsSingle();
        }
    }
}
