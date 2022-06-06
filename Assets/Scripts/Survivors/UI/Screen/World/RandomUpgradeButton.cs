using Feofun.Config;
using Feofun.Modifiers;
using Survivors.Modifiers.Config;
using Survivors.Squad.Upgrade;
using Survivors.Squad.Upgrade.Config;
using Survivors.Units.Service;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Survivors.UI.Screen.World
{
    [RequireComponent(typeof(Button))]
    public class RandomUpgradeButton : MonoBehaviour
    {
        [Inject] private Location.World _world;
        [Inject] private ModifierFactory _modifierFactory;
        [Inject] private UpgradeService _upgradeService;
        [Inject] private StringKeyedConfigCollection<ParameterUpgradeConfig> _modifierConfigs;
        private void Awake()
        {
            GetComponent<Button>().onClick.AddListener(AddRandomUpgrade);
        }

        private void AddRandomUpgrade()
        {
            _upgradeService.AddRandomUpgrade();
            // UpgradePyroUnit();
        }

        private void UpgradePyroUnit()
        {
            AddModifier("DamageAngle5", "PyroUnit");
            AddModifier("AttackDistance1", "PyroUnit");
            AddModifier("ProjectileSpeed25", "PyroUnit");
        }
        
        private void AddModifier(string modifierId, string unitId)
        {
            var modifierConfig = _modifierConfigs.Get(modifierId);
            var modifier = _modifierFactory.Create(modifierConfig.ModifierConfig);
            _world.Squad.AddModifier(modifier, modifierConfig.Target, unitId);
        }
    }
}