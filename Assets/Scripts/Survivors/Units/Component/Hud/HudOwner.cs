using Survivors.UI.Hud.Unit;
using UniRx;
using UnityEngine;
using Zenject;

namespace Survivors.Units.Component.Hud
{
    public class HudOwner : MonoBehaviour
    {
        [SerializeField] private HudPresenter _hudPrefab;
        [SerializeField] private Transform _hudPlace;
        [SerializeField] private float _hudHeightOffset;
        
        private HudPresenter _hudPresenter;
        private IHealthBarOwner _healthBarOwner;
        private CompositeDisposable _disposable;

        [Inject]
        private DiContainer _container;

        public float HudHeightOffset => _hudHeightOffset;
        public IHealthBarOwner HealthBarOwner => _healthBarOwner ??= GetComponent<IHealthBarOwner>();

        public void CreateHud()
        {
            Dispose();
            _disposable = new CompositeDisposable();
            _hudPresenter = _container.InstantiatePrefabForComponent<HudPresenter>(_hudPrefab);
            _hudPresenter.Init(this, _hudPlace);
        }

        private void OnDestroy()
        {
            Dispose();
        }

        private void OnDisable()
        {
            Dispose();
        }

        private void Dispose()
        {
            _disposable?.Dispose();
            _disposable = null;

            if (_hudPresenter == null) {
                return;
            }
            Destroy(_hudPresenter.gameObject);
            _hudPresenter = null;
        }
    }
}