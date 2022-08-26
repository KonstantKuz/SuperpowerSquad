using UnityEngine;
using Zenject;

namespace Survivors.Units.Component.DamageReaction.Reactions
{
    public class DamageVibrationReaction : MonoBehaviour, IDamageReaction
    {
        [Inject]
        private VibrationManager _vibrationManager;

        public void OnDamageReaction()
        {
            _vibrationManager.VibrateLow();
        }
    }
}