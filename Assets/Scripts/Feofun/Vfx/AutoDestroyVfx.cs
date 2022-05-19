using System.Linq;
using UniRx;
using UniRx.Triggers;
using UnityEngine;

namespace Feofun.Vfx
{
    public class AutoDestroyVfx : MonoBehaviour
    {
        private ParticleSystem[] _particles;
        private readonly CompositeDisposable _disposable = new CompositeDisposable();
    
        private void Awake()
        {
            _particles = GetComponentsInChildren<ParticleSystem>();
            _particles.Select(it => it.OnDestroyAsObservable()).WhenAll()
                .Subscribe(it => Destroy(gameObject)).AddTo(_disposable);
        }

        private void OnDestroy()
        {
            _disposable?.Dispose();
        }
    }
}