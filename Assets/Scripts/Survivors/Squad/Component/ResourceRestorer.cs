using System;
using System.Collections.Generic;
using System.Linq;
using Feofun.Components;
using Survivors.Squad.Data;
using Survivors.Squad.Model;
using Survivors.Squad.Service;
using UnityEngine;
using Zenject;

namespace Survivors.Squad.Component
{
    public class ResourceRestorer : MonoBehaviour,  IInitializable<Squad>
    {
        private readonly Dictionary<SquadProgressType, float> _increments = new Dictionary<SquadProgressType, float>()
        {
            { SquadProgressType.Exp, 0 },
            { SquadProgressType.Token, 0 },
        };
        
        private SquadModel _squadModel;

        [Inject]
        private SquadProgressService _squadProgressService;
        
        public void Init(Squad owner)
        {
            _squadModel = owner.Model;
        }
        public void Update() => _increments.Keys.ToList().ForEach(Regeneration);

        private void Regeneration(SquadProgressType type)
        {
            _increments[type] += GetRegenerationValue(type) * Time.deltaTime;
            if (_increments[type] < 1) return;
            _squadProgressService.Add(type, 1);
            _increments[type] -= 1;
        }

        private float GetRegenerationValue(SquadProgressType type)
        {
            switch (type)
            {
                case SquadProgressType.Exp:
                    return _squadModel.ExpRegeneration.Value;
                case SquadProgressType.Token:
                    return _squadModel.TokenRegeneration.Value;
                default:
                    throw new ArgumentOutOfRangeException($"Unexpected squadProgressType:= {type}");
            }
        }

    }
}