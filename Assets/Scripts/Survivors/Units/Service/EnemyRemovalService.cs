using System.Collections.Generic;
using System.Linq;
using SuperMaxim.Messaging;
using Survivors.Location;
using Survivors.Units.Enemy.Model;
using Survivors.Units.Messages;
using UnityEngine;
using UnityEngine.Assertions;
using Zenject;

namespace Survivors.Units.Service
{
    public class EnemyRemovalService: MonoBehaviour, IWorldScope
    {
        private class UnitComparer : IComparer<Unit>
        {
            public int Compare(Unit x, Unit y)
            {
                if (x == null && y == null)
                {
                    return 0;
                }

                if (x == null)
                {
                    return -1;
                }

                if (y == null)
                {
                    return 1;
                }

                var result = x.LifeTime.CompareTo(y.LifeTime);
                return result == 0 ? x.GetInstanceID().CompareTo(y.GetInstanceID()) : result;
            }
        }

        [SerializeField] private int _softLimit;
        [SerializeField] private int _hardLimit;
        [SerializeField] private float _minRemovalAge;

        [Inject] private IMessenger _messenger;

        private int _lastSpawnedLevel = 1;
        private readonly SortedSet<Unit> _units = new SortedSet<Unit>(new UnitComparer());
        private bool _isWorldActive;

        public void OnWorldSetup()
        {
            _isWorldActive = true;
        }

        public void OnWorldCleanUp()
        {
            _isWorldActive = false;
        }

        private void Awake()
        {
            _messenger.Subscribe<UnitSpawnedMessage>(OnUnitSpawned);
        }

        private void OnDestroy()
        {
            _messenger.Unsubscribe<UnitSpawnedMessage>(OnUnitSpawned);
        }

        private void Update()
        {
            if (_isWorldActive) return;
            if (_units.Count <= _softLimit) return;
            
            var frustumPlanes = GeometryUtility.CalculateFrustumPlanes(UnityEngine.Camera.main);
            var candidatesFromNewestToOldest = GetCandidates(frustumPlanes);
            
            RemoveSoftWay(_units.Count - _softLimit, candidatesFromNewestToOldest);

            if (_units.Count <= _hardLimit) return;
            candidatesFromNewestToOldest = GetCandidates(frustumPlanes);

            RemoveHardWay(_units.Count - _hardLimit, candidatesFromNewestToOldest);
        }

        private void RemoveSoftWay(int removeCount, List<Unit> candidatesFromNewestToOldest)
        {
            for (int i = 0; i < removeCount; i++)
            {
                var first = candidatesFromNewestToOldest.Last();
                if (first == null) break;
                candidatesFromNewestToOldest.RemoveAt(candidatesFromNewestToOldest.Count - 1);
                var second = FindRemovalCandidate(candidatesFromNewestToOldest, first.Health.CurrentValue.Value);
                if (second == null) break;
                Merge(first, second);
            }
        }

        private void RemoveHardWay(int removeCount, List<Unit> candidatesFromNewestToOldest)
        {
            for (int i = 0; i < removeCount; i++)
            {
                var unit = candidatesFromNewestToOldest.Last();
                if (unit == null) break;
                candidatesFromNewestToOldest.RemoveAt(candidatesFromNewestToOldest.Count - 1);
                unit.Kill(DeathCause.Removed);
            }
        }

        private List<Unit> GetCandidates(Plane[] frustumPlanes)
        {
            var candidates = new List<Unit>(_units.Count);
            foreach (var unit in _units.Reverse())
            {
                if (unit.LifeTime < _minRemovalAge) continue;
                if (IsVisible(unit, frustumPlanes)) continue;
                candidates.Add(unit);
            }

            return candidates;
        }

        private Unit FindRemovalCandidate(List<Unit> candidatesFromNewestToOldest, float health)
        {
            for (int idx = candidatesFromNewestToOldest.Count - 1; idx >=0; idx--)
            {
                var unit = candidatesFromNewestToOldest[idx];
                var enemyModel = unit.Model as EnemyUnitModel;
                Assert.IsNotNull(unit.Health);
                var sumLevel = enemyModel.CalculateLevelOfHealth(unit.Health.CurrentValue.Value + health);
                if (sumLevel <= _lastSpawnedLevel)
                {
                    return unit;
                }
            }

            return null;
        }

        private bool IsVisible(Unit unit, Plane[] frustrumPlanes)
        {
            return GeometryUtility.TestPlanesAABB(frustrumPlanes, unit.Bounds);
        }

        private void Merge(Unit first, Unit second)
        {
            Assert.IsNotNull(first.Health);
            Assert.IsNotNull(second.Health);
            second.Health.Add(first.Health.CurrentValue.Value, true);
            first.Kill(DeathCause.Removed);
        }

        private void OnUnitSpawned(UnitSpawnedMessage msg)
        {
            var unit = msg.Unit as Unit;
            if (unit.UnitType != UnitType.ENEMY) return;
            _lastSpawnedLevel = (unit.Model as EnemyUnitModel).Level;
            var rez = _units.Add(unit);
            Assert.IsTrue(rez, "Failed to add unit to EnemyRemovalService");
            unit.OnDeath += OnUnitDeath;
            unit.OnUnitDestroyed += OnUnitDestroyed;
        }

        private void OnUnitDeath(IUnit unit, DeathCause deathCause)
        {
            OnUnitDestroyed(unit);
        }

        private void OnUnitDestroyed(IUnit unit)
        {
            unit.OnUnitDestroyed -= OnUnitDestroyed;
            unit.OnDeath -= OnUnitDeath;
            _units.Remove(unit as Unit);
        }
    }
}