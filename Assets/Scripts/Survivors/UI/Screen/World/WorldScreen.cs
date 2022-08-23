using System;
using System.Collections;
using Feofun.UI.Dialog;
using Feofun.UI.Screen;
using JetBrains.Annotations;
using SuperMaxim.Messaging;
using Survivors.Session.Messages;
using Survivors.Session.Model;
using Survivors.Session.Service;
using Survivors.UI.Dialog.PauseDialog;
using Survivors.UI.Dialog.StartUnitDialog;
using Survivors.UI.Dialog.StartUnitDialog.Model;
using Survivors.UI.Screen.Debriefing;
using Survivors.UI.Screen.Debriefing.Model;
using Survivors.Upgrade;
using UnityEngine;
using Zenject;

namespace Survivors.UI.Screen.World
{
    public class WorldScreen : BaseScreen
    {
        public const ScreenId ID = ScreenId.World;
        public override ScreenId ScreenId => ID;
        public override string Url => ScreenName;

        [SerializeField]
        private float _afterSessionDelay = 2;

        [Inject] private SessionService _sessionService;
        [Inject] private IMessenger _messenger;
        [Inject] private ScreenSwitcher _screenSwitcher;     
        [Inject] private Location.World _world;
        [Inject] private Joystick _joystick;
        [Inject] private Analytics.Analytics _analytics;
        [Inject] private DialogManager _dialogManager;
        [Inject] private UpgradeService _upgradeService;
        
        [PublicAPI]
        public void Init()
        {
            _analytics.ReportLevelStart();
            _dialogManager.Show<StartUnitDialog, Action<StartUnitSelection>>(OnChangeStartUnit);
            _joystick.Attach(transform);
            _messenger.Subscribe<SessionEndMessage>(OnSessionFinished);
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
    }
}