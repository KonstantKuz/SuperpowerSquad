using System;
using System.Collections.Generic;
using System.Linq;
using SuperMaxim.Messaging;
using Survivors.Units.Enemy.Model;
using Survivors.Units.Messages;
using UnityEngine;
using Zenject;

namespace Survivors.Units.Service
{
    public class EnemyRemovalService: MonoBehaviour
    {
        [SerializeField] private int _softLimit;
        [SerializeField] private int _hardLimit;
        [SerializeField] private float _minRemovalAge;

        [Inject] private UnitService _unitService;
        [Inject] private IMessenger _messenger;

        private int _lastSpawnedLevel = 1;
        private SortedSet<Unit> _units = new SortedSet<Unit>(Comparer<Unit>.Create((a, b) => a.LifeTime.CompareTo(b.LifeTime)));

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
            second.Health.Add(first.Health.CurrentValue.Value, true);
            first.Kill(DeathCause.Removed);
        }

        private void OnUnitSpawned(UnitSpawnedMessage msg)
        {
            var unit = msg.Unit as Unit;
            if (unit.UnitType != UnitType.ENEMY) return;
            _lastSpawnedLevel = (unit.Model as EnemyUnitModel).Level;
            _units.Add(unit);
            unit.OnDeath += OnUnitDeath;
        }

        private void OnUnitDeath(IUnit unit, DeathCause deathCause)
        {
            unit.OnDeath -= OnUnitDeath;
            _units.Remove(unit as Unit);
        }
    }
}