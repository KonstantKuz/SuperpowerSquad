using System.Runtime.Serialization;
using Feofun.Config;
using Feofun.Modifiers.Config;

namespace Survivors.Modifiers.Config
{
    public class ParameterUpgradeConfig : ICollectionItem<string>
    {
        [field: DataMember(Name = "Id")]
        public string Id { get; }

        [field: DataMember]
        public ModifierConfig ModifierConfig { get; }
    }
}