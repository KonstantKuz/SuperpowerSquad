using System.Collections;
using SuperMaxim.Messaging;
using Survivors.Location;
using Survivors.Session.Messages;
using Survivors.WorldEvents.Config;
using UnityEngine;
using Zenject;

namespace Survivors.WorldEvents.Service
{
    public class WorldEventService : IWorldScope
    {
        [Inject]
        private WorldEventsConfig _worldEventsConfig;      
        [Inject]
        private WorldEventFactory _worldEventFactory;
        [Inject]
        private IMessenger _messenger;
        
        private Coroutine _eventsCoroutine;
        
        public void OnWorldSetup()
        {
            _messenger.Subscribe<SessionStartMessage>(OnSessionStarted);
            _messenger.Subscribe<SessionEndMessage>(OnSessionFinished);
        }
        public void OnWorldCleanUp()
        {
            _messenger.Unsubscribe<SessionStartMessage>(OnSessionStarted);
            _messenger.Unsubscribe<SessionEndMessage>(OnSessionFinished);
        }
        private void OnSessionStarted(SessionStartMessage evn)
        {
            StartLevelEvents(evn.Level.ToString());
        }
        private void OnSessionFinished(SessionEndMessage evn)
        {
            DisposeCoroutine();
        }

        private void StartLevelEvents(string levelId)
        {
            DisposeCoroutine();
            _eventsCoroutine = GameApplication.Instance.StartCoroutine(StartEvents(levelId));
        }
        private IEnumerator StartEvents(string levelId)
        {
            foreach (var eventConfig in _worldEventsConfig.GetEventConfigs(levelId)) {
                yield return new WaitForSeconds(eventConfig.TimeSincePreviousEvent);
                yield return StartEvent(eventConfig);
            }
            StartLevelEvents(levelId);
        }
        
        private IEnumerator StartEvent(WorldEventConfig eventConfig)
        {
            var currentEvent = _worldEventFactory.CreateEvent(eventConfig.EventType);
            yield return currentEvent.Start(_worldEventFactory.GetConfig(eventConfig.EventType));
        }
        
        private void DisposeCoroutine()
        {
            if (_eventsCoroutine == null) {
                return;
            }
            GameApplication.Instance.StopCoroutine(_eventsCoroutine);
            _eventsCoroutine = null;
        }
    }
}