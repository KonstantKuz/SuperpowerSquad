using Survivors.Units.Player;
using Survivors.Units.Player.Model;

namespace Survivors.Units
{
    public interface IUnitInitializable<U, M> where U : IUnit<M> where M : IUnitModel
    {
        public void Init(U unit);
    }
}