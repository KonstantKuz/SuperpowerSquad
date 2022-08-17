using Survivors.WorldEvents.Events;
using UnityEngine;

namespace Survivors.WorldEvents.WaitConditions
{
    public class WaitForWorldEventFinished : CustomYieldInstruction
    {
        private bool _completed;
        private WorldEvent _currentEvent;
        public WaitForWorldEventFinished(WorldEvent worldEvent)
        {
            _currentEvent = worldEvent;
            worldEvent.OnFinished += OnEventFinished;
        }
        private void OnEventFinished()
        {
            _currentEvent.OnFinished -= OnEventFinished;
            _completed = true;
        }
        public override bool keepWaiting => !_completed;
    }
}