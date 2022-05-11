using System.Collections.Generic;
using JetBrains.Annotations;

namespace Feofun.Config
{
    public class StringKeyedConfigCollection<TValue> : ConfigCollection<string, TValue>
    {
        [UsedImplicitly]
        public StringKeyedConfigCollection()
        {
        }

        public StringKeyedConfigCollection(IReadOnlyList<TValue> items) : base(items)
        {
        }
    }
}