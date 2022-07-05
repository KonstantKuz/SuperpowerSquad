using Survivors.Player.Progress.Service;
using UnityEngine;
using Zenject;

namespace Survivors.UI.Components.ActivatableObject.Conditions
{
    public class LevelCondition : MonoBehaviour, ICondition
    {
        [SerializeField]
        public int _neededLevel;

        [Inject]
        private PlayerProgressService _playerProgress;

        public bool IsAllow()
        {
            return _playerProgress.Progress.LevelNumber >= _neededLevel;
        }
    }
}