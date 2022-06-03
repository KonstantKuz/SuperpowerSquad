using Feofun.Components;
using Survivors.UI.Hud.Unit;
using Survivors.Units;
using Survivors.Units.Component;
using UniRx;
using UnityEngine;
using Zenject;

namespace Survivors.Squad.Component.Hud
{
    public class HudOwner : MonoBehaviour, IInitializable<Squad>
    {
        [SerializeField] private HudPresenter _hudPrefab;
        [SerializeField] private Transform _hudPlace;
        [SerializeField] private float _hudPlaceOffset;

        private HudPresenter _hudPresenter;
        private IHealthBarOwner _healthBarOwner;
        private CompositeDisposable _disposable;

        [Inject]
        private DiContainer _container;

        public IHealthBarOwner HealthBarOwner => _healthBarOwner ?? GetComponent<IHealthBarOwner>();

        public void Init(Squad squad)
        {
            CleanUp();
            _disposable = new CompositeDisposable();
            squad.UnitsCount.Subscribe(it=>UpdateHudPlaceOffset(squad.SquadRadius)).AddTo(_disposable);
            _hudPresenter = _container.InstantiatePrefabForComponent<HudPresenter>(_hudPrefab);
            _hudPresenter.Init(this, _hudPlace);
        }

        private void UpdateHudPlaceOffset(float radius)
        {
            _hudPresenter.UpdateHudPlaceOffset(radius * _hudPlaceOffset);
        }

        private void OnDestroy()
        {
            CleanUp();
        }

        private void CleanUp()
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