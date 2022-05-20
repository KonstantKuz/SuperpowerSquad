using System.Runtime.Serialization;

namespace Feofun.Modifiers.Config
{
    [DataContract]
    public class ModifierConfig
    {
        [DataMember(Name = "Modifier")]
        private ModifierType _modifier;

        [DataMember(Name = "ParameterName")]
        private string _parameterName;

        [DataMember(Name = "Value")]
        private float _value;

        [DataMember(Name = "Target")]
        private ModifierTarget _target;

        public ModifierType Modifier => _modifier;

        public string ParameterName => _parameterName;

        public float Value => _value;

        public ModifierTarget Target => _target;
    }
}