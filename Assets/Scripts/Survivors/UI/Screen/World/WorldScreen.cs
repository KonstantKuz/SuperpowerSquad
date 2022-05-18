using System.Collections;
using Feofun.App;
using Feofun.UI.Screen;
using JetBrains.Annotations;
using SuperMaxim.Messaging;
using Survivors.Session;
using Survivors.Session.Messages;
using Survivors.UI.Screen.Debriefing;
using UnityEngine;
using Zenject;

namespace Survivors.UI.Screen.World
{
    public class WorldScreen : BaseScreen
    {
        public const ScreenId WORLD_SCREEN = ScreenId.World;
        public override ScreenId ScreenId => WORLD_SCREEN;
        public override string Url => ScreenName;

        [SerializeField]
        private float _afterSessionDelay = 2;

        [Inject]
        private SessionService _sessionService;
        [Inject]
        private IMessenger _messenger;
        [Inject]
        private ScreenSwitcher _screenSwitcher;

        [PublicAPI]
        public void Init()
        {
            _sessionService.Start();
            _messenger.Subscribe<SessionEndMessage>(OnSessionFinished);
        }

        private void OnSessionFinished(SessionEndMessage evn)
        {
            StartCoroutine(EndSession());
        }

        private IEnumerator EndSession()
        {
            TermSession();
            yield return new WaitForSeconds(_afterSessionDelay);
            _screenSwitcher.SwitchTo(DebriefingScreen.DEBRIEFING_SCREEN.ToString());
        }

        private void TermSession()
        {
            var services = AppContext.Container.ResolveAll<ISessionTerm>();
            services.ForEach(it => it.Term());
            
        }
    }
}