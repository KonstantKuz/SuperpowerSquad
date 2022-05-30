using Survivors.UI.Hud.Unit;
using Survivors.Units;
using Survivors.Units.Component;
using UnityEngine;
using Zenject;

namespace Survivors.Squad.Component.Hud
{
    public class HudOwner : MonoBehaviour, IInitializable<Squad>
    {
        [SerializeField] private UnitHudPresenter _hudPrefab;
        [SerializeField] private Transform _hudPlace;
        
        private UnitHudPresenter _hudPresenter;
        private IHealthBarOwner _healthBarOwner;

        [Inject] private DiContainer _container;

        public IHealthBarOwner HealthBarOwner => _healthBarOwner ?? GetComponent<IHealthBarOwner>();
        public void Init(Squad squad)
        {
            CleanUp();
            _hudPresenter = _container.InstantiatePrefabForComponent<UnitHudPresenter>(_hudPrefab);
            _hudPresenter.Init(this, _hudPlace);
        }
        private void OnDestroy()
        {
            CleanUp();
        }
        private void CleanUp()
        {
            if (_hudPresenter == null) {
                return;
            }
            Destroy(_hudPresenter.gameObject);
            _hudPresenter = null;
        }
        
    }
}
