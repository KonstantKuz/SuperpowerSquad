
using System;
using Survivors.Units.Model;

namespace Survivors.Units
{
    public interface IUnit
    {
        IUnitModel Model { get; }
        Action<IUnit> OnDeath { get; set; }
        public void Init(IUnitModel model);
        public void Kill();
    }
}