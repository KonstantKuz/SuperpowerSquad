using Feofun.Localization;
using SuperMaxim.Messaging;
using Survivors.WorldEvents.Config;
using Survivors.WorldEvents.Messages;
using UnityEngine;
using Zenject;

namespace Survivors.UI.Screen.World.WorldEvent
{
    public class WorldEventPresenter : MonoBehaviour
    {

        [SerializeField]
        private WorldEventView _worldEventView;
        
        [Inject]
        public IMessenger _messenger;
        
        public void OnEnable()
        {
            _messenger.Subscribe<WorldEventStartMessage>(OnEventStarted);
        }

        private void OnEventStarted(WorldEventStartMessage evn)
        {
            _worldEventView.Init(new EventViewModel(evn.EventType));
        }

        public void OnDisable()
        {
            _messenger.Unsubscribe<WorldEventStartMessage>(OnEventStarted);
        }

    }

    public class EventViewModel
    {
        public LocalizableText Text { get; }
        public EventViewModel(WorldEventType eventType)
        {
            Text = LocalizableText.Create(eventType.ToString());
        }
    }
}