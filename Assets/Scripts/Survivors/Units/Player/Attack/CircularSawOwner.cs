using System;
using System.Collections.Generic;
using Survivors.Extension;
using Survivors.Location.Service;
using Survivors.Units.Component.Health;
using Survivors.Units.Player.Model;
using Survivors.Units.Target;
using Survivors.Units.Weapon.Projectiles;
using UnityEngine;
using Zenject;

namespace Survivors.Units.Player.Attack
{
    public class CircularSawOwner : MonoBehaviour, IUnitInitializable
    {
        [SerializeField] private CircularSaw _circularSawPrefab;

        private Unit _owner;
        private Squad.Squad _squad;
        private PlayerAttackModel _playerAttackModel;
        private List<CircularSaw> _currentSaws;

        [Inject] private WorldObjectFactory _worldObjectFactory;
        
        public void Init(IUnit unit)
        {
            _owner = unit as Unit;
            if (!(unit.Model.AttackModel is PlayerAttackModel attackModel))
            {
                throw new ArgumentException("Unit must be a player unit.");
            }

            _playerAttackModel = attackModel;
            _squad = _owner.gameObject.RequireComponentInParent<Squad.Squad>();
            _currentSaws = new List<CircularSaw>();
            
            var projectileParams = attackModel.CreateProjectileParams();
            for (int i = 0; i < attackModel.ClipSize; i++)
            {
                AddNewSaw(projectileParams);
            }
        }

        public void AddNewSaw(ProjectileParams projectileParams)
        {
            var newSaw = CreateSaw();
            _currentSaws.Add(newSaw);
            newSaw.SetRotationCenter(_squad.Destination.transform);
            newSaw.Launch(_owner.SelfTarget, projectileParams, DoDamage);
            
            PlaceSaws();
        }

        private void PlaceSaws()
        {
            float angleStep = 360 / _currentSaws.Count;
            float currentPlaceAngle = 0;
            for (int i = 0; i < _currentSaws.Count; i++)
            {
                _currentSaws[i].SetPlaceAngle(currentPlaceAngle);
                currentPlaceAngle += angleStep;
            }
        }

        private void DoDamage(GameObject target)
        {
            var damageable = target.RequireComponent<IDamageable>();
            damageable.TakeDamage(_playerAttackModel.AttackDamage);
            Debug.Log($"Damage applied, target:= {target.name}");
        }

        private CircularSaw CreateSaw()
        {
            return _worldObjectFactory.CreateObject(_circularSawPrefab.gameObject).GetComponent<CircularSaw>();
        }
    }
}
