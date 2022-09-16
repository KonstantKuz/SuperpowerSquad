using System.Runtime.Serialization;

namespace Survivors.App.Config
{
    public class ConstantsConfig
    {
        [DataMember(Name = "HealthScaleIncrementFactor")]
        private float _healthScaleIncrementFactor;

        [DataMember(Name = "FirstUnit")]
        private string _firstUnit;    
        
        [DataMember(Name = "MaxMetaUpgradeLevel")]
        private int _maxMetaUpgradeLevel;

        [DataMember(Name = "ReviveEnemyRemoveRadius")]
        private float _reviveEnemyRemoveRadius;     
        
        [DataMember(Name = "EventWarningShowDuration")]
        private float _eventWarningShowDuration;   
        
        [DataMember(Name = "EnemyHpsSpawnerEnabled")]
        private bool _enemyHpsSpawnerEnabled;

        public float HealthScaleIncrementFactor => _healthScaleIncrementFactor;
        public string FirstUnit => _firstUnit;
        public int MaxMetaUpgradeLevel => _maxMetaUpgradeLevel;

        public float ReviveEnemyRemoveRadius => _reviveEnemyRemoveRadius;  
        
        public float EventWarningShowDuration => _eventWarningShowDuration;   
        
        public bool EnemyHpsSpawnerEnabled => _enemyHpsSpawnerEnabled;
    }
}