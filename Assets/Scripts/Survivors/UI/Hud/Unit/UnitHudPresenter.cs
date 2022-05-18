using Feofun.UI;
using Survivors.Units;
using Survivors.Units.Component;
using Survivors.Units.Component.Hud;
using UniRx;
using UnityEngine;
using Zenject;

namespace Survivors.UI.Hud.Unit
{
    public class UnitHudPresenter : MonoBehaviour
    {
        [SerializeField] private HealthBarView _healthBarView;
        
        private CompositeDisposable _disposable;
        private Transform _hudPlace;
        
        [Inject] private HudContainer _hudContainer;

        private static int HudCount;
        
        public void Init(HudOwner hudOwner, Transform hudPlace)
        {
            HudCount++;
            hudPlace.gameObject.name += HudCount;
            
            _disposable?.Dispose();
            _disposable = new CompositeDisposable();

            transform.SetParent(_hudContainer.transform);
            _hudPlace = hudPlace;
            
            InitHealthBar(hudOwner.HealthBarOwner);
        }

        public void OnDeath()
        {
            _disposable?.Dispose();
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
