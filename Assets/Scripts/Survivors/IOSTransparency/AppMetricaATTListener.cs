using System;
using UnityEngine;

namespace Survivors.IOSTransparency
{
    public class AppMetricaATTListener: IATTListener
    {
        public event Action OnStatusReceived;
        public void Init()
        {
            Debug.Log("Requesting Tracking Authorization with AppMetrica");
            AppMetrica.Instance.RequestTrackingAuthorization (status => {
                Debug.Log("Tracking Authorization received");
                OnStatusReceived?.Invoke();
            });
        }
    }
} 