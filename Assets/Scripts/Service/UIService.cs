using RewardTask.Rewards;
using UnityEngine;
using UnityEngine.UI;

namespace RewardTask.Service
{
    public class UIService : MonoBehaviour
    {
        [SerializeField] private Button openRewardsButton, subtractButton, cancelButton, collectButton, quitButton;
        [SerializeField] private GameObject rewardsPanel, collectRewardPanel, loadingPanel;
        [SerializeField] private RewardService rewardService;

        private RewardView rewardView;

        protected void Awake()
        {
            openRewardsButton.onClick.AddListener(OpenRewardsPanel);
            subtractButton.onClick.AddListener(SubtractCurrencies);
            cancelButton.onClick.AddListener(CloseRewardsPanel);
            collectButton.onClick.AddListener(CollectReward);
            quitButton.onClick.AddListener(Application.Quit);
        }

        public void OpenRewardsPanel()
        {
            EventService.Instance.OnShowRewards();
            rewardsPanel.SetActive(true);
        }

        public void GotData()
        {
            loadingPanel.SetActive(false);
        }

        public void SubtractCurrencies()
        {
            EventService.Instance.OnSubtractCoins();
        }

        public void CloseRewardsPanel()
        {
            EventService.Instance.OnRewardsClosed();
            rewardsPanel.SetActive(false);
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