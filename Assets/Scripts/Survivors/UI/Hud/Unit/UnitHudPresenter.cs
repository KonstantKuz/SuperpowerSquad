using Feofun.UI;
using Survivors.Units.Component;
using Survivors.Units.Component.Hud;
using UnityEngine;
using Zenject;

namespace Survivors.UI.Hud.Unit
{
    public class UnitHudPresenter : MonoBehaviour
    {
        [SerializeField] private HealthBarView _healthBarView;
        
        private Transform _hudPlace;
        
        [Inject] private UIRoot _uiRoot;

        public void Init(UnitHudOwner unitHudOwner, Transform hudPlace)
        {
            transform.SetParent(_uiRoot.HudContainer);
            _hudPlace = hudPlace;
            
            InitHealthBar(unitHudOwner.HealthBarOwner);
        }

        public void OnUnitDeath()
        {
            Destroy(gameObject);
        }

        private void InitHealthBar(IHealthBarOwner healthBarOwner)
        {
            var model = new HealthBarModel(healthBarOwner);
            _healthBarView.Init(model);
        }

        private void Update()
        {
            if (_hudPlace == null)
            {
                return;
            }

            var worldToScreenPoint = UnityEngine.Camera.main.WorldToScreenPoint(_hudPlace.position);
            transform.position = worldToScreenPoint;
        }
    }
}