using Feofun.Components;

namespace Survivors.WorldEvents.Events.Tornado.Swirler
{
    public class SquadSwirler : Swirler, IInitializable<Squad.Squad>
    {
        private Squad.Squad _squad;
        
        public void Init(Squad.Squad owner)
        {
            _squad = owner;
        }
        protected override void Capture()
        {
            _squad.IsActive = false;
        }
        protected override void Release()
        {
            _squad.IsActive = true;
        }
    }
}