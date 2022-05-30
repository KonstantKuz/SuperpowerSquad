using System.Collections.Generic;
using DG.Tweening;
using Survivors.Loot.Service;
using Survivors.Session;
using Survivors.Squad;
using UniRx;
using UnityEngine;
using Zenject;

namespace Survivors.Loot
{
    public class LootCollector : MonoBehaviour, IWorldScope, ISquadInitializable
    {
        [SerializeField]
        private float _collectTime;
        [SerializeField]
        private SphereCollider _collider;

        [Inject]
        private DroppingLootService _lootService;

        private List<Tween> _movingLoots = new List<Tween>();
        private CompositeDisposable _disposable;
        
        public void OnWorldSetup()
        {
            _disposable?.Dispose();
            _disposable = new CompositeDisposable();
        }
        public void Init(Squad.Squad squad)
        {
            squad.Model.CollectRadius.Subscribe(radius => _collider.radius = radius).AddTo(_disposable);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!other.TryGetComponent(out DroppingLoot loot)) {
                return;
            }
            MoveLoot(loot);
        }

        private void MoveLoot(DroppingLoot loot)
        {
            var collectLootMove = loot.transform.DOMove(transform.position, _collectTime).SetEase(Ease.Linear);
            _movingLoots.Add(collectLootMove);
            collectLootMove.onComplete = () => {
                _movingLoots.Remove(collectLootMove);
                _lootService.OnLootCollected(loot.Config);
                Destroy(loot.gameObject);
            };
        }
        
        public void OnWorldCleanUp()
        {
            _movingLoots.ForEach(it => it.Kill());
            _disposable?.Dispose();
            _disposable = null;
        }
    }
}