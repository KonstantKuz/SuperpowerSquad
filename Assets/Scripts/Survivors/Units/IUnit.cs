using System;
using Survivors.Units.Model;
using UnityEngine;

namespace Survivors.Units
{
    public interface IUnit
    {
        UnitType UnitType { get; }
        IUnitModel Model { get; }
        GameObject GameObject { get; }
        public bool IsActive { get; set; }
        event Action<IUnit> OnDeath;
        public void Init(IUnitModel model);
        public void Kill();
    }
}