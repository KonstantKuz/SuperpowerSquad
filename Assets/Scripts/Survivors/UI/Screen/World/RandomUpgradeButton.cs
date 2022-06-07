using Feofun.Config;
using Feofun.Modifiers;
using Survivors.Modifiers.Config;
using Survivors.Squad.Upgrade;
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
        }
    }
}