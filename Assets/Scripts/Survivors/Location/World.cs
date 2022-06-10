using System.Collections.Generic;
using System.Linq;
using Feofun.App;
using JetBrains.Annotations;
using SuperMaxim.Core.Extensions;
using UnityEngine;

namespace Survivors.Location
{
    public class World : RootContainer
    {
        [SerializeField]
        private Transform _ground;
        [SerializeField]
        private GameObject _spawn;

        public Transform Ground => _ground;
        public GameObject Spawn => _spawn;
    
        [CanBeNull]
        public Squad.Squad Squad { get; set; }

        public Vector3 GetGroundIntersection(Ray withRay)
        {
            var plane = new Plane(Ground.up, Ground.position);
            plane.Raycast(withRay, out var intersectionDist);
            return withRay.GetPoint(intersectionDist);
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