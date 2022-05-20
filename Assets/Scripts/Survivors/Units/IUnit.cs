using System;
using Survivors.Units.Model;
using UnityEngine;

namespace Survivors.Units
{
    public interface IUnit
    {
        UnitType UnitType { get; }
        IUnitModel Model { get; }  
        event Action<IUnit> OnDeath;
        public void Init(IUnitModel model);
        public void Kill();
    }
}