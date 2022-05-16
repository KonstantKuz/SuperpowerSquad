using Feofun.Config;
using Feofun.Config.Serializers;
using Feofun.Localization.Config;
using Survivors.Config;
using Survivors.EnemySpawn.Config;
using Survivors.Units.Config;
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
                .RegisterSingle<EnemyUnitConfigs>(Configs.ENEMY_UNIT);
        }
    }
}