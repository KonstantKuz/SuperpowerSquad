using System;
using System.Collections;
using System.Runtime.InteropServices;
using Feofun.Config;
using Feofun.Extension;
using Feofun.UI.Dialog;
using Feofun.UI.Screen;
using JetBrains.Annotations;
using SuperMaxim.Messaging;
using Survivors.App.Config;
using Survivors.Session.Messages;
using Survivors.Session.Model;
using Survivors.Session.Service;
using Survivors.UI.Dialog.PauseDialog;
using Survivors.UI.Dialog.StartUnitDialog;
using Survivors.UI.Dialog.StartUnitDialog.Model;
using Survivors.UI.Screen.Debriefing;
using Survivors.UI.Screen.Debriefing.Model;
using Survivors.Units;
using Survivors.Units.Enemy.Config;
using Survivors.Units.Messages;
using Survivors.Upgrade;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using Zenject;
using Unit = Survivors.Units.Unit;

namespace Survivors.UI.Screen.World
{
    public class WorldScreen : BaseScreen
    {
        public const ScreenId ID = ScreenId.World;
        public override ScreenId ScreenId => ID;
        public override string Url => ScreenName;

        [SerializeField] private MissionProgressView _missionProgressView;
        [SerializeField] private GameObject _squadProgressView;
        [SerializeField] private float _afterSessionDelay = 2;

        private CompositeDisposable _disposable;
        
        [Inject] private SessionService _sessionService;
        [Inject] private IMessenger _messenger;
        [Inject] private ScreenSwitcher _screenSwitcher;     
        [Inject] private Location.World _world;
        [Inject] private Joystick _joystick;
        [Inject] private DialogManager _dialogManager;
        [Inject] private UpgradeService _upgradeService;
        [Inject] private ConstantsConfig _constants;
        [Inject] private StringKeyedConfigCollection<EnemyUnitConfig> _enemyUnitConfigs; 
        
        [PublicAPI]
        public void Init()
        {
            Dispose();
            _disposable = new CompositeDisposable();
            
            _sessionService.Start();
            InitProgressView();

            _joystick.Attach(transform);
            _messenger.SubscribeWithDisposable<UnitSpawnedMessage>(OnUnitSpawned).AddTo(_disposable);
            _messenger.SubscribeWithDisposable<SessionEndMessage>(OnSessionFinished).AddTo(_disposable);
            
            if (_constants.ChooseFirstUnitEnabled)
            {
                _world.Pause();
                _dialogManager.Show<StartUnitDialog, Action<StartUnitSelection>>(OnChangeStartUnit);
            }
        }

        private void InitProgressView()
        {
            var model = new MissionProgressModel(_sessionService.LevelConfig, _sessionService.Kills, _sessionService.PlayTime);
            _missionProgressView.Init(model);
        }

        private void OnUnitSpawned(UnitSpawnedMessage msg)
        {
            var unit = msg.Unit;
            if (unit.UnitType != UnitType.ENEMY || !_enemyUnitConfigs.Get(unit.Model.Id).IsBoss)
            {
                return;
            }
            SetActiveProgressView(false);
            unit.GameObject.OnDisableAsObservable().Subscribe(it => SetActiveProgressView(true)).AddTo(_disposable);
        }

        private void SetActiveProgressView(bool value)
        {
            _missionProgressView.gameObject.SetActive(value);
            _squadProgressView.gameObject.SetActive(value);
        }
        
        private void OnChangeStartUnit(StartUnitSelection startUnitSelection)
        {
            _sessionService.ChangeStartUnit(startUnitSelection.UnitId);
            _upgradeService.IncreaseLevel(startUnitSelection.UpgradeId);
            _dialogManager.Hide<StartUnitDialog>();
            _dialogManager.Show<PauseDialog>();
        }

        private void OnSessionFinished(SessionEndMessage evn)
        {
            StartCoroutine(EndSession(evn.Result));
        }

        private IEnumerator EndSession(SessionResult result)
        {
            yield return new WaitForSeconds(_afterSessionDelay);
            _world.CleanUp();
            var debriefingModel = new DebriefingScreenModel(result, _sessionService.Session);
            _screenSwitcher.SwitchTo(DebriefingScreen.URL, debriefingModel);
        }

        private void Dispose()
        {
            _disposable?.Dispose();
            _disposable = null;
        }

        private void OnDestroy()
        {
            Dispose();
        }
    }
}