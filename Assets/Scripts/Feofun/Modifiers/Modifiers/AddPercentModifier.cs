using Feofun.Modifiers.Parameters;

namespace Feofun.Modifiers.Modifiers
{
    public class AddPercentModifier: IModifier
    {
        private readonly string _paramName;
        private readonly float _percentValue;

        public AddPercentModifier(string paramName, float percentValue)
        {
            _paramName = paramName;
            _percentValue = percentValue;
        }

        public void Apply(IModifiableParameterOwner owner)
        {
            var parameter = owner.GetParameter<FloatModifiableParameter>(_paramName);
            parameter.AddValue(parameter.InitialValue * _percentValue / 100);
        }
    }
}