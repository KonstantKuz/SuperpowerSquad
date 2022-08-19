using UnityEngine;

namespace Survivors.WorldEvents.Events.Lava.Config
{
    [CreateAssetMenu(fileName = "LavaEventConfig", menuName = "ScriptableObjects/EventConfig/LavaEventConfig")]
    public class LavaEventConfig : EventConfig
    {
        public int SpawnCountOnCircle = 5;    
        
        public int SpawnCountStepOnCircle = 6;
        
        public float MinLavaRadius = 4;

        public float MaxLavaRadius = 7;
        
        public float DamagePeriod = 1;

        public float DamagePercent = 30;

        public string LavaPrefabId = "Lava";
        
        [SerializeField]
        private float _minAppearTime = 3;
        [SerializeField]
        private float _maxAppearTime = 7;  
        
        [SerializeField]
        private float _minDisappearTime = 3;
        [SerializeField]
        private float _maxDisappearTime = 7;
        

        public float RandomAppearTime => Random.Range(_minAppearTime, _maxAppearTime); 
        public float RandomDisappearTime => Random.Range(_minDisappearTime, _maxDisappearTime);
        public float RandomRadius => Random.Range(MinLavaRadius, MaxLavaRadius);
        public float MinLavaDiameter => MinLavaRadius * 2;
    }

    public class EventConfig : ScriptableObject
    {
        public float EventDuration = 30;
    }
}