using Zenject;

namespace Survivors.Player.Inventory.Service
{
    public class InventoryService
    {
        [Inject]
        private InventoryRepository _repository;

        public Model.Inventory Inventory
        {
            get
            {
                if (!_repository.Exists()) {
                    _repository.Set(new Model.Inventory());
                }
                return _repository.Get();
            }
        }
        public void AddUpgrade(string upgradeId)
        {
            var inventory = Inventory;
            inventory.UnitsUpgrades.AddUpgrade(upgradeId);
            Set(inventory);
        }

        private void Set(Model.Inventory model)
        {
            _repository.Set(model);
        }
    }
}