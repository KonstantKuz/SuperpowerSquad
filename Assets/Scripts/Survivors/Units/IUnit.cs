
using Survivors.Units.Model;

namespace Survivors.Units
{
    public interface IUnit
    {
        IUnitModel Model { get; }
        public void Init(IUnitModel model);

    }
}