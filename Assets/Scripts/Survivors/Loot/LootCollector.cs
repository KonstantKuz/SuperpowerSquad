using System;
using System.Collections.Generic;
using DG.Tweening;
using Survivors.Loot.Service;
using Survivors.Session;
using Survivors.Squad.Config;
using UnityEngine;
using Zenject;

namespace Survivors.Loot
{
    public class LootCollector : MonoBehaviour, IWorldCleanUp
    {
        [SerializeField] private float _collectTime;
        [SerializeField] private SphereCollider _collider;
        
        private List<Tween> _movingLoots = new List<Tween>();
        
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
            
            MoveLoot(loot);
        }

        private void MoveLoot(DroppingLoot loot)
        {
            var collectLootMove = loot.transform.DOMove(transform.position, _collectTime).SetEase(Ease.Linear);
            _movingLoots.Add(collectLootMove);
            collectLootMove.onComplete = () =>
            {
                _movingLoots.Remove(collectLootMove);
                _lootService.OnLootCollected(loot.Config);
                Destroy(loot.gameObject);
            };
        }
        
        public void OnWorldCleanUp()
        {
            _movingLoots.ForEach(it => it.Kill());
        }
    }
}