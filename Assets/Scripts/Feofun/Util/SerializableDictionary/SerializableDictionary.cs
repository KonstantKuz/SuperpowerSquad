using System;
using System.Collections.Generic;
using UnityEngine;

namespace Feofun.Util.SerializableDictionary
{
    [Serializable]
    public class SerializableDictionary<TKey, TValue> : Dictionary<TKey, TValue>, ISerializationCallbackReceiver
    {
        [SerializeField]
        private List<SerializableKeyValuePair<TKey, TValue>> _map = new List<SerializableKeyValuePair<TKey, TValue>>();

        public void OnBeforeSerialize() { }

        public void OnAfterDeserialize()
        {
            Clear();
            foreach (var pair in _map) {
                this[pair.Key] = pair.Value;
            }
        }
    }
}