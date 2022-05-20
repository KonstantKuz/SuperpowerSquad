using System;
using Survivors.Units.Model;
using UnityEngine;

namespace Survivors.Units
{
    public interface IUnit
    {
        event Action<IUnit> OnDeath;
        IUnitModel Model { get; }  
        GameObject Object { get; }
        UnitType UnitType { get; }
        public void Init(IUnitModel model);
        public void Kill();
    }
}