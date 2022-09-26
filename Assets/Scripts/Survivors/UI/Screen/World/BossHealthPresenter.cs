using Feofun.Config;
using Feofun.Extension;
using SuperMaxim.Messaging;
using Survivors.UI.Hud.Unit;
using Survivors.Units;
using Survivors.Units.Component;
using Survivors.Units.Enemy.Config;
using Survivors.Units.Messages;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using Zenject;
using Unit = Survivors.Units.Unit;

namespace Survivors.UI.Screen.World
{
    public class BossHealthPresenter : MonoBehaviour
    {
        [SerializeField] private HealthBarView _bossHealthBarView;
        [SerializeField] private GameObject _missionProgressView;
        [SerializeField] private GameObject _squadProgressView;

        private CompositeDisposable _disposable;
        private IUnit _currentBoss;

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
            _messenger.SubscribeWithDisposable<UnitSpawnedMessage>(OnUnitSpawned).AddTo(_disposable);
        }
        
        private void OnUnitSpawned(UnitSpawnedMessage msg)
        {
            if (msg.Unit.UnitType != UnitType.ENEMY || !_enemyUnitConfigs.Get(msg.Unit.Model.Id).IsBoss)
            {
                return;
            }
            
            _currentBoss = msg.Unit;
            InitHealthBar();
            SwitchToBossHealthBar(true);
            _currentBoss.OnDeath += (it, cause) => SwitchToBossHealthBar(false);
        }

        private void InitHealthBar()
        {
            var healthModel = new HealthBarModel(_currentBoss.GameObject.GetComponent<IHealthBarOwner>());
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
            if (_currentBoss != null)
            {
                _currentBoss.OnDeath -= (it, cause) => SwitchToBossHealthBar(false);
                _currentBoss = null;
            }

            _disposable?.Dispose();
            _disposable = null;
        }
    }
}