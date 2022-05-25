using System;
using System.Collections.Generic;

namespace Feofun.Modifiers
{
    public class ModifiableParameterOwner : IModifiableParameterOwner
    {
        private readonly Dictionary<string, IModifiableParameter> _parameters = new Dictionary<string, IModifiableParameter>();
        
        public void AddModifier(IModifier modifier)
        {
            modifier.Apply(this);
        }
        public IModifiableParameter GetParameter(string name)
        {
            if (!_parameters.ContainsKey(name))
            {
                throw new Exception($"No modifiable parameter named {name}");
            }
            return _parameters[name];
        }
        
        public void AddParameter(IModifiableParameter parameter)
        {
            if (_parameters.ContainsKey(parameter.Name))
            {
                throw new Exception($"UnitModel already have parameter named {parameter.Name}");
            }
            _parameters.Add(parameter.Name, parameter);
        }
    }
}