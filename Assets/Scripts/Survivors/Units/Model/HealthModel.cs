using System;
using Survivors.Units.Config;
using Survivors.Units.Player.Config;

namespace Survivors.Units.Model
{
    public class HealthModel : IUnitHealthModel
    {
        public int MaxHealth { get; }

        public HealthModel(int maxHealth)
        {
            MaxHealth = maxHealth;
        }
    }
}