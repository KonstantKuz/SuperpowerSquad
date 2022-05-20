using System;
using System.Linq;
using Feofun.Config;
using Survivors.Location;
using Survivors.Location.Service;
using Survivors.Loot.Config;
using Survivors.Session;
using Survivors.Squad.Service;
using Survivors.Units;
using Survivors.Units.Service;
using Zenject;
using Random = UnityEngine.Random;

namespace Survivors.Loot.Service
{
    public class DroppingLootService : IWorldCleanUp
    {
        [Inject] private World _world;
        [Inject] private SquadUpgradeService _squadUpgradeService;
        [Inject] private UnitService _unitService;
        [Inject] private WorldObjectFactory _worldObjectFactory;
        [Inject] private StringKeyedConfigCollection<DroppingLootConfig> _droppingLoots;
        
        public void Init()
        {
            _unitService.OnEnemyUnitDeath += TrySpawnLoot;
        }

        public void TrySpawnLoot(IUnit unit)
        {
            var lootConfig = _droppingLoots.Values.FirstOrDefault(it => it.EnemyId == unit.Model.Id);
            var dropChance = lootConfig.DropChance;

            if (Random.value > dropChance)
            {
                return;
            }
            
            var lootId = lootConfig.Id;
            var loot = _worldObjectFactory.CreateObject(lootId, _world.SpawnContainer).GetComponent<DroppingLoot>();
            loot.transform.position = unit.GameObject.transform.position;
            loot.Init(lootConfig);
        }

        public void OnLootCollected(DroppingLootConfig collectedLoot)
        {
            switch (collectedLoot.Type)
            {
                case DroppingLootType.Exp:
                    _squadUpgradeService.AddExp(collectedLoot.Amount);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
        
        public void OnWorldCleanUp()
        {
            _unitService.OnEnemyUnitDeath -= TrySpawnLoot;
        }
    }
}
