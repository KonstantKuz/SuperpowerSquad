using Feofun.Extension;
using Feofun.UI;
using SuperMaxim.Core.Extensions;
using SuperMaxim.Messaging;
using Survivors.Location.ObjectFactory.Factories;
using Survivors.Session.Messages;
using Survivors.Units;
using Survivors.Units.Messages;
using Survivors.Units.Service;
using UniRx;
using UnityEngine;
using UnityEngine.Assertions;
using Zenject;

namespace Survivors.UI.Hud
{
    public class DamagePopupPresenter : MonoBehaviour
    {
        [SerializeField] private DamagePopup _popupPrefab;

        private CompositeDisposable _disposable;

        [Inject] private IMessenger _messenger;
        [Inject] private UnitService _unitService;
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

            _messenger.SubscribeWithDisposable<EnemyDamagedMessage>(it => SpawnPopup(it.Unit, (int) it.Damage)).AddTo(_disposable);
            _messenger.SubscribeWithDisposable<SessionEndMessage>(it => Dispose()).AddTo(_disposable);
        }

        public void SpawnPopup(Units.Unit unit, int takenDamage)
        {
            if(!unit.SelfTarget.Center.position.IsInViewport()) return;
            
            var popup = _objectPoolFactory.Create<DamagePopup>(_popupPrefab.gameObject.name, _popupPrefab.gameObject, _uiRoot.HudContainer);
            popup.Init(takenDamage.ToString(), unit.SelfTarget.Center.position);
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