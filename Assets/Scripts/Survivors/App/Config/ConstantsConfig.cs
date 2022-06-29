using System.Runtime.Serialization;

namespace Survivors.App.Config
{
    public class ConstantsConfig
    {
        [DataMember(Name = "HealthScaleIncrementFactor")]
        private int _healthScaleIncrementFactor;

        public int HealthScaleIncrementFactor => _healthScaleIncrementFactor;
    }
}