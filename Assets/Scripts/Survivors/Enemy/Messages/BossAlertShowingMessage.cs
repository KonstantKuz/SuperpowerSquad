
namespace Survivors.Enemy.Messages
{
    public readonly struct BossAlertShowingMessage
    {
        public readonly float ShowDuration;

        public BossAlertShowingMessage(float showDuration)
        {
            ShowDuration = showDuration;
        }
    }
}