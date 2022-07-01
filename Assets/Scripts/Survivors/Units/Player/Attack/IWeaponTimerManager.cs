using System;
using Survivors.Units.Model;

namespace Survivors.Units.Player.Attack
{
    public interface IWeaponTimerManager
    {
        void Subscribe(string weaponId, IAttackModel attackModel, Action onAttackReady);
        void Unsubscribe(string weaponId, Action onAttackReady);
    }
}