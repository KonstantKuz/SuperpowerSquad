using System.Collections;
using Survivors.WorldEvents.Config;
using Survivors.WorldEvents.WaitConditions;
using UnityEngine;
using Zenject;

namespace Survivors.WorldEvents.Service
{
    public class WorldEventsService : MonoBehaviour
    {
        [Inject]
        private WorldEventsConfig _worldEventsConfig;      
        [Inject]
        private WorldEventFactory _worldEventFactory;

        private Coroutine _eventsCoroutine;
        
        public void StartLevelEvents(string levelId)
        {
            DisposeCoroutine();
            _eventsCoroutine = StartCoroutine(StartEvents(levelId));
        }
        private IEnumerator StartEvents(string levelId)
        {
            foreach (var eventConfig in _worldEventsConfig.GetEventConfigs(levelId)) {
                yield return WaitTimeoutAndStartEvent(eventConfig);
            }
            StartLevelEvents(levelId);
        }

        private IEnumerator WaitTimeoutAndStartEvent(WorldEventConfig eventConfig)
        {
            yield return new WaitForSeconds(eventConfig.TimeSincePreviousEvent);
            yield return StartEvent(eventConfig);
        }

        private IEnumerator StartEvent(WorldEventConfig eventConfig)
        {
            var currentEvent = _worldEventFactory.CreateEvent(eventConfig.EventType);
            var finishWaiter = new WaitForWorldEventFinished(currentEvent);
            currentEvent.Start();
            yield return finishWaiter;
        }
        
        private void DisposeCoroutine()
        {
            if (_eventsCoroutine == null) {
                return;
            }
            StopCoroutine(_eventsCoroutine);
            _eventsCoroutine = null;
        }
    }
}