using Survivors.UI.Hud.Unit;
using UnityEngine;
using Zenject;

namespace Survivors.Units.Component.Hud
{
    public class UnitHudOwner : MonoBehaviour, IUnitInitializable, IUnitDeathEventReceiver
    {
        [SerializeField] private UnitHudPresenter _hudPrefab;
        [SerializeField] private Transform _hudPlace;
        
        private IHealthBarOwner _healthBarOwner;
        private UnitHudPresenter _hudPresenter;

        [Inject] private DiContainer _container;

        public IHealthBarOwner HealthBarOwner => _healthBarOwner ?? GetComponent<IHealthBarOwner>();
        
        public void Init(IUnit unit)
        {
            _hudPresenter = _container.InstantiatePrefabForComponent<UnitHudPresenter>(_hudPrefab);
            _hudPresenter.Init(this, _hudPlace);
        }

        public void OnDeath()
        {
            _hudPresenter.OnUnitDeath();
        }
    }
}
