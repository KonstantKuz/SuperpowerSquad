using Feofun.Modifiers.Parameters;

namespace Feofun.Modifiers.Modifiers
{
    public class PercentModifier: IModifier
    {
        private readonly string _paramName;
        private readonly float _percentValue;
        private readonly bool _isIncreasing;

        public PercentModifier(string paramName, float percentValue, bool isIncreasing)
        {
            _paramName = paramName;
            _percentValue = percentValue;
            _isIncreasing = isIncreasing;
        }

        public void Apply(IModifiableParameterOwner owner)
        {
            var parameter = owner.GetParameter<FloatModifiableParameter>(_paramName);
            var percentValue = parameter.InitialValue * _percentValue / 100;
            if (_isIncreasing) {
                parameter.AddValue(percentValue);
            }
            else
            {
                parameter.RemoveValue(percentValue);
            }


        }
    }
}