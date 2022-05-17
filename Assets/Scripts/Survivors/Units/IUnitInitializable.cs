using Survivors.Units.Player;
using Survivors.Units.Player.Model;

namespace Survivors.Units
{
    public interface IUnitInitializable
    {
        void Init(IUnitModel unitModel);
    }
}