using Survivors.UI.Screen.Debriefing.Model;
using TMPro;
using UniRx;
using UnityEngine;

namespace Survivors.UI.Screen.Debriefing
{
    public class RewardMultiplier : MonoBehaviour
    {
        [SerializeField] private RewardAdsButtons _adsButtons;
        [SerializeField] private TextMeshProUGUI _rewardText;
        [SerializeField] private RectTransform _arrowContainer;
        [SerializeField] private int _moveSpeed;

        private CompositeDisposable _disposable;
        private RewardMultiplierModel _model;

        public void Init(RewardMultiplierModel model)
        {
            Dispose();
            _disposable = new CompositeDisposable();
            
            _model = model;
            _model.MoveSpeed = _moveSpeed;
            _model.MultiplierValue.Subscribe(UpdateRewardText).AddTo(_disposable);
            _adsButtons.Init(_model.OnGetRewardClick, _model.OnCancelClick);
        }

        private void UpdateRewardText(int multiplierValue)
        {
            var finalReward = _model.CoinsReward * multiplierValue;
            _rewardText.SetText(finalReward.ToString());
        }
        
        private void Update()
        {
            _arrowContainer.localEulerAngles = Vector3.forward * _model.ArrowAngle;
        }

        private void Dispose()
        {
            _disposable?.Dispose();
            _disposable = null;
        }

        private void OnDisable()
        {
            Dispose();
        }
    }
}
