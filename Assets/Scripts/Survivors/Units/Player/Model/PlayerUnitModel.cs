using System;
using System.Collections.Generic;
using Feofun.Modifiers;
using Survivors.Units.Model;
using Survivors.Units.Player.Config;

namespace Survivors.Units.Player.Model
{
    public class PlayerUnitModel : IUnitModel, IModifiableParameterOwner
    {
        private readonly PlayerUnitConfig _config;
        private readonly PlayerAttackModel _playerAttackModel;
        private readonly Dictionary<string, IModifiableParameter> _parameters = new Dictionary<string, IModifiableParameter>();

        public PlayerUnitModel(PlayerUnitConfig config)
        {
            _config = config;
            HealthModel = new HealthModel(config.Health);
            _playerAttackModel = new PlayerAttackModel(config.PlayerAttackConfig, this);
        }

        public string Id => _config.Id;
        public HealthModel HealthModel { get; }
        public IAttackModel AttackModel => _playerAttackModel;

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