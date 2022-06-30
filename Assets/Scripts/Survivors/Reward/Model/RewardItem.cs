namespace Survivors.Reward.Model
{
    public class RewardItem
    {
        public readonly string RewardId;
        public readonly RewardType RewardType;
        public readonly int Count;
        
        public RewardItem(string rewardId, RewardType rewardType, int count)
        {
            RewardId = rewardId;
            RewardType = rewardType;
            Count = count;
        }

        protected bool Equals(RewardItem other)
        {
            return RewardId == other.RewardId && RewardType == other.RewardType && Count == other.Count;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((RewardItem)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = (RewardId != null ? RewardId.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (int)RewardType;
                hashCode = (hashCode * 397) ^ Count;
                return hashCode;
            }
        }
    }
}