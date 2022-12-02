using Feofun.Extension;
using SuperMaxim.Core.Extensions;
using Survivors.Location;
using UnityEngine;
using Zenject;

namespace Survivors.Loot
{
    public class LootCollectorByTap : MonoBehaviour
    {
        [SerializeField]
        private float _radius = 1.5f;
        [Inject]
        private World _world;

        private LootCollector _lootCollector;     
        
        private LootCollector LootCollector => _lootCollector??= gameObject.RequireComponent<LootCollector>();

        
        private void Update()
        {
            if (!Input.GetMouseButtonDown(0)) return;

            var tapPosition = Input.mousePosition;
            var camera = UnityEngine.Camera.main;
            var ray = camera.ScreenPointToRay(tapPosition);
            var worldPosition = _world.GetGroundIntersection(ray);
            
            TryCollectLoots(worldPosition);
        }

        private void TryCollectLoots(Vector3 worldPosition)
        { 
            Physics.OverlapSphere(worldPosition, _radius)
                .ForEach(go => LootCollector.TryCollect(go));
        }
        
    }
}