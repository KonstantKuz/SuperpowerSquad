using UnityEngine;

namespace Survivors.WorldEvents.Events.Lava
{
    [CreateAssetMenu(fileName = "LavaEventConfig", menuName = "ScriptableObjects/EventConfig/LavaEventConfig")]
    public class LavaEventConfig : EventConfig
    {
        [SerializeField]
        private float _minAppearTime = 3;
        [SerializeField]
        private float _maxAppearTime = 7;  
        
        [SerializeField]
        private float _minDisappearTime = 3;
        [SerializeField]
        private float _maxDisappearTime = 7;
        
        
        public float LavaAverageRadius = 7;

        public float LavaRadiusDispersion = 4;
        
        
        public float DamagePeriod = 1;

        public float DamagePercent = 30;

        public float RandomAppearTime => Random.Range(_minAppearTime, _maxAppearTime); 
        public float RandomDisappearTime => Random.Range(_minDisappearTime, _maxDisappearTime);
        public float RandomRadius => Random.Range(LavaAverageRadius - LavaRadiusDispersion, LavaAverageRadius + LavaRadiusDispersion);
        public float LavaAverageDiameter => LavaAverageRadius * 2;
    }

    public class EventConfig : ScriptableObject
    {
        public float EventDuration = 30;
    }
}