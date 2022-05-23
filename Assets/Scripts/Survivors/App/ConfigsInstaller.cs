using Feofun.Config;
using Feofun.Config.Serializers;
using Feofun.Localization.Config;
using Survivors.Config;
using Survivors.EnemySpawn.Config;
using Survivors.Units.Enemy.Config;
using Survivors.Squad;
using Survivors.Squad.Upgrade;
using Survivors.Units.Modifiers;
using Survivors.Units.Player.Config;
using Zenject;

namespace Survivors.App
{
    public class ConfigsInstaller
    {
        public static void Install(DiContainer container)
        {
            new ConfigLoader(container, new CsvConfigDeserializer())
                .RegisterSingle<LocalizationConfig>(Configs.LOCALIZATION)
                .RegisterSingle<EnemyWavesConfig>(Configs.ENEMY_WAVES)
                .RegisterStringKeyedCollection<PlayerUnitConfig>(Configs.PLAYER_UNIT)
                .RegisterStringKeyedCollection<EnemyUnitConfig>(Configs.ENEMY_UNIT)
                .RegisterSingle<SquadConfig>(Configs.SQUAD)
                .RegisterStringKeyedCollection<ParameterUpgradeConfig>(Configs.MODIFIERS)
                .RegisterSingle<UpgradesConfig>(Configs.UPGRADES);
        }
    }
}