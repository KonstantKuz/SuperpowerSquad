using UnityEngine;

namespace Survivors.WorldEvents.Events.Lava
{
    [CreateAssetMenu(fileName = "LavaEventConfig", menuName = "ScriptableObjects/LavaEventConfig")]
    public class LavaEventConfig : EventConfig
    {
        public float EventDuration = 30;

        public float AppearTime = 1;

        public float Radius = 3;

        public float DamagePeriod = 1;

        public float DamagePercent = 30;
        
        public LayerMask LavaMask;

    }

    public class EventConfig : ScriptableObject
    {
    }
}