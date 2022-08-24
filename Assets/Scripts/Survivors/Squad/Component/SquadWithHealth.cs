using Feofun.Components;
using Survivors.Units.Component.Health;
using Survivors.Vibration;
using Zenject;

namespace Survivors.Squad.Component
{
    public class SquadWithHealth : Health, IInitializable<Squad>
    {
        [Inject] private VibrationManager _vibrationManager;
        
        public void Init(Squad squad)
        {
            base.Init(squad.Model.HealthModel);
        }

        public override void TakeDamage(float damage, DamageUnits units)
        {
            base.TakeDamage(damage, units);
            _vibrationManager.VibrateHigh();
        }
    }
}