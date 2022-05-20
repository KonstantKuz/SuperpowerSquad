using System;
using DG.Tweening;
using Survivors.Loot.Service;
using Survivors.Squad.Config;
using UnityEngine;
using Zenject;

namespace Survivors.Loot
{
    public class LootCollector : MonoBehaviour
    {
        [SerializeField] private SphereCollider _collider;
        
        [Inject] private SquadConfig _squadConfig;
        [Inject] private DroppingLootService _lootService;

        private void Awake()
        {
            _collider.radius = _squadConfig.Params.CollectRadius;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!other.TryGetComponent(out DroppingLoot loot))
            {
                return;
            }
            var collectLootMove = loot.transform.DOMove(transform.position, 0.5f).SetEase(Ease.Linear);
            collectLootMove.onComplete += delegate
            {
                _lootService.OnLootCollected(loot.Config);
                Destroy(loot.gameObject);
            };
        }
    }
}