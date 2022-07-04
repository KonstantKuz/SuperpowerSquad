using Feofun.Repository;

namespace Survivors.Player.Inventory.Service
{
    public class InventoryRepository : LocalPrefsSingleRepository<Model.Inventory>
    {
        protected InventoryRepository() : base("Inventory_v1")
        {
        }
    }
}