namespace Feofun.Modifiers
{
    public class AddValueModifier: IModifier
    {
        private readonly string _paramName;
        private readonly float _value;

        public AddValueModifier(string paramName, float value)
        {
            _paramName = paramName;
            _value = value;
        }

        public void Apply(IModifiableParameterOwner owner)
        {
            var parameter = owner.GetParameter<FloatModifiableParameter>(_paramName);
            parameter.AddValue(_value);
        }
    }
}