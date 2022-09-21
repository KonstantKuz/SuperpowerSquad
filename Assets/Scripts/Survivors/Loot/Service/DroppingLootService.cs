using System;
using System.Collections.Generic;
using System.Linq;
using Feofun.Extension;
using Logger.Extension;
using SuperMaxim.Core.Extensions;
using Survivors.Location;
using Survivors.Location.ObjectFactory;
using Survivors.Loot.Config;
using Survivors.Squad.Service;
using Survivors.Units;
using Survivors.Units.Service;
using Survivors.Units.Target;
using UnityEngine;
using Zenject;
using Random = UnityEngine.Random;

namespace Survivors.Loot.Service
{
    public class DroppingLootService : IWorldScope
    {
        private readonly ISet<DroppingLoot> _loots = new HashSet<DroppingLoot>();

        [Inject] private World _world;
        [Inject] private SquadProgressService _squadProgressService;
        [Inject] private UnitService _unitService;
        [Inject(Id = ObjectFactoryType.Pool)] 
        private IObjectFactory _objectFactory;
        [Inject] private LootConfig _lootConfig;

        public IEnumerable<DroppingLoot> AllLoot => _loots;

        public void OnWorldSetup()
        {
            _unitService.OnEnemyUnitDeath += TrySpawnLoot;
        }

        public void RemoveAll()
        {
            _loots.Clear();
        }

        private void TrySpawnLoot(IUnit unit, DeathCause deathCause)
        {
            if (deathCause != DeathCause.Killed) return;
            
            var possibleLoot = _lootConfig.FindPossibleLootsFor(unit.Model.Id);
            if (possibleLoot == null)
            {
                this.Logger().Trace($"There is no loot config for enemy with id {unit.Model.Id}.");
                return;
            }

            var configsWithChance = possibleLoot.Select(it => Tuple.Create(it, it.DropChance)).ToList();
            var loot = SpawnLoot(unit.SelfTarget.Root.position, configsWithChance.SelectRandomWithChance());
            _loots.Add(loot);
        }

        public void Remove(DroppingLoot loot)
        {
            _loots.Remove(loot);
        }

        private DroppingLoot SpawnLoot(Vector3 position, DroppingLootConfig config)
        {
            var loot = _objectFactory.Create<DroppingLoot>(config.LootId, _world.Spawn.transform);
            loot.transform.position = position;
            loot.Init(config);
            return loot;
        }
        
        public void OnLootCollected(DroppingLootType lootType, DroppingLootConfig collectedLoot)
        {
            switch (lootType)
            {
                case DroppingLootType.Exp:
                    _squadProgressService.AddExp(collectedLoot.Amount);
                    break;
                case DroppingLootType.Health:
                    _world.GetSquad().AddHealthPercent(collectedLoot.Amount);
                    break;
                case DroppingLootType.Magnet:
                    _world.GetSquad().CollectAllLoot();
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
