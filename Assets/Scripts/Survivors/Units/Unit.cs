using Survivors.Location.Data;
using Survivors.Units.Player.Model;
using UnityEngine;

namespace Survivors.Units
{
    public class Unit : MonoBehaviour, IWorldObject
    {
        public PlayerUnitModel UnitModel { get; private set; }
        public string ObjectId => GetComponent<WorldObject>().ObjectId;
        public GameObject GameObject => gameObject;
        public void Init(PlayerUnitModel model)
        {
            UnitModel = model;
            foreach (var component in GetComponentsInChildren<IUnitInitialization>()) {
                component.Init(this);
            }
        }
    }
}