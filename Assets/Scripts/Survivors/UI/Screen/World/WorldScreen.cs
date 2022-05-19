using System.Collections;
using Feofun.UI.Screen;
using JetBrains.Annotations;
using SuperMaxim.Messaging;
using Survivors.Session;
using Survivors.Session.Messages;
using Survivors.UI.Screen.Debriefing;
using Survivors.Units;
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
            _sessionService.Start();
            _messenger.Subscribe<SessionEndMessage>(OnSessionFinished);
        }

        private void OnSessionFinished(SessionEndMessage evn)
        {
            StartCoroutine(EndSession(evn.Winner));
        }

        private IEnumerator EndSession(UnitType winner)
        {
            yield return new WaitForSeconds(_afterSessionDelay);
            _world.CleanUp();
            _screenSwitcher.SwitchTo(DebriefingScreen.ID.ToString(), winner);
        }

 
    }
}