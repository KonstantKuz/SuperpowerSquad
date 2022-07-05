using System.Runtime.Serialization;

namespace Survivors.App.Config
{
    public class ConstantsConfig
    {
        [DataMember(Name = "HealthScaleIncrementFactor")]
        private int _healthScaleIncrementFactor;

        [DataMember(Name = "FirstUnit")]
        private string _firstUnit;

        [DataMember(Name = "ReviveEnemyRemoveRadius")]
        private float _reviveEnemyRemoveRadius;

        public int HealthScaleIncrementFactor => _healthScaleIncrementFactor;

        public string FirstUnit => _firstUnit;

        public float ReviveEnemyRemoveRadius => _reviveEnemyRemoveRadius;
    }
}