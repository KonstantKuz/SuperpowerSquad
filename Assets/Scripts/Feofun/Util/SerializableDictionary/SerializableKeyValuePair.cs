using System;
using UnityEngine;

namespace Feofun.Util.SerializableDictionary
{
    [Serializable]
    public struct SerializableKeyValuePair<TKey, TValue>
    {
        [SerializeField]
        private TKey _key;
        
        [SerializeField]
        private TValue _value;

        public SerializableKeyValuePair(TKey key, TValue value)
        {
            _key = key;
            _value = value;
        }

        public TKey Key => _key;
        public TValue Value => _value;
    }
}