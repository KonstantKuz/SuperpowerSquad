using UnityEngine;

namespace Survivors.WorldEvents.Events
{
    [CreateAssetMenu(fileName = "AvalancheEventConfig", menuName = "ScriptableObjects/EventConfig/AvalancheEventConfig")]
    public class AvalancheEventConfig : EventConfig
    {
        [SerializeField] private float _spawnPeriod;
        [Range(0f, 1f)]
        [SerializeField] private float _moveDirectionDrivenChance;
        [SerializeField] private float _minDistanceBtwnStones;
        [SerializeField] private float _maxAngleSpread;
        [SerializeField] private float _minDistanceFromPlayer;
        [SerializeField] private float _maxDistanceFromPlayer;
        [SerializeField] private LayerMask _cobbleStoneMask;
        [SerializeField] private GameObject _cobblestonePrefab;

        public float SpawnPeriod => _spawnPeriod;
        public float MoveDirectionDrivenChance => _moveDirectionDrivenChance;
        public float MinDistanceBtwnStones => _minDistanceBtwnStones;
        public float MaxAngleSpread => _maxAngleSpread;
        public float MinDistanceFromPlayer => _minDistanceFromPlayer;
        public float MaxDistanceFromPlayer => _maxDistanceFromPlayer;
        public LayerMask CobbleStoneMask => _cobbleStoneMask;
        public GameObject CobblestonePrefab => _cobblestonePrefab;
    }
}
