using System.Collections.Generic;
using System.Linq;

namespace Survivors.UI.Dialog.UpgradeDialog.Star
{
    public class StarViewModel
    {
        public StarState State { get; }
        
        public StarViewModel(StarState state)
        {
            State = state;
        }
        public static IEnumerable<StarViewModel> CreateCollection(int count, StarState state)
        {
            return Enumerable.Range(0, count).Select(_ => new StarViewModel(state));
        }
    }
}