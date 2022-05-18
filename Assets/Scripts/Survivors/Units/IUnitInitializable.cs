using Survivors.Units.Player;
using Survivors.Units.Player.Model;

namespace Survivors.Units
{
    public interface IUnitInitializable
    {
        public void Init(IUnit unit);
    }
}