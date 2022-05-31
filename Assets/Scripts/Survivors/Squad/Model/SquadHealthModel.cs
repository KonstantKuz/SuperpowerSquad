﻿using Feofun.Modifiers;
using Feofun.Modifiers.Parameters;
using Survivors.Modifiers;
using Survivors.Units.Model;
using UniRx;

namespace Survivors.Squad.Model
{
    public class SquadHealthModel : IHealthModel
    {
        private readonly FloatModifiableParameter _maxHealth;

        public SquadHealthModel(IModifiableParameterOwner parameterOwner, float startingMaxHealth)
        {
            StartingMaxHealth = startingMaxHealth;
            _maxHealth = new FloatModifiableParameter(Parameters.HEALTH, 0, parameterOwner);
        }

        public float StartingMaxHealth { get; }
        public IReadOnlyReactiveProperty<float> MaxHealth => _maxHealth.ReactiveValue;
    }
}