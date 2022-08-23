using System.Collections;
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
        [SerializeField]
        private float _showDuration;
        
        [Inject]
        public IMessenger _messenger;
        
        private Coroutine _timerCoroutine;
        
        public void OnEnable()
        {
            Dispose();
            _messenger.Subscribe<WorldEventStartTimerMessage>(OnEventTimerStarted);
        }

        private void OnEventTimerStarted(WorldEventStartTimerMessage evn)
        {
            Dispose();
            var timeout = Mathf.Max(0, evn.TimeBeforeShowEvent - _showDuration);
            _timerCoroutine = StartCoroutine(StartTimerBeforeShowView(timeout, evn.EventType));
        }
        private IEnumerator StartTimerBeforeShowView(float timeout, WorldEventType eventType)
        {
            yield return new WaitForSeconds(timeout);
            _worldEventView.Init(new EventViewModel(eventType, _showDuration));
        }
        public void OnDisable()
        {
            Dispose();
            _messenger.Unsubscribe<WorldEventStartTimerMessage>(OnEventTimerStarted);
        }

        private void Dispose()
        {
            if (_timerCoroutine == null) {
                return;
            }
            StopCoroutine(_timerCoroutine);
            _timerCoroutine = null;
        }

    }
}