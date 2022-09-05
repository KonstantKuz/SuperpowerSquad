using System;
using System.Linq;
using Feofun.Config;
using Logger.Extension;
using Survivors.Location;
using Survivors.Location.ObjectFactory;
using Survivors.Location.ObjectFactory.Factories;
using Survivors.Loot.Config;
using Survivors.Squad.Service;
using Survivors.Units;
using Survivors.Units.Service;
using UnityEditor;
using Zenject;
using Random = UnityEngine.Random;

namespace Survivors.Loot.Service
{
    public class DroppingLootService : IWorldScope
    {
        [Inject] private World _world;
        [Inject] private SquadProgressService _squadProgressService;
        [Inject] private UnitService _unitService;
        [Inject(Id = ObjectFactoryType.Pool)] 
        private IObjectFactory _objectFactory;
        [Inject] private StringKeyedConfigCollection<LootEmitterConfig> _lootEmitters;

        public void OnWorldSetup()
        {
            _unitService.OnEnemyUnitDeath += TrySpawnLoot;
        }

        private void TrySpawnLoot(IUnit unit, DeathCause deathCause)
        {
            if (deathCause != DeathCause.Killed) return;
            
            var emitterConfig = _lootEmitters.Values.FirstOrDefault(it => it.EmitterId == unit.Model.Id);
            if (emitterConfig == null)
            {
                this.Logger().Warn($"There is no loot config for enemy with id {unit.Model.Id}.");
                return;
            }

            var lootConfig = emitterConfig.LootConfig;
            var dropChance = lootConfig.DropChance;

            if (Random.value > dropChance)
            {
                return;
            }
            
            var loot = _objectFactory.Create<DroppingLoot>(lootConfig.LootId, _world.Spawn.transform);
            loot.transform.position = unit.GameObject.transform.position;
            loot.Init(lootConfig);
        }

        public void OnLootCollected(DroppingLootType lootType, DroppingLootConfig collectedLoot)
        {
            switch (lootType)
            {
                case DroppingLootType.Exp:
                    _squadProgressService.AddExp(collectedLoot.Amount);
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
