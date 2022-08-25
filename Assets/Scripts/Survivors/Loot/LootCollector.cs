using System.Collections.Generic;
using System.Linq;
using Feofun.Components;
using Survivors.Location;
using Survivors.Location.ObjectFactory;
using Survivors.Loot.Service;
using Survivors.Session.Service;
using UniRx;
using UnityEngine;
using Zenject;

namespace Survivors.Loot
{
    public class LootCollector : MonoBehaviour, IInitializable<Squad.Squad>
    {
        private const float LOOT_DESTROY_DISTANCE = 1f;
        
        [SerializeField]
        private float _collectSpeed = 1;
        [SerializeField]
        private SphereCollider _collider;

        [Inject]
        private DroppingLootService _lootService;       
        [Inject]
        private ObjectInstancingFactory objectInstancingFactory;
        [Inject]
        private SessionService _sessionService;
        [Inject] 
        private World _world;

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
            if (_sessionService.SessionCompleted) {
                return;
            }
            if (!other.TryGetComponent(out DroppingLoot loot)) {
                return;
            }
            _movingLoots.Add(loot);
        }

        private void Update()
        {
            var loots = _movingLoots.ToList();
            loots.ForEach(Move);
            loots.ForEach(TryCollect);
        }

        private void Move(DroppingLoot loot)
        {
            var moveDirection = (transform.position - loot.transform.position).normalized;
            var speed = _collectSpeed + _squad.Model.Speed.Value;
            loot.transform.position += moveDirection * speed * Time.deltaTime;
        }

        private void TryCollect(DroppingLoot loot)
        {
            if (_world.IsPaused) return;            
            if (Vector3.Distance(loot.transform.position, transform.position) > LOOT_DESTROY_DISTANCE)
            {
                return;
            }
            
            _lootService.OnLootCollected(loot.Config);
            _movingLoots.Remove(loot);
            objectInstancingFactory.Destroy<DroppingLoot>(loot.gameObject);
        }

        public void OnDestroy()
        {
            _movingLoots.Clear();
            _disposable?.Dispose();
            _disposable = null;
        }
    }
}