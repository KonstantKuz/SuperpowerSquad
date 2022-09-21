using Survivors.Location.Model;
using Survivors.Loot.Config;
using Survivors.Loot.Service;
using UnityEngine;
using Zenject;

namespace Survivors.Loot
{
    public class DroppingLoot : WorldObject
    {
        [SerializeField] private DroppingLootType _lootType;

        public DroppingLootType LootType => _lootType;
        public DroppingLootConfig Config { get; private set; }

        [Inject] private DroppingLootService _droppingLootService;
        
        public void Init(DroppingLootConfig config)
        {
            Config = config;
            SetRandomRotation();
        }

        private void SetRandomRotation()
        {
            transform.localRotation = Quaternion.Euler(0, Random.Range(0f, 360f), 0);
        }
    }
}