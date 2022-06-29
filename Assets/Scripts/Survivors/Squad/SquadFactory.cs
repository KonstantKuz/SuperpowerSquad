using Feofun.Config;
using Survivors.Extension;
using Survivors.Location;
using Survivors.Location.Service;
using Survivors.Squad.Config;
using Survivors.Squad.Model;
using Survivors.Units.Player.Config;
using Survivors.Units.Service;
using Zenject;

namespace Survivors.Squad
{
    public class SquadFactory
    {
        private const string SQUAD_NAME = "Squad";

        [Inject] private World _world;
        [Inject] private WorldObjectFactory _worldObjectFactory;
        [Inject] private StringKeyedConfigCollection<PlayerUnitConfig> _playerUnitConfigs;
        [Inject] private SquadConfig _squadConfig;
        
        public Squad CreateSquad()
        {
            var startingMaxHealth = _playerUnitConfigs.Get(UnitFactory.SIMPLE_PLAYER_ID).Health;
            var model = new SquadModel(_squadConfig, startingMaxHealth);
            var squad = _worldObjectFactory.CreateObject(SQUAD_NAME, _world.transform).RequireComponent<Squad>();
            squad.transform.SetPositionAndRotation(_world.Spawn.transform.position, _world.Spawn.transform.rotation);
            squad.Init(model);
            return squad;
        }
    }
}