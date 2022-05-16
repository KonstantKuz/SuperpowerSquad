using Survivors.GameWorld.Data;
using Survivors.Units.Player.Model;
using UnityEngine;

namespace Survivors.Units.Player
{
    public class PlayerUnit : MonoBehaviour, IWorldObject
    {
        private IUpdatableUnitComponent[] _updatables;
        public PlayerUnitModel Model { get; private set; }
        public string ObjectId => GetComponent<WorldObject>().ObjectId;
        public GameObject GameObject => gameObject;
        public void Init(PlayerUnitModel model)
        {
            Model = model;
            _updatables = GetComponentsInChildren<IUpdatableUnitComponent>();
            foreach (var component in GetComponentsInChildren<IUnitInitialization>()) {
                component.Init(this);
            }
        }
        private void Update()
        {
            UpdateComponents();
        }
        private void UpdateComponents()
        {
            for (int i = 0; i < _updatables.Length; i++) {
                _updatables[i].OnTick();
            }
        }
    }
}