using UnityEngine;

namespace RewardTask.Rewards
{
    public class RewardService : MonoBehaviour
    {
        private RewardsList rewardsList;

        public void SpawnRewards(RewardsList _rewardsList)
        {
            rewardsList = _rewardsList;

            for(int i = 0; i < rewardsList.rewards.Length; i++)
            {

            }
        }
    }
}