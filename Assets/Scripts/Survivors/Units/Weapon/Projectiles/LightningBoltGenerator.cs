using System;
using System.Collections.Generic;
using DigitalRuby.ThunderAndLightning;
using Survivors.Location.Service;
using UniRx;
using UnityEngine;

namespace Survivors.Units.Weapon.Projectiles
{
    public class LightningBoltGenerator : MonoBehaviour
    {
        private struct LightningBoltData
        {
            public LightningBoltPrefabScript Lightning { get; set; }
            public Transform EndPosition { get; set; }
        }

        [SerializeField]
        private LightningBoltPrefabScript _lightningBoltPrefab;

        private readonly List<LightningBoltData> _lightnings = new List<LightningBoltData>();
        private CompositeDisposable _disposable = new CompositeDisposable();

        public void Hit(WorldObjectFactory objectFactory, Transform container, Transform endPosition, float duration)
        {
            var lightningData = CreateLightning(objectFactory, container, endPosition);
            _lightnings.Add(lightningData);
            Observable.Timer(TimeSpan.FromSeconds(duration)).Subscribe(it => Destroy(lightningData)).AddTo(_disposable);
        }

        private void Destroy(LightningBoltData lightningData)
        {
            Destroy(lightningData.Lightning);
            _lightnings.Remove(lightningData);
        }

        private LightningBoltData CreateLightning(WorldObjectFactory objectFactory, Transform container, Transform endPosition)
        {
            var lightningBolt = objectFactory.CreateObject(_lightningBoltPrefab.gameObject, container).GetComponent<LightningBoltPrefabScript>();
            lightningBolt.transform.localPosition = Vector3.zero;
            lightningBolt.Source.transform.localPosition = Vector3.zero;
            lightningBolt.Destination.transform.position = endPosition.position;
            return new LightningBoltData {
                    Lightning = lightningBolt,
                    EndPosition = endPosition,
            };
        }

        private void Update()
        {
            _lightnings.ForEach(it => {
                if (it.Lightning == null || it.EndPosition) {
                    return;
                }
                it.Lightning.Destination.transform.position = it.EndPosition.position;
            });
        }

        private void OnDestroy()
        {
            _disposable?.Dispose();
            _disposable = null;
        }
    }
}