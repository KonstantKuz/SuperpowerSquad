using System;
using UniRx;
using UnityEngine;

namespace Survivors.UI.Components.ActivatableObject
{
    public class ActivatableObject : MonoBehaviour
    {
        private IDisposable _disposable;
        
        public void Init(IObservable<bool> model)
        {
            CleanUp();
            _disposable = model.Subscribe(value => gameObject.SetActive(value));
        }

        public void Init(bool isActive)
        {
            CleanUp();
            gameObject.SetActive(isActive);
        }

        private void CleanUp()
        {
            _disposable?.Dispose();
            _disposable = null;
        }

        private void OnDestroy()
        {
            CleanUp();
        }
    }
}