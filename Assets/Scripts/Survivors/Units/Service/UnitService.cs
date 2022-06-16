﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using SuperMaxim.Core.Extensions;

namespace Survivors.Units.Service
{
    [PublicAPI]
    public class UnitService
    {
        private readonly Dictionary<UnitType, HashSet<IUnit>> _units = new Dictionary<UnitType, HashSet<IUnit>>();
        public event Action<IUnit> OnPlayerUnitDeath;
        public event Action<IUnit, DeathCause> OnEnemyUnitDeath;

        public IEnumerable<IUnit> AllUnits => _units.SelectMany(it => it.Value);
        public void Add(IUnit unit)
        {
            if (!_units.ContainsKey(unit.UnitType)) {
                _units[unit.UnitType] = new HashSet<IUnit>();
            }
            _units[unit.UnitType].Add(unit);
            unit.OnDeath += OnDeathUnit;
        }
        public void Remove(IUnit unit)
        {
            _units[unit.UnitType].Remove(unit);
            unit.OnDeath -= OnDeathUnit;
        }
        public void DeactivateAll() => AllUnits.ForEach(u => { u.IsActive = false; });
        public bool HasUnitOfType(UnitType unitType) => _units.ContainsKey(unitType) && _units[unitType].Any();
        


        private void OnDeathUnit(IUnit unit, DeathCause deathCause)
        {
            unit.OnDeath -= OnDeathUnit;
            Remove(unit);
            if (unit.UnitType == UnitType.PLAYER) {
                OnPlayerUnitDeath?.Invoke(unit);
            } else {
                OnEnemyUnitDeath?.Invoke(unit, deathCause);
            }
        }

        public IEnumerable<IUnit> GetAllUnitsOfType(UnitType unitType) =>
            _units.ContainsKey(unitType) ? _units[unitType] : Enumerable.Empty<IUnit>();
    }
}