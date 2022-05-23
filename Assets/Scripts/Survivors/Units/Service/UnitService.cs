using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;

namespace Survivors.Units.Service
{
    [PublicAPI]
    public class UnitService
    {
        private readonly Dictionary<UnitType, HashSet<IUnit>> _units = new Dictionary<UnitType, HashSet<IUnit>>();
        public event Action<IUnit> OnPlayerUnitDeath;
        public event Action<IUnit> OnEnemyUnitDeath;

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
        public bool HasUnitOfType(UnitType unitType) => _units.ContainsKey(unitType) && _units[unitType].Any();

        private void OnDeathUnit(IUnit unit)
        {
            unit.OnDeath -= OnDeathUnit;
            Remove(unit);
            if (unit.UnitType == UnitType.PLAYER) {
                OnPlayerUnitDeath?.Invoke(unit);
            } else {
                OnEnemyUnitDeath?.Invoke(unit);
            }
        }
    }
}