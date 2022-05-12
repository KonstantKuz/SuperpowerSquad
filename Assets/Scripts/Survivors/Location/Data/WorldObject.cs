using UnityEngine;

namespace Survivors.Location.Data
{
    public class WorldObject : MonoBehaviour, IWorldObject
    {
        [SerializeField]
        private string _objectId;

        public void Reset()
        {
            ObjectId = gameObject.name;
        }
        public string ObjectId
        {
            get => _objectId;
            set => _objectId = value;
        }
        public GameObject GameObject => gameObject;
    }
}