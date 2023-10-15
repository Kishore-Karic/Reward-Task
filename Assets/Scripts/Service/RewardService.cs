using System.Collections.Generic;
using UnityEngine;
using RewardTask.Service;

namespace RewardTask.Rewards
{
    public class RewardService : MonoBehaviour
    {
        [SerializeField] private List<RewardView> rewardPrefabsList;
        [SerializeField] private APIService apiService;

        public void SpawnRewards()
        {
            for(int i = 0; i < apiService.RewardModelPropertiesList.Count; i++)
            {
                new RewardController(new RewardModel(apiService.RewardModelPropertiesList[i]), rewardPrefabsList[i], apiService.RewardSpritesList[i], apiService.CurrencySpritesList[i]);
            }
        }
    }
}