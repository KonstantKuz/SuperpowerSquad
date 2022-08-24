using SuperMaxim.Messaging;
using Survivors.WorldEvents.Messages;
using UnityEngine;
using Zenject;

namespace Survivors.UI.Screen.World.WorldEvent
{
    public class WorldEventPresenter : MonoBehaviour
    {
        [SerializeField]
        private WorldEventWarningView _worldEventView;

        [Inject]
        public IMessenger _messenger;

        public void OnEnable()
        {
            _messenger.Subscribe<WorldEventWarningShowMessage>(OnEventWarningShow);
        }

        private void OnEventWarningShow(WorldEventWarningShowMessage evn)
        {
            _worldEventView.Init(new EventViewModel(evn.EventType, evn.EventWarningShowDuration));
        }
        public void OnDisable()
        {
            _messenger.Unsubscribe<WorldEventWarningShowMessage>(OnEventWarningShow);
        }
        

    }
}