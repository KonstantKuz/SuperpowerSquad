using Zenject;

namespace Survivors.Player.Inventory.Service
{
    public class InventoryService
    {
        [Inject]
        private InventoryRepository _repository;

        public Model.Inventory Inventory => _repository.Get() ?? new Model.Inventory();
          
        public void AddUpgrade(string upgradeId)
        {
            var inventory = Inventory;
            inventory.AddUpgrade(upgradeId);
            Set(inventory);
        }
        private void Set(Model.Inventory model)
        {
            _repository.Set(model);
        }

    }
}