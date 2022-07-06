using System;
using Survivors.Player.Progress.Service;
using UniRx;
using UnityEngine;
using Zenject;

namespace Survivors.UI.Components.ActivatableObject.Conditions
{
    public class GameCountCondition : MonoBehaviour, ICondition
    {
        [SerializeField]
        public int _neededCount;

        [Inject]
        private PlayerProgressService _playerProgress;

        public IObservable<bool> IsAllow()
        {
            return _playerProgress.GameCount.Select(gameCount => gameCount >= _neededCount);
        }
    }
}