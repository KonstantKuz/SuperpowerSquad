using System.Collections.Generic;
using System.Linq;
using ModestTree;
using Survivors.Units.Component.Health;
using UnityEngine;
using Zenject;

namespace Survivors.Units.Service
{
    public class EnemyRemovalService: MonoBehaviour
    {
        [SerializeField] private int _maxEnemies;
        [SerializeField] private float _minRemovalAge;

        [Inject] private UnitService _unitService;
        private void Update()
        {
            var enemies = _unitService
                .GetAllUnitsOfType(UnitType.ENEMY)
                .Select(it => it as Unit)
                .ToList();
            if (enemies.Count <= _maxEnemies) return;

            var orderedByAge = new Queue<Unit>(enemies
                .Where(it => it.LifeTime > _minRemovalAge)
                .OrderByDescending(it => it.LifeTime));
            
            Plane[] planes = GeometryUtility.CalculateFrustumPlanes(UnityEngine.Camera.main);

            for (int i = 0; i < enemies.Count - _maxEnemies; i++)
            {
                var first = FindRemovalCandidate(orderedByAge, planes);
                if (first == null) break;
                var second = FindRemovalCandidate(orderedByAge, planes);
                if (second == null) break;
                Merge(first, second);
            }
        }

        private Unit FindRemovalCandidate(Queue<Unit> units, Plane[] frustrumPlanes)
        {
            while (!units.IsEmpty())
            {
                var first = units.Dequeue();
                if (!IsVisible(first, frustrumPlanes)) return first;
            }

            return null;
        }

        private bool IsVisible(Unit unit, Plane[] frustrumPlanes)
        {
            return GeometryUtility.TestPlanesAABB(frustrumPlanes, unit.Bounds);
        }

        private void Merge(Unit first, Unit second)
        {
            second.Health.Add(first.Health.CurrentValue.Value);
            first.Kill(DeathCause.Removed);
        }
    }
}