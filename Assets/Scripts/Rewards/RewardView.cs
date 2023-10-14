using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RewardTask.Rewards
{
    public class RewardView : MonoBehaviour
    {
        private RewardController rewardController;

        public void SetController(RewardController _rewardController)
        {
            rewardController = _rewardController;
        }
    }
}