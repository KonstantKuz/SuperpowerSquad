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
        [SerializeField] private int _maxEnemies;
        [SerializeField] private float _minRemovalAge;

        [Inject] private UnitService _unitService;
        [Inject] private IMessenger _messenger;

        private int _lastSpawnedLevel = 1;

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
            var enemies = GetEnemies();
            if (enemies.Count <= _maxEnemies) return;
            var removeCount = enemies.Count - _maxEnemies;
            
            var frustumPlanes = GeometryUtility.CalculateFrustumPlanes(UnityEngine.Camera.main);            

            var candidatesOrderedByAge = GetCandidates(enemies, frustumPlanes);

            for (int i = 0; i < removeCount; i++)
            {
                var first = candidatesOrderedByAge.Dequeue();
                if (first == null) break;
                var second = FindRemovalCandidate(candidatesOrderedByAge, first.Health.CurrentValue.Value);
                if (second == null) break;
                Merge(first, second);
            }
        }

        private Queue<Unit> GetCandidates(IEnumerable<Unit> enemies, Plane[] frustumPlanes)
        {
            return new Queue<Unit>(enemies
                .Where(it => it.LifeTime > _minRemovalAge)
                .Where(it => !IsVisible(it, frustumPlanes))
                .OrderByDescending(it => it.LifeTime));
        }

        private List<Unit> GetEnemies()
        {
            return _unitService
                .GetAllUnitsOfType(UnitType.ENEMY)
                .Select(it => it as Unit)
                .ToList();
        }

        private Unit FindRemovalCandidate(Queue<Unit> units, float health)
        {
            foreach (var unit in units)
            {
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
            second.Health.Add(first.Health.CurrentValue.Value);
            first.Kill(DeathCause.Removed);
        }

        private void OnUnitSpawned(UnitSpawnedMessage msg)
        {
            var unit = msg.Unit;
            if (unit.UnitType != UnitType.ENEMY) return;
            _lastSpawnedLevel = (unit.Model as EnemyUnitModel).Level;
        }
    }
}