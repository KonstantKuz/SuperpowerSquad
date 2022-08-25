using System.Collections.Generic;
using DigitalRuby.ThunderAndLightning;
using Survivors.Location.ObjectFactory;
using UnityEngine;

namespace Survivors.Units.Weapon.Projectiles
{
    public class LightningBoltGenerator : MonoBehaviour
    {
        private struct BoltData
        {
            public LightningBoltPrefabScript Lightning { get; set; }
            public Transform EndPosition { get; set; }
        }

        [SerializeField]
        private LightningBoltPrefabScript _boltPrefab;

        private readonly List<BoltData> _lightnings = new List<BoltData>();
        
        public void Hit(ObjectInstancingFactory objectInstancingFactory, Transform container, Transform endPosition, float duration)
        {
            var boltData = CreateLightning(objectInstancingFactory, container, endPosition, duration);
            _lightnings.Add(boltData);
        }
        private BoltData CreateLightning(ObjectInstancingFactory objectInstancingFactory, Transform container, Transform endPosition, float duration)
        {
            var lightning = objectInstancingFactory.CreateObject(_boltPrefab.gameObject, container).GetComponent<LightningBoltPrefabScript>();
            lightning.transform.localPosition = Vector3.zero;
            lightning.Source.transform.localPosition = Vector3.zero;
            lightning.Destination.transform.position = endPosition.position;
            lightning.LifeTime = duration;
            return new BoltData {
                    Lightning = lightning,
                    EndPosition = endPosition,
            };
        }
        private void Update()
        {
            _lightnings.RemoveAll(it => it.Lightning == null);
            _lightnings.ForEach(it => {
                if (it.Lightning == null || it.EndPosition == null) {
                    return;
                }
                it.Lightning.Destination.transform.position = it.EndPosition.position;
            });
        }
    }
}