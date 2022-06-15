using System;
using Unity.Advertisement.IosSupport;
using UnityEngine;

namespace Survivors.IOSTransparency
{
    public class IosATTListener: IATTListener
    {
        public event Action OnStatusReceived;
        public void Init()
        {
            Debug.Log("Requesting Tracking Authorization");
            var status = ATTrackingStatusBinding.GetAuthorizationTrackingStatus();
            if (status ==
                ATTrackingStatusBinding.AuthorizationTrackingStatus.NOT_DETERMINED)
            {
                ATTrackingStatusBinding.RequestAuthorizationTracking(OnStatusUpdated);
            }
            else
            {
                Debug.Log($"Tracking Authorization already received: {status}");
                OnStatusReceived?.Invoke();
            }
        }

        private void OnStatusUpdated(int status)
        {
            Debug.Log($"Tracking Authorization received: {status}");
            OnStatusReceived?.Invoke();
        }
    }
} 