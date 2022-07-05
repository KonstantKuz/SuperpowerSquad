using Survivors.Player.Progress.Service;
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

        public bool IsAllow()
        {
            return _playerProgress.Progress.GameCount >= _neededCount;
        }
    }
}