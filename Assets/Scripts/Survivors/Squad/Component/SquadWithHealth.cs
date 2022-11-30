using Feofun.Components;
using Survivors.Squad.Model;
using Survivors.Units.Component.Health;
using UnityEngine;
using Zenject;

namespace Survivors.Squad.Component
{
    public class SquadWithHealth : Health, IInitializable<Squad>
    {
        private SquadHealthModel _healthModel;
        
        [Inject] private VibrationManager _vibrationManager;
        
        public bool IsAlive => CurrentValue.Value > 0;
        
        public void Init(Squad squad)
        {
            _healthModel = squad.Model.HealthModel as SquadHealthModel;
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
                ChangeHealth(_healthModel.Regeneration * Time.deltaTime);
            }
        }
    }
}