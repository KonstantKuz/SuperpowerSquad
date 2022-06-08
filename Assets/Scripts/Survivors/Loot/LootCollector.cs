using System.Collections.Generic;
using System.Linq;
using Feofun.Components;
using Survivors.Loot.Service;
using UniRx;
using UnityEngine;
using Zenject;

namespace Survivors.Loot
{
    public class LootCollector : MonoBehaviour, IInitializable<Squad.Squad>
    {
        private const float DISTANCE_PRECISION = 1f;
        
        [SerializeField]
        private float _collectSpeed = 1;
        [SerializeField]
        private SphereCollider _collider;

        [Inject]
        private DroppingLootService _lootService;

        private Squad.Squad _squad;
        private CompositeDisposable _disposable;
        private List<DroppingLoot> _movingLoots = new List<DroppingLoot>();
        
        public void Init(Squad.Squad squad)
        {
            _squad = squad;
            _disposable?.Dispose();
            _disposable = new CompositeDisposable();
            squad.Model.CollectRadius.Subscribe(radius => _collider.radius = radius).AddTo(_disposable);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!other.TryGetComponent(out DroppingLoot loot)) {
                return;
            }

            _movingLoots.Add(loot);
        }

        private void Update()
        {
            _movingLoots = _movingLoots.Where(it => it != null).ToList();
            _movingLoots.ForEach(Move);
            _movingLoots.ForEach(TryCollect);
        }

        private void Move(DroppingLoot loot)
        {
            var moveDirection = (transform.position - loot.transform.position).normalized;
            var speed = _collectSpeed + _squad.Model.Speed.Value;
            loot.transform.position += moveDirection * speed * Time.deltaTime;
        }

        private void TryCollect(DroppingLoot loot)
        {
            if (Vector3.Distance(loot.transform.position, transform.position) > DISTANCE_PRECISION)
            {
                return;
            }
            _lootService.OnLootCollected(loot.Config);
            Destroy(loot.gameObject);
        }

        public void OnDestroy()
        {
            _movingLoots.Clear();
            _disposable?.Dispose();
            _disposable = null;
        }
    }
}