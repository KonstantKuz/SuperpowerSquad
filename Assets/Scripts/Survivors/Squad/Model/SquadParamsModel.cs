using Feofun.Modifiers;
using Feofun.Modifiers.Parameters;
using Survivors.Modifiers;
using Survivors.Squad.Config;

namespace Survivors.Squad.Model
{
    public class SquadParamsModel : ModifiableParameterOwner
    {
        private readonly FloatModifiableParameter _speed;
        private readonly FloatModifiableParameter _collectRadius;
        
        public SquadParamsModel(SquadParams config)
        {
            _speed = new FloatModifiableParameter(Parameters.SPEED, config.Speed, this);
            _collectRadius = new FloatModifiableParameter(Parameters.COLLECT_RADIUS, config.CollectRadius, this);
        }
        
        public float Speed => _speed.Value;
        public float CollectRadius => _collectRadius.Value;
    }
}