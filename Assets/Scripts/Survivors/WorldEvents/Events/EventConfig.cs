using UnityEngine;

namespace Survivors.WorldEvents.Events
{
    [CreateAssetMenu(fileName = "EventConfig", menuName = "ScriptableObjects/EventConfig/BaseEventConfig")]
    public class EventConfig : ScriptableObject
    {
        public float EventDuration = 15;
    }
}