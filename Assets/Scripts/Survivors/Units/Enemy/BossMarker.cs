using Feofun.Components;
using SuperMaxim.Messaging;
using Survivors.Enemy.Spawn;
using Survivors.Enemy.Spawn.Service;
using Survivors.Units.Messages;
using UnityEngine;
using Zenject;

namespace Survivors.Units.Enemy
{
    public class BossMarker : MonoBehaviour, IInitializable<IUnit>
    {
        [Inject] private IMessenger _messenger; 
        [Inject] private EnemySpawnService _enemySpawnService;

        public void Init(IUnit owner)
        {
            _messenger.Publish(new BossSpawnedMessage(owner));
            owner.OnDeath += OnDeath;
        }

        private void OnDeath(IUnit unit, DeathCause arg2)
        {
            unit.OnDeath -= OnDeath;
            _enemySpawnService.UpdatableScope.IsPaused = false;
        }
    }
}