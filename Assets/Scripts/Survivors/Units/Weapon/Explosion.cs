﻿using System;
using System.Linq;
using Survivors.Location.Service;
using Survivors.Units.Component.Health;
using Survivors.Units.Target;
using UnityEngine;

namespace Survivors.Units.Weapon
{
    public class Explosion : MonoBehaviour
    {
        [SerializeField]
        private ParticleSystem[] _particles;

        private void Explode(float damageRadius, UnitType targetType, Action<GameObject> hitCallback)
        {
            PlayParticles();
            var hits = GetHits(damageRadius, targetType);
            DamageHits(hits, hitCallback);
        }

        private void PlayParticles()
        {
            foreach (var ps in _particles)
            {
                ps.Play();
            }
        }

        private Collider[] GetHits(float damageRadius, UnitType targetType)
        {
            var hits = Physics.OverlapSphere(transform.position, damageRadius);
            return hits.Where(hit => IsAliveEnemy(targetType, hit))
                       .ToArray();
        }

        private static bool IsAliveEnemy(UnitType targetType, Collider collider)
        {
            var target = collider.GetComponent<ITarget>();
            return target is { IsAlive: true } && target.UnitType == targetType;
        }

        private void DamageHits(Collider[] hits, Action<GameObject> hitCallback)
        {
            foreach (var hit in hits) {
                if (hit.TryGetComponent(out IDamageable _)) {
                    hitCallback?.Invoke(hit.gameObject);
                }
            }
        }

        public static void Explode(WorldObjectFactory objectFactory, 
            Explosion prefab, 
            Vector3 pos,
            float radius, 
            UnitType targetType,
            Action<GameObject> hitCallback)
        {
            var explosion = objectFactory.CreateObject(prefab.gameObject).GetComponent<Explosion>();
            explosion.transform.position = pos;
            explosion.Explode(radius, targetType, hitCallback);            
        }
    }
}