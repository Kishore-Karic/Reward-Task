using RewardTask.GenericSingleton;
using RewardTask.Rewards;
using UnityEngine;
using UnityEngine.UI;

namespace RewardTask.Service
{
    public class UIService : GenericSingleton<UIService>
    {
        [SerializeField] private Button openRewardsButton, subtractButton, cancelButton, collectButton;
        private RewardView rewardView;
        [SerializeField] private GameObject collectRewardPanel;

        protected override void Awake()
        {
            base.Awake();

            subtractButton.onClick.AddListener(SubtractCurrencies);
            cancelButton.onClick.AddListener(CloseRewardsPanel);
            collectButton.onClick.AddListener(CollectReward);
        }

        public void SubtractCurrencies()
        {
            EventService.Instance.OnSubtractCoins();
        }

        public void CloseRewardsPanel()
        {

        }

        public void ShowCollectReward(RewardView _rewardView)
        {
            rewardView = _rewardView;
            collectRewardPanel.SetActive(true);
        }

        public void CollectReward()
        {
            collectRewardPanel.SetActive(false);
            rewardView.RewardCollected();
        }
    }
}