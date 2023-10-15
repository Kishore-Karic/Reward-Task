using RewardTask.GenericSingleton;
using System;

namespace RewardTask.Service
{
    public class EventService : GenericSingleton<EventService>
    {
        public event Action onSubtractCoins;

        public void OnSubtractCoins()
        {
            onSubtractCoins?.Invoke();
        }
    }
}