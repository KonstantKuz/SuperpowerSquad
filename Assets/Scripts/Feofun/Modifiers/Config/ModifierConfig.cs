using System.Runtime.Serialization;

namespace Feofun.Modifiers.Config
{
    [DataContract]
    public class ModifierConfig
    {
        [DataMember(Name = "Modifier")]
        private string _modifier;

        [DataMember(Name = "ParameterName")]
        private string _parameterName;

        [DataMember(Name = "Value")]
        private float _value;

        public string Modifier => _modifier;

        public string ParameterName => _parameterName;

        public float Value => _value; 
    }
}