using System.Runtime.Serialization;
using UnityEngine;

namespace Survivors.Units.Enemy.Config
{
    [DataContract]
    public class EnemyScaleConfig
    {
        [DataMember] 
        public float Scale;
        [DataMember]
        public float MultiplicationScaleStepFactor;
        [DataMember]
        public int MultiplicationScaleLimitLevel;
        [DataMember]
        public float IncrementScaleStep;    
        
        public float CalculateScale(int level) => Scale * GetMultiplicationScaleFactor(level) + GetIncrementValue(level);
        
        private float GetMultiplicationScaleFactor(int level)
        {
            level = Mathf.Clamp(level, EnemyUnitConfig.MIN_LEVEL, MultiplicationScaleLimitLevel);
            return Mathf.Pow(MultiplicationScaleStepFactor, level - EnemyUnitConfig.MIN_LEVEL);
        }       
        private float GetIncrementValue(int level)
        {
            return level <= MultiplicationScaleLimitLevel ? 0 : (level - MultiplicationScaleLimitLevel) * IncrementScaleStep;
        }
    }
}