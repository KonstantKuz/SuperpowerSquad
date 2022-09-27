using System;
using Feofun.Config;
using Feofun.Extension;
using SuperMaxim.Messaging;
using Survivors.UI.Hud.Unit;
using Survivors.Units;
using Survivors.Units.Component;
using Survivors.Units.Enemy.Config;
using Survivors.Units.Messages;
using UniRx;
using UnityEngine;
using Zenject;

namespace Survivors.UI.Screen.World
{
    public class BossHealthPresenter : MonoBehaviour
    {
        [SerializeField] private HealthBarView _bossHealthBarView;
        [SerializeField] private GameObject _missionProgressView;
        [SerializeField] private GameObject _squadProgressView;

        private CompositeDisposable _disposable;

        [Inject] private IMessenger _messenger;
        [Inject] private StringKeyedConfigCollection<EnemyUnitConfig> _enemyUnitConfigs;

        private void OnEnable()
        {
            Init();
        }

        private void Init()
        {
            Dispose();
            _disposable = new CompositeDisposable();
            _messenger.SubscribeWithDisposable<BossSpawnedMessage>(OnBossSpawned).AddTo(_disposable);
        }

        private void OnBossSpawned(BossSpawnedMessage msg)
        {
            var unit = msg.Unit;
            if (unit.UnitType != UnitType.ENEMY || !_enemyUnitConfigs.Get(unit.Model.Id).IsBoss)
            {
                throw new ArgumentException($"Unit {msg.Unit.Model.Id} must be enemy boss.");
            }

            InitHealthBar(unit);
            SwitchToBossHealthBar(true);
            unit.OnDeath += OnBossDeath;
            Disposable.Create(() => unit.OnDeath -= OnBossDeath).AddTo(_disposable);
        }

        private void OnBossDeath(IUnit unit, DeathCause deathCause)
        {
            unit.OnDeath -= OnBossDeath;
            SwitchToBossHealthBar(false);
        }

    private void InitHealthBar(IUnit unit)
        {
            var healthModel = new HealthBarModel(unit.GameObject.RequireComponent<IHealthBarOwner>());
            _bossHealthBarView.Init(healthModel);
        }
        
        private void SwitchToBossHealthBar(bool value)
        {
            _bossHealthBarView.gameObject.SetActive(value);
            _missionProgressView.gameObject.SetActive(!value);
            _squadProgressView.gameObject.SetActive(!value);
        }

        private void OnDisable()
        {
            Dispose();
        }

        private void Dispose()
        {
            _disposable?.Dispose();
            _disposable = null;
        }
    }
}