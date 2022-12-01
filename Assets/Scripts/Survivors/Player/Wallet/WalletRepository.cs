using System.Collections.Generic;
using Feofun.Repository;

namespace Survivors.Player.Wallet
{
    public class WalletRepository : LocalPrefsSingleRepository<Dictionary<string, int>>
    {
        public WalletRepository() : base("Wallet_v1")
        {
        }
    }
}