using System.Collections;
using DG.Tweening;
using Logger.Extension;
using Survivors.Extension;
using Survivors.Units;
using Survivors.Units.Component.Health;
using Survivors.Units.Weapon.Projectiles;
using UnityEngine;
using UnityEngine.Rendering;

namespace Survivors.WorldEvents.Avalanche
{
    public class Cobblestone : MonoBehaviour
    {
        [SerializeField] private float _disappearTime;
        [SerializeField] private float _maxDistance;
        [SerializeField] private float _damagePercent;
        [SerializeField] private float _moveSpeed;
        [SerializeField] private Renderer _stoneRenderer;
        [SerializeField] private Material _trajectoryMaterial;

        private bool _isLaunched;
        private float _radius;
        private Vector3 _moveDirection;
        private float _distanceTraveled;
        private LineRenderer _trajectory;
        private Tween _destroyTween;

        private float DistanceToDisappear => _maxDistance - _moveSpeed * _disappearTime;

        private void Awake()
        {
            _stoneRenderer.material.color = Color.clear;
            _stoneRenderer.shadowCastingMode = ShadowCastingMode.Off;
        }

        private IEnumerator Start()
        {
            yield return new WaitForSeconds(3);
            Launch();
        }

        private void Launch()
        {
            Init();
            SpawnTrajectory();
            _stoneRenderer.material.DOColor(Color.white, _disappearTime).onComplete = () =>
            {
                _stoneRenderer.shadowCastingMode = ShadowCastingMode.On;
                _isLaunched = true;
            };
        }

        private void Init()
        {
            _radius = transform.localScale.x / 2;
            _moveDirection = transform.forward;
        }

        private void SpawnTrajectory()
        {
            var groundedPosition = transform.position - Vector3.up * _radius;
            var trajectoryPositions = new [] { groundedPosition, groundedPosition + _maxDistance * _moveDirection };
            _trajectory = new GameObject().AddComponent<LineRenderer>();
            _trajectory.SetPositions(trajectoryPositions);
            _trajectory.material = _trajectoryMaterial;
            _trajectory.material.color = Color.clear;
            _trajectory.material.DOColor(_trajectoryMaterial.color, _disappearTime);
        }

        private void Update()
        {
            if (!_isLaunched)  return;
         
            Move();

            if (_distanceTraveled >= DistanceToDisappear && _destroyTween == null)
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
            transform.rotation *= Quaternion.Euler(Vector3.right * angle);
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
            var health = target.GetComponent<Health>() ?? target.GetComponentInParent<Health>();
            var damageable = target.RequireComponent<IDamageable>();
            var damage = health.MaxValue.Value * (_damagePercent / 100);
            damageable.TakeDamage(damage);
            this.Logger().Trace($"Damage applied, target:= {target.name}");
        }
        
        private bool CanDamageTarget(Collider other)
        {
            return Projectile.CanDamageTarget(other, UnitType.ENEMY, out var enemy) ||
                   Projectile.CanDamageTarget(other, UnitType.PLAYER, out var player);
        }

        private void Destroy()
        {
            _stoneRenderer.shadowCastingMode = ShadowCastingMode.Off;

            var sequence = DOTween.Sequence();
            sequence.Insert(0,_stoneRenderer.material.DOColor(Color.clear, _disappearTime));
            sequence.Insert(0,_trajectory.material.DOColor(Color.clear, _disappearTime));
            sequence.onComplete = () =>
            {
                _isLaunched = false;
                Destroy(_trajectory.gameObject);
                Destroy(gameObject);
            };

            _destroyTween = sequence;
        }

        private void OnDestroy()
        {
            _destroyTween.Kill();
            _destroyTween = null;
        }
    }
}
