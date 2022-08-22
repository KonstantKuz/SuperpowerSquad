using DG.Tweening;
using Logger.Extension;
using Survivors.Extension;
using Survivors.Location.Service;
using Survivors.Units;
using Survivors.Units.Component.Health;
using Survivors.Units.Weapon.Projectiles;
using UnityEngine;
using UnityEngine.Rendering;
using Zenject;

namespace Survivors.WorldEvents.Events.Avalanche
{
    public class Cobblestone : MonoBehaviour
    {
        [SerializeField] private float _disappearTime;
        [SerializeField] private float _maxDistance;
        [SerializeField] private float _moveSpeed;
        [SerializeField] private Renderer _stoneRenderer;
        [SerializeField] private GameObject _trajectoryPrefab;

        private float _damagePercent;
        private bool _isAppeared;
        private float _radius;
        private Vector3 _moveDirection;
        private float _distanceTraveled;
        private LineRenderer _trajectory;
        private Sequence _appearTween;
        private Tween _destroyTween;

        [Inject] private WorldObjectFactory _worldObjectFactory;
        
        private float DistanceToDisappear => _maxDistance - _moveSpeed * _disappearTime;
        public float Radius => _radius;
        private bool IsTimeToDestroy => _distanceTraveled >= DistanceToDisappear && _destroyTween == null;
        private Color Transparent => new Color(1,1,1,0);
        
        private void Awake()
        {
            _radius = _stoneRenderer.bounds.size.x / 2;
            _stoneRenderer.material.color = Transparent;
            _stoneRenderer.shadowCastingMode = ShadowCastingMode.Off;
        }

        public void Launch(Vector3 direction, float damagePercent)
        {
            Dispose();
            
            _damagePercent = damagePercent;
            _moveDirection = direction;
            transform.forward = direction;

            SpawnTrajectory();
            SetTrajectoryPosition();
            PlayAppear();
        }

        private void SpawnTrajectory()
        {
            _trajectory = _worldObjectFactory.CreateObject(_trajectoryPrefab).GetComponent<LineRenderer>();
        }

        private void SetTrajectoryPosition()
        {
            var groundedPosition = transform.position - Vector3.up * _radius;
            var trajectoryPositions = new [] { groundedPosition, groundedPosition + _maxDistance * _moveDirection };
            _trajectory.SetPositions(trajectoryPositions);
        }

        private void PlayAppear()
        {
            _appearTween = DOTween.Sequence();
            _appearTween.Join(PlayStoneAppear());
            _appearTween.Join(PlayTrajectoryAppear());
            _appearTween.onComplete = () => { _isAppeared = true; };
        }

        private Tween PlayStoneAppear()
        {
            var stoneAppear = _stoneRenderer.material.DOColor(Color.white, _disappearTime);
            stoneAppear.onComplete = () =>
            {
                _stoneRenderer.shadowCastingMode = ShadowCastingMode.On;
            };
            return stoneAppear;
        }

        private Tween PlayTrajectoryAppear()
        {
            var initialColor = _trajectory.material.color;
            _trajectory.material.color = Transparent;
            return _trajectory.material.DOColor(initialColor, _disappearTime);
        }

        private void Update()
        {
            if (!_isAppeared)  return;
         
            Move();

            if (IsTimeToDestroy)
            {
                Destroy();
            }
        }

        private void Move()
        {
            var distance = _moveSpeed * Time.deltaTime;
            _distanceTraveled += distance;
            transform.position += _moveDirection * distance;

            var angle= (distance * 180) / (_radius * Mathf.PI);
            transform.localRotation *= Quaternion.Euler(Vector3.right * angle);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!CanDamageTarget(other))
            {
                return;
            }

            DoDamage(other.gameObject);
        }
        
        private void DoDamage(GameObject target)
        {
            var damageable = target.RequireComponent<IDamageable>();
            damageable.TakeDamage(_damagePercent, DamageUnits.PercentFromMax);
            if (target.name.Contains("Simple")) return;
            this.Logger().Trace($"Cobblestone, damage applied, target:= {target.name}");
        }

        private bool CanDamageTarget(Collider other)
        {
            return Projectile.CanDamageTarget(other, UnitType.ENEMY, out var enemy) ||
                   Projectile.CanDamageTarget(other, UnitType.PLAYER, out var player);
        }

        private void Destroy()
        {
            _destroyTween = PlayDisappear();
            _destroyTween.onComplete = () =>
            {
                Destroy(_trajectory.gameObject);
                Destroy(gameObject);
            };
        }

        private Tween PlayDisappear()
        {
            _stoneRenderer.shadowCastingMode = ShadowCastingMode.Off;
            var disappear = DOTween.Sequence();
            disappear.Insert(0,_stoneRenderer.material.DOFade(0, _disappearTime));
            disappear.Insert(0,_trajectory.material.DOFade(0, _disappearTime));
            return disappear;
        }

        private void OnDestroy()
        {
            Dispose();
        }

        private void Dispose()
        {
            _appearTween.Kill();
            _appearTween = null;
            _destroyTween.Kill();
            _destroyTween = null;
        }
    }
}
