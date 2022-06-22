using System.Linq;
using Feofun.Config;
using Survivors.Cheats;
using Survivors.Extension;
using Survivors.Units.Player.Config;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Survivors.UI.Cheats
{
    public class AddUnitView : MonoBehaviour
    {
        [SerializeField] private Dropdown _dropdown;
        [SerializeField] private Button _addButton;
        
        [Inject] private StringKeyedConfigCollection<PlayerUnitConfig> _playerUnitConfigs;
        [Inject] private CheatsManager _cheatsManager;
        
        private void OnEnable()
        {
            _addButton.onClick.AddListener(OnClick);
            InitDropdown();
        }
        private void InitDropdown()
        {
            _dropdown.ClearOptions();
            var allUnits = _playerUnitConfigs.Keys.ToList();
            _dropdown.AddOptions(allUnits);
        }
        private void OnDisable() => _addButton.onClick.RemoveListener(OnClick);

        private void OnClick()
        {
            _cheatsManager.AddUnit(_dropdown.options[_dropdown.value].text);
            
        }
    }
}
