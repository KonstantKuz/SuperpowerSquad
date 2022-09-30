using Feofun.Extension;
using Feofun.UI;
using SuperMaxim.Messaging;
using Survivors.Location.ObjectFactory.Factories;
using Survivors.Session.Messages;
using Survivors.Units;
using Survivors.Units.Messages;
using UniRx;
using UnityEngine;
using Zenject;

namespace Survivors.UI.Hud
{
    public class DamagePopupPresenter : MonoBehaviour
    {
        [SerializeField] private string _popupPrefabId;
        [SerializeField] private float _valueMultiplayer = 1;

        private CompositeDisposable _disposable;

        [Inject] private IMessenger _messenger;
        [Inject] private UIRoot _uiRoot;
        [Inject] private ObjectPoolFactory _objectPoolFactory;
        
        private void OnEnable()
        {
            Init();
        }

        public void Init()
        {
            Dispose();
            _disposable = new CompositeDisposable();

            _messenger.SubscribeWithDisposable<UnitDamagedMessage>(it => SpawnPopup(it.Unit, (int) it.Damage)).AddTo(_disposable);
            _messenger.SubscribeWithDisposable<SessionEndMessage>(it => Dispose()).AddTo(_disposable);
        }

        public void SpawnPopup(Units.Unit unit, int takenDamage)
        {
            if(unit.UnitType != UnitType.ENEMY) return;
            if(!unit.SelfTarget.Center.position.IsInViewport()) return;
            
            var popup = _objectPoolFactory.Create<DamagePopup>(_popupPrefabId, _uiRoot.HudContainer);
            popup.Init(Mathf.CeilToInt(takenDamage * _valueMultiplayer).ToString(), unit.SelfTarget.Center.position);
            var popupTween = popup.PlayPopup();
            popupTween.onComplete = () => _objectPoolFactory.Destroy(popup.gameObject);
            popupTween.ToDisposable(true).AddTo(_disposable);
        }

        private void OnDisable()
        {
            Dispose();
        }

        public void Dispose()
        {
            _disposable?.Dispose();
            _disposable = null;
        }
    }
}