using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using ModestTree;
using SuperMaxim.Core.Extensions;
using UnityEngine;
using AppContext = Feofun.App.AppContext;

namespace Survivors.Location
{
    public class World : RootContainer
    {
        [SerializeField]
        private Transform _ground;
        [SerializeField]
        private GameObject _spawn;
        
        private Bounds? _groundBounds;

        public Transform Ground => _ground;
        private Bounds GroundBounds => _groundBounds ??= _ground.GetComponent<Collider>().bounds;

        public GameObject Spawn => _spawn;

        [CanBeNull]
        public Squad.Squad Squad { get; set; }


        public Squad.Squad GetSquad()
        {
            Assert.IsNotNull(Squad, "Squad is null, should call this method only inside game session");
            return Squad;
        }

        public bool IsPaused => Time.timeScale == 0;

        public Vector3 GetGroundIntersection(Ray withRay)
        {
            var plane = new Plane(Ground.up, Ground.position);
            plane.Raycast(withRay, out var intersectionDist);
            return withRay.GetPoint(intersectionDist);
        }

        public Vector3 ClampByWorldBBox(Vector3 position)
        {
            position.x = Mathf.Clamp(position.x, GroundBounds.min.x, GroundBounds.max.x);
            position.z = Mathf.Clamp(position.z, GroundBounds.min.z, GroundBounds.max.z);
            return position;
        }
        
        public void Pause()
        {
            Time.timeScale = 0;
        }

        public void UnPause()
        {
            Time.timeScale = 1;
        }

        public void Setup()
        {
            GetAllOf<IWorldScope>().ForEach(it => it.OnWorldSetup());
        }

        public void CleanUp()
        {
            GetAllOf<IWorldScope>().ForEach(it => it.OnWorldCleanUp());
            Squad = null;
        }

        private IEnumerable<T> GetAllOf<T>()
        {
            return GetDISubscribers<T>().Union(GetChildrenSubscribers<T>());
        }

        private static List<T> GetDISubscribers<T>() => AppContext.Container.ResolveAll<T>();
    }
}