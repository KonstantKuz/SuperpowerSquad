using System.Collections;
using Feofun.UI.Dialog;
using Feofun.UI.Screen;
using JetBrains.Annotations;
using SuperMaxim.Messaging;
using Survivors.Session.Messages;
using Survivors.Session.Model;
using Survivors.Session.Service;
using Survivors.UI.Dialog.PauseDialog;
using Survivors.UI.Screen.Debriefing;
using UniRx;
using UnityEngine;
using UnityEngine.Assertions;
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

        private CompositeDisposable _disposable;

        [Inject] private SessionService _sessionService;
        [Inject] private IMessenger _messenger;
        [Inject] private ScreenSwitcher _screenSwitcher;     
        [Inject] private Location.World _world;
        [Inject] private DialogManager _dialogManager;
        [Inject] private Analytics.Analytics _analytics;

        [PublicAPI]
        public void Init()
        {
            _world.Setup();
            _sessionService.Start();
            StartCoroutine(WaitForAnimationUpdateBeforePause());
            _messenger.Subscribe<SessionEndMessage>(OnSessionFinished);
        }

        public override IEnumerator Hide()
        {
            _disposable.Dispose();
            _disposable = null;
            return base.Hide();
        }

        private IEnumerator WaitForAnimationUpdateBeforePause()
        {
            yield return new WaitForEndOfFrame();
            var pauseDialog = _dialogManager.Show<PauseDialog>();
            
            Assert.IsNull(_disposable);
            _disposable = new CompositeDisposable();
            pauseDialog.CloseEvent.Subscribe(OnSessionStarted).AddTo(_disposable);
        }

        private void OnSessionStarted(Unit _)
        {
            _analytics.ReportLevelStart();
        }

        private void OnSessionFinished(SessionEndMessage evn)
        {
            StartCoroutine(EndSession(evn.Result));
        }

        private IEnumerator EndSession(SessionResult result)
        {
            yield return new WaitForSeconds(_afterSessionDelay);
            _world.CleanUp();
            _screenSwitcher.SwitchTo(DebriefingScreen.ID.ToString(), result);
        }

 
    }
}