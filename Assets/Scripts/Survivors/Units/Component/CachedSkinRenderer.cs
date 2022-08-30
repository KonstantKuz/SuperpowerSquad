using System;
using System.Collections.Generic;
using ModestTree;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Survivors.Units.Component
{
    public class CachedSkinRenderer : MonoBehaviour
    {
        private const int FRAME_COUNT = 30;
        
        [SerializeField] private Animator _animator;
        [SerializeField] private SkinnedMeshRenderer _renderer;

        private float _currentTime;
        private float _animationLength;

        private static readonly Dictionary<int, Mesh> _meshCache = new Dictionary<int, Mesh>();
        public Bounds Bounds => _renderer.bounds;

        private void Awake()
        {
            if (_meshCache.IsEmpty())
            {
                PrepareCache();
            }
            
            _renderer.updateWhenOffscreen = false;
            _renderer.enabled = false;
            _animator.cullingMode = AnimatorCullingMode.AlwaysAnimate;
            _animator.enabled = false;
            _animationLength = _animator.GetCurrentAnimatorStateInfo(0).length;
            _currentTime = Random.Range(0f, _animationLength);
        }

        private void PrepareCache()
        {
            _renderer.updateWhenOffscreen = true;
            _animator.cullingMode = AnimatorCullingMode.AlwaysAnimate;

            var animState = _animator.GetCurrentAnimatorStateInfo(0);
            for (int frameNum = 0; frameNum <= FRAME_COUNT; frameNum++)
            {
                _animator.Update(animState.length / FRAME_COUNT);
                _meshCache[frameNum] = new Mesh();

                _renderer.BakeMesh(_meshCache[frameNum]);
            }
        }

        private void LateUpdate()
        {
            _currentTime = Mathf.Repeat(_currentTime + Time.deltaTime, _animationLength);
            int currentFrame = Mathf.RoundToInt( _currentTime / _animationLength * FRAME_COUNT );
            Graphics.DrawMesh(_meshCache[currentFrame], 
                transform.localToWorldMatrix, 
                _renderer.material, 
                gameObject.layer, 
                UnityEngine.Camera.main, 
                0, 
                null, 
                _renderer.shadowCastingMode, 
                _renderer.receiveShadows,
                gameObject.transform,
                _renderer.lightProbeUsage);
        }

        private void OnEnable()
        {
            _animator.cullingMode = AnimatorCullingMode.AlwaysAnimate;
        }

        private void OnDisable()
        {
            _animator.cullingMode = AnimatorCullingMode.CullCompletely;
        }
    }
}