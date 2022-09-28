using Feofun.Localization;
using SuperMaxim.Messaging;
using Survivors.Enemy.Messages;
using Survivors.UI.Screen.World.WorldEvent;
using UnityEngine;
using Zenject;

namespace Survivors.UI.Screen.World.Boss
{
    public class BossAlertPresenter : MonoBehaviour
    {
        [SerializeField]
        private AlertView _alertView;

        [Inject]
        public IMessenger _messenger;

        public void OnEnable()
        {
            _messenger.Subscribe<BossAlertShowingMessage>(OnShowingBossAlert);
        }

        private void OnShowingBossAlert(BossAlertShowingMessage evn)
        {
            var model = new AlertViewModel(evn.ShowDuration);
            _alertView.Init(model);
        }
        public void OnDisable()
        {
            _messenger.Unsubscribe<BossAlertShowingMessage>(OnShowingBossAlert);
        }
        

    }
}