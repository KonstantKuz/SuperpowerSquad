﻿using Feofun.Modifiers;
using Feofun.Modifiers.Parameters;
using Survivors.Units.Model;
using Survivors.Units.Modifiers;
using UniRx;

namespace Survivors.Units.Player.Model
{
    public class PlayerHealthModel: IHealthModel
    {
        private readonly FloatModifiableParameter _maxHealth;

        public PlayerHealthModel(float maxHealth, IModifiableParameterOwner parameterOwner)
        {
            _maxHealth = new FloatModifiableParameter(Parameters.HEALTH, maxHealth, parameterOwner);
        }

        public IReadOnlyReactiveProperty<float> MaxHealth => _maxHealth.ReactiveValue;
    }
}