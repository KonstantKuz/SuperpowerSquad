using System;
using System.Collections.Generic;
using System.Linq;
using SuperMaxim.Core.Extensions;
using UniRx;
using UnityEngine;

namespace Survivors.UI.Screen.Debriefing.Model
{
    public class RewardMultiplierModel
    {
        private const float MAX_ARROW_ANGLE = 50f;
        private const float X5_MAX_ANGLE = 15f;
        private const float X4_MAX_ANGLE = 26f;
        private const float X3_MAX_ANGLE = 38f;
        private const int MIN_MULTIPLIER = 2;
        private static readonly List<(float angle, int multiplier)> MultiplierUnderAngle = new List<(float angle, int multiplier)>
        {
            (X5_MAX_ANGLE, 5),
            (X4_MAX_ANGLE, 4),
            (X3_MAX_ANGLE, 3),
        };
        
        private readonly IntReactiveProperty _multiplierValue;
        private bool _isStopped;
        
        public float MoveSpeed { get; set; }
        public int CoinsReward { get; }
        public float ArrowAngle { get; private set; }
        public IntReactiveProperty MultiplierValue => _multiplierValue;
        public Action OnGetRewardClick { get; }
        public Action OnCancelClick { get; }

        public RewardMultiplierModel(int coinsReward, Action onGetRewardClick, Action onCancelClick)
        {
            _multiplierValue = new IntReactiveProperty();
            CoinsReward = coinsReward;
            OnGetRewardClick = onGetRewardClick;
            OnCancelClick = onCancelClick;
        }
        
        public void UpdateMultiplierValue()
        {
            if(_isStopped) return;
            
            UpdateArrowAngle();
            UpdateMultiplier();
        }

        public void Stop()
        {
            _isStopped = true;
        }

        private void UpdateArrowAngle()
        {
            if (Mathf.Abs(ArrowAngle) >= MAX_ARROW_ANGLE)
            {
                MoveSpeed = -MoveSpeed;
            }
            ArrowAngle += MoveSpeed * Time.deltaTime;
        }

        private void UpdateMultiplier()
        {
            var currentMultiplierValue = GetCurrentMultiplier();
            if (_multiplierValue.Value != currentMultiplierValue)
            {
                _multiplierValue.SetValueAndForceNotify(currentMultiplierValue);
            }
        }

        private int GetCurrentMultiplier()
        {
            foreach (var (angle, multiplier) in MultiplierUnderAngle) 
            {
                if (Mathf.Abs(ArrowAngle) < angle) return multiplier;
            }
            return MIN_MULTIPLIER;
        }
    }
}