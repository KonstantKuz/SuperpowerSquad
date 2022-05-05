using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Feofun.Config.Csv;
using JetBrains.Annotations;
using Survivors.Config;

namespace Feofun.Config
{
    public class ConfigCollection<TKey, TValue> : IEnumerable<TValue>, ILoadableConfig
    {
        private Dictionary<TKey, TValue> _map;

        [UsedImplicitly]
        public ConfigCollection()
        {
        }

        public ConfigCollection(IReadOnlyList<TValue> items)
        {
            Load(items);
        }

        public void Load(Stream stream)
        {
            Load(new CsvSerializer().ReadObjectArray<TValue>(stream));
        }

        private void Load(IReadOnlyList<TValue> items)
        {
            _map = items.ToDictionary(it => {
                var collectionItem = (ICollectionItem<TKey>) it;
                return collectionItem.Id;
            }, it => it);
        }

        public bool Contains(TKey id) => _map.ContainsKey(id);

        public TValue Get(TKey id)
        {
            if (!Contains(id)) {
                throw new NullReferenceException($"Config with id={id} of collection={GetType().Name} not found");
            }
            return _map[id];
        }

        public TValue Find(TKey id) => Contains(id) ? _map[id] : default;

        public List<TValue> Values => _map.Values.ToList();

        public IEnumerator<TValue> GetEnumerator()
        {
            return _map.Values.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}