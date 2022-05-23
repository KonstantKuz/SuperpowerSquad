using UniRx;
using UnityEngine;

namespace Feofun.Modifiers.Parameters
{
    public class FloatModifiableParameter : IModifiableParameter
    {
        private readonly float _minValue;
        private readonly ReactiveProperty<float> _reactiveValue;
        
        private float _value;
        public float InitialValue { get; }
        public string Name { get; }
        public IReadOnlyReactiveProperty<float> ReactiveValue => _reactiveValue;

        public FloatModifiableParameter(string name, float initialValue, IModifiableParameterOwner owner, float minValue = 0)
        {
            Name = name;
            _minValue = minValue;
            _reactiveValue = new ReactiveProperty<float>();
            Value = InitialValue = initialValue;
            owner.AddParameter(this);
        }

        public void AddValue(float amount)
        {
            Value += amount;
        }

        public void OverrideValue(float value)
        {
            Value = value;
        }
        
        public void Reset()
        {
            Value = InitialValue;
        }
        public float Value
        {
            get => _value;
            //For parameters with max value = 100% we allow to overflow, cause in case of 50 + 50 + 30 - 20 we should have 100, not 80 as result
            //value of more than 100 should be checked at place of application and applied correctly
            private set
            {
                _value = Mathf.Max(value, _minValue);
                _reactiveValue.SetValueAndForceNotify(_value);
            }
        }


    }
}