
using System;
using Survivors.Units.Model;
using UnityEngine;

namespace Survivors.Units
{
    public interface IUnit
    {
        GameObject GameObject { get; }
        IUnitModel Model { get; }
        Action<IUnit> OnDeath { get; set; }
        public void Init(IUnitModel model);
        public void Kill();
    }
}