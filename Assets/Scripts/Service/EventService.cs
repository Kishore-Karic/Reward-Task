using RewardTask.GenericSingleton;
using System;

namespace RewardTask.Service
{
    public class EventService : GenericSingleton<EventService>
    {
        public event Action onSubtractCoins, onShowRewards, onRewardsClosed;

        public void OnSubtractCoins()
        {
            onSubtractCoins?.Invoke();
        }

        public void OnShowRewards()
        {
            onShowRewards?.Invoke();
        }

        public void OnRewardsClosed()
        {
            onRewardsClosed?.Invoke();
        }
    }
}