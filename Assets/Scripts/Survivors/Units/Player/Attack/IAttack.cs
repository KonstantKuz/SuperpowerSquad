using System;
using Survivors.Units.Target;

namespace Survivors.Units.Player.Attack
{
    public interface IAttack
    {
        event Action OnAttack;
        void Attack(ITarget target);
    }
}