using UnityEngine;

namespace Survivors.WorldEvents.Events
{
    [CreateAssetMenu(fileName = "EventConfig", menuName = "ScriptableObjects/EventConfig/BaseEventConfig")]
    public class EventConfig : ScriptableObject
    {
        [SerializeField]
        private float _eventDuration = 15;
        public float EventDuration => _eventDuration;
    }
}