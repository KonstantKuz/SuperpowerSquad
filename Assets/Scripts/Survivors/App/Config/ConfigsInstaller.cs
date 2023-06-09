﻿using Feofun.Config;
using Feofun.Config.Serializers;
using Feofun.Localization.Config;
using Survivors.Config;
using Survivors.Enemy.Spawn.Config;
using Survivors.Loot.Config;
using Survivors.Modifiers.Config;
using Survivors.Reward.Config;
using Survivors.Session.Config;
using Survivors.Session.Model;
using Survivors.Shop.Config;
using Survivors.Squad.Config;
using Survivors.Units.Enemy.Config;
using Survivors.Units.Player.Config;
using Survivors.Units.Weapon;
using Survivors.Units.Weapon.FormationWeapon;
using Survivors.Upgrade.Config;
using Survivors.Upgrade.UpgradeSelection.Config;
using Survivors.WorldEvents.Config;
using Zenject;

namespace Survivors.App.Config
{
    public class ConfigsInstaller
    {
        public static void Install(DiContainer container)
        {
            new ConfigLoader(container, new CsvConfigDeserializer())
                .RegisterSingle<LocalizationConfig>(Configs.LOCALIZATION)
                .RegisterSingle<EnemyWavesConfig>(Configs.ENEMY_WAVES)
                .RegisterSingle<UpgradesConfig>(Configs.UPGRADES)         
                .RegisterSingle<WorldEventsConfig>(Configs.WORLD_EVENTS)
                .RegisterSingle<LootConfig>(Configs.DROPPING_LOOT)
                .RegisterSingleObjectConfig<UpgradeBranchSelectionConfig>(Configs.CONSTANTS)  
                .RegisterSingleObjectConfig<SquadConfig>(Configs.SQUAD)
                .RegisterSingleObjectConfig<ConstantsConfig>(Configs.CONSTANTS)
                .RegisterSingleObjectConfig<HpsSpawnerConfig>(Configs.ENEMY_SPAWNER)
                .RegisterStringKeyedCollection<SpawnableEnemyConfig>(Configs.SPAWNABLE_ENEMIES)
                .RegisterStringKeyedCollection<PlayerUnitConfig>(Configs.PLAYER_UNIT)
                .RegisterStringKeyedCollection<EnemyUnitConfig>(Configs.ENEMY_UNIT)
                .RegisterCollection<ProjectileFormationType, BossAttackConfig>(Configs.BOSS_ATTACK)
                .RegisterStringKeyedCollection<SquadLevelConfig>(Configs.SQUAD_LEVEL)
                .RegisterStringKeyedCollection<ParameterUpgradeConfig>(Configs.MODIFIERS, true)            
                .RegisterStringKeyedCollection<UpgradeProductConfig>(Configs.META_UPGRADES_SHOP)
                .RegisterStringKeyedCollection<ParameterUpgradeConfig>(Configs.META_UPGRADES, true)
                .RegisterStringKeyedCollection<LevelMissionConfig>(Configs.LEVEL_MISSION)
                .RegisterCollection<SessionResult, MissionRewardsConfig>(Configs.MISSION_REWARDS);
        }
    }
}