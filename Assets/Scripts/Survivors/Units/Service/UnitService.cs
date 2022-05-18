using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using ModestTree;
using SuperMaxim.Core.Extensions;
using Survivors.Units.Target;

namespace Survivors.Units.Service
{
    [PublicAPI]
    public class UnitService 
    {
        private readonly Dictionary<UnitType, HashSet<IUnit>> _targets = new Dictionary<UnitType, HashSet<IUnit>>();
        
        public event Action<Unit> OnUnitDeath;

        public List<Unit> InitialUnits => _initialUnits;
        public List<DeadUnitAtCell> DeadUnitsPositions => _deadUnitsPositions;

        public IReadOnlyList<Unit> Units
        {
            get
            {
                CheckInitialized(); 
                return _units;
            }
        }
        public void Init()
        {
            _units = _locationObjectFactory.GetObjectComponents<Unit>();
            _initialUnits = _units;
            _deadUnitsPositions = new List<DeadUnitAtCell>();
            
            if (AppConfig.FactionEnabled) {
                _factionService.ApplyFactionModifiers(_units);
            }
            _units.ForEach(u => {
                u.IsAlive = true;
                u.OnDeath += OnDeathUnit;
            });
        }
        public void Term()
        {
            DisableReceiveDamage();
            Units.ForEach(u => u.OnDeath -= OnDeathUnit);
            _units = null;
        }
        private void CheckInitialized() => Assert.IsNotNull(_units, "UnitService is not initialized");
        public bool AllAliveUnitsHaveType(UnitType unitType) => Units.All(t => t.UnitType == unitType);
        private void DisableReceiveDamage() => Units.ForEach(u => u.SetDamageReceiveEnabled(false));
        
        private void OnDeathUnit(Unit unit)
        {
            SavePosition(unit);
            unit.OnDeath -= OnDeathUnit;
            _units.Remove(unit);
            OnUnitDeath?.Invoke(unit);
        }
        
        private void SavePosition(Unit deadUnit)
        {
            var unitAtCell = new DeadUnitAtCell
            {
                UnitType = deadUnit.UnitType,
                UnitModel = deadUnit.UnitModel,
                CellId = _gridPositionProvider.GetCellByPos(deadUnit.transform.position),
            };
            _deadUnitsPositions.Add(unitAtCell);
        }
    }
}