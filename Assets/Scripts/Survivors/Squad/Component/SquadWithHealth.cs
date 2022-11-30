using Feofun.Components;
using Survivors.Squad.Config;
using Survivors.Units.Component.Health;
using UnityEngine;
using Zenject;

namespace Survivors.Squad.Component
{
    public class SquadWithHealth : Health, IInitializable<Squad>
    {
        [Inject] private VibrationManager _vibrationManager;
        [Inject] private SquadConfig _squadConfig;
        
        public bool IsAlive => CurrentValue.Value > 0;
        
        public void Init(Squad squad)
        {
            base.Init(squad.Model.HealthModel);
        }

        public override void TakeDamage(float damage, DamageUnits units)
        {
            base.TakeDamage(damage, units);
            _vibrationManager.VibrateHigh();
        }
        
        public void Update()
        {
            if (!IsAlive) return;
            if (CurrentValue.Value < MaxValue.Value) 
            {
                ChangeHealth(_squadConfig.Regeneration * Time.deltaTime);
            }
        }
    }
}