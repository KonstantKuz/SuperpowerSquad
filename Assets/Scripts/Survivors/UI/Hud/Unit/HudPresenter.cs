using Feofun.UI;
using Survivors.Units.Component;
using Survivors.Units.Component.Hud;
using UnityEngine;
using Zenject;

namespace Survivors.UI.Hud.Unit
{
    public class HudPresenter : MonoBehaviour
    {
        [SerializeField] private HealthBarView _healthBarView;
        
        private Transform _hudPlace;
        
        [Inject] private UIRoot _uiRoot;
        private float _hudPlaceOffset;

        public void Init<T>(HudOwner<T> hudOwner, Transform hudPlace) where T : class
        {
            transform.SetParent(_uiRoot.HudContainer);
            _hudPlace = hudPlace;
            _hudPlaceOffset = hudOwner.HudHeightOffset;   
            InitHealthBar(hudOwner.HealthBarOwner);
        }
        private void InitHealthBar(IHealthBarOwner healthBarOwner)
        {
            var model = new HealthBarModel(healthBarOwner);
            _healthBarView.Init(model);
        }
        public void UpdateHudPlaceOffset(float hudPlaceOffset)
        {
            _hudPlaceOffset = hudPlaceOffset;
        }
        private void Update()
        {
            if (_hudPlace == null) {
                return;
            }

            var worldToScreenPoint = UnityEngine.Camera.main.WorldToScreenPoint(_hudPlace.position + (Vector3.up * _hudPlaceOffset));
            transform.position = worldToScreenPoint;
        }
    }
}
