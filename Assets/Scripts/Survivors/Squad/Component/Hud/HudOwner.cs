using Survivors.UI.Hud.Unit;
using Survivors.Units;
using Survivors.Units.Component;
using UnityEngine;
using Zenject;

namespace Survivors.Squad.Component.Hud
{
    public class HudOwner : MonoBehaviour, ISquadInitializable, IUnitDeathEventReceiver
    {
        [SerializeField] private UnitHudPresenter _hudPrefab;
        [SerializeField] private Transform _hudPlace;
        
        private UnitHudPresenter _hudPresenter;
        private IHealthBarOwner _healthBarOwner;

        [Inject] private DiContainer _container;

        public IHealthBarOwner HealthBarOwner => _healthBarOwner ?? GetComponent<IHealthBarOwner>();
        public void Init(Squad squad)
        {
            _hudPresenter = _container.InstantiatePrefabForComponent<UnitHudPresenter>(_hudPrefab);
            _hudPresenter.Init(this, _hudPlace);
        }
        private void OnDestroy()
        {
            CleanUp();
        }
        public void OnDeath()
        {
            CleanUp();
        }
        private void CleanUp()
        {
            if (_hudPresenter == null) {
                return;
            }
            _hudPresenter.OnUnitDeath();
            _hudPresenter = null;

        }


    }
}
