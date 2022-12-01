using System.Collections.Generic;
using System.Linq;
using Feofun.Components;
using Feofun.Extension;
using SuperMaxim.Core.Extensions;
using Survivors.Location;
using Survivors.Location.ObjectFactory.Factories;
using Survivors.Loot.Service;
using Survivors.Session.Service;
using Survivors.Squad.Data;
using Survivors.Squad.Service;
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
        private ObjectPoolFactory _objectFactory;
        [Inject]
        private SessionService _sessionService;    
        [Inject]
        private SquadProgressService _squadProgressService;
        [Inject] 
        private World _world;
        
        private CompositeDisposable _disposable;
        
        private readonly ISet<DroppingLoot> _movingLoots = new HashSet<DroppingLoot>();
        
        public void Init(Squad.Squad squad)
        {
            _disposable?.Dispose();
            _disposable = new CompositeDisposable();
            squad.Model.CollectRadius.Subscribe(radius => _collider.radius = radius).AddTo(_disposable);
            _squadProgressService.GetAsObservable(SquadProgressType.Level).Diff().Subscribe(it => CollectAllLoot())
                .AddTo(_disposable);
        } 
        
        private void CollectAllLoot()
        {
            _lootService.AllLoot.ForEach(it => _movingLoots.Add(it));
            _lootService.RemoveAll();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (_sessionService.SessionCompleted) {
                return;
            }
            if (!other.TryGetComponent(out DroppingLoot loot)) {
                return;
            }
            if (_movingLoots.Contains(loot)) return;

            _lootService.Remove(loot);
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
            loot.transform.position +=  _collectSpeed * Time.deltaTime * moveDirection;
        }

        private void TryCollect(DroppingLoot loot)
        {
            if (_world.IsPaused) return;            
            if (Vector3.Distance(loot.transform.position, transform.position) > LOOT_DESTROY_DISTANCE) {
                return;
            }
            _lootService.OnLootCollected(loot.LootType, loot.Config);
            _movingLoots.Remove(loot);
            _objectFactory.Destroy(loot.gameObject);
        }

        public void OnDestroy()
        {
            _movingLoots.Clear();
            _disposable?.Dispose();
            _disposable = null;
        }
    }
}