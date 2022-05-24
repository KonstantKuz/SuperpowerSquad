using System.Collections.Generic;
using Feofun.Modifiers;
using Feofun.Modifiers.Parameters;
using Survivors.Squad.Config;
using Survivors.Units.Modifiers;
using Survivors.Units.Player.Config;

namespace Survivors.Squad.Model
{
    public class SquadParamsModel : IModifiableParameterOwner
    {
        private readonly SquadParams _config;
        
        private readonly FloatModifiableParameter _attackDamage;
        private readonly FloatModifiableParameter _attackTime;
        
        private readonly Dictionary<string, IModifiableParameter> _parameters = new Dictionary<string, IModifiableParameter>();
        
        public SquadParamsModel(PlayerAttackConfig config)
        {
            _config = config;
            _attackDamage = new FloatModifiableParameter(Parameters.ATTACK_DAMAGE, _config.AttackDamage, parameterOwner);
            _attackTime = new FloatModifiableParameter(Parameters.ATTACK_TIME, _config.AttackTime, parameterOwner);
            _projectileSpeed = new FloatModifiableParameter(Parameters.PROJECTILE_SPEED, _config.ProjectileSpeed, parameterOwner);
            _damageRadius = new FloatModifiableParameter(Parameters.DAMAGE_RADIUS, _config.DamageRadius, parameterOwner);
        }
        
        public float DamageRadius => _damageRadius.Value;

        public float AttackDamage => _attackDamage.Value;

        public IModifiableParameter GetParameter(string name)
        {
            
        }

        public void AddParameter(IModifiableParameter parameter)
        {
           
        }
    }
}