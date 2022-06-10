using System.Collections;
using Feofun.UI.Screen;
using JetBrains.Annotations;
using SuperMaxim.Messaging;
using Survivors.Session.Messages;
using Survivors.Session.Model;
using Survivors.Session.Service;
using Survivors.UI.Screen.Debriefing;
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

        [PublicAPI]
        public void Init()
        {
            _world.Setup();
            _sessionService.Start();
            _messenger.Subscribe<SessionEndMessage>(OnSessionFinished);
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