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
        private struct BoltData
        {
            public LightningBoltPrefabScript Lightning { get; set; }
            public Transform EndPosition { get; set; }
        }

        [SerializeField]
        private LightningBoltPrefabScript _boltPrefab;

        private readonly List<BoltData> _lightnings = new List<BoltData>();
        private CompositeDisposable _disposable = new CompositeDisposable();

        public void Hit(WorldObjectFactory objectFactory, Transform container, Transform endPosition, float duration)
        {
            var boltData = CreateLightning(objectFactory, container, endPosition);
            _lightnings.Add(boltData);
            Observable.Timer(TimeSpan.FromSeconds(duration)).Subscribe(it => Destroy(boltData)).AddTo(_disposable);
        }

        private void Destroy(BoltData data)
        {
            Destroy(data.Lightning);
            _lightnings.Remove(data);
        }

        private BoltData CreateLightning(WorldObjectFactory objectFactory, Transform container, Transform endPosition)
        {
            var lightning = objectFactory.CreateObject(_boltPrefab.gameObject, container).GetComponent<LightningBoltPrefabScript>();
            lightning.transform.localPosition = Vector3.zero;
            lightning.Source.transform.localPosition = Vector3.zero;
            lightning.Destination.transform.position = endPosition.position;
            return new BoltData {
                    Lightning = lightning,
                    EndPosition = endPosition,
            };
        }

        private void Update()
        {
            _lightnings.ForEach(it => {
                if (it.Lightning == null || it.EndPosition == null) {
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