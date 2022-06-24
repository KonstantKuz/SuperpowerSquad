﻿using System;
using Survivors.Location.Service;
using Survivors.Units.Weapon.Projectiles.Params;
using UnityEngine;
using Zenject;

namespace Survivors.Units.Weapon.Projectiles
{
    public class Meteor : MonoBehaviour
    {
        [SerializeField]
        private GameObject _hitVfx;
        
        private Action<GameObject> _hitCallback;
        private UnitType _targetType;
        private IProjectileParams _params;
        private float _timeLeft;
        private float _speed;
        
        [Inject]
        private WorldObjectFactory _objectFactory;        

        public void Launch(UnitType targetType, IProjectileParams projectileParams, float lifeTime, float speed, Action<GameObject> hitCallback)
        {
            _hitCallback = hitCallback;
            _targetType = targetType;
            _timeLeft = lifeTime;
            _speed = speed;
            _params = projectileParams;
        }
        
        private void Update()
        {
            _timeLeft -= Time.deltaTime;
            UpdatePosition();
            if (_timeLeft > 0) {
                return;
            }

            Projectile.TryHitTargetsInRadius(transform.position, _params.DamageRadius, _targetType, null, _hitCallback);
            PlayVfx(transform.position, Vector3.forward);
            Destroy();
        }
        private void UpdatePosition()
        {
            transform.position += transform.forward * _speed * Time.deltaTime;
        }
        
        private void PlayVfx(Vector3 pos, Vector3 up)
        {
            if (_hitVfx == null) return;
            var vfx = _objectFactory.CreateObject(_hitVfx);
            vfx.transform.SetPositionAndRotation(pos, Quaternion.LookRotation(up));
        }
        
        private void Destroy()
        {
            gameObject.SetActive(false);
            _hitCallback = null;
            Destroy(gameObject);
        }
    }
}