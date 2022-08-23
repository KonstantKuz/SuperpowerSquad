using SuperMaxim.Messaging;
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
}