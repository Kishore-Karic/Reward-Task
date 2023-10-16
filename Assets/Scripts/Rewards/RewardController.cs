using RewardTask.Enum;
using RewardTask.Service;
using System.Threading.Tasks;
using UnityEngine.UI;

namespace RewardTask.Rewards
{
    public class RewardController
    {
        private RewardModel rewardModel;
        private RewardView rewardView;
        private int goUnlockTimeInSeconds, coolUnlockTimeInSeconds, currentTimer, currentCurrency;
        private RewardStatus currentStatus;
        private Image progressImage;
        private bool currencyNotEarned, rewardsPanelClosed;
        private RewardService rewardService;

        private const int SecondsInDay = 86400, SecondsInHour = 3600, SecondsInMinute = 60, CurrencySubtractValue = 5, Zero = 0, One = 1, Thousand = 1000;
        private const string HeartBeatAnimation = "heartbeat", BounceAnimation = "bounce", RotateAnimation = "rotate", EmptyString = "", SemicolonString = ":";

        public RewardController(RewardModel _rewardModel, RewardView _rewardView, Image _rewardImage, Image _currencyImage, RewardService _rewardService)
        {
            rewardModel = _rewardModel;
            rewardView = _rewardView;
            rewardService = _rewardService;

            rewardView.SetController(this);
            rewardView.CurrentStatus = rewardModel.Status;
            rewardView.CurrencyEarned = rewardModel.CurrencyEarned;
            rewardView.SetImages(_rewardImage, _currencyImage);
            
            goUnlockTimeInSeconds = rewardModel.MinimumConnectionMinutes * SecondsInMinute;
            coolUnlockTimeInSeconds = (rewardModel.AwardEveryMinutes - rewardModel.CoolDownMinutesPassed) * SecondsInMinute;

            InitializeRewards();

            EventService.Instance.onShowRewards += StartTimer;
            EventService.Instance.onRewardsClosed += StopTimer;
        }

        private void InitializeRewards()
        {
            rewardView.SetCurrentState(rewardModel.Status);
            rewardView.SetBackGround(rewardModel.Status, rewardModel.CurrencyEarned);
            rewardView.SetTime(goUnlockTimeInSeconds, coolUnlockTimeInSeconds);

            currentCurrency = rewardModel.CurrencyRequired;
            rewardView.SetCurrency(currentCurrency);

            currentStatus = rewardModel.Status;

            if (currentStatus == RewardStatus.Go)
            {
                currencyNotEarned = (rewardModel.CurrencyEarned == Zero);
                currentTimer = goUnlockTimeInSeconds;
                EventService.Instance.onSubtractCoins += SubtractCoins;
            }
            else if (currentStatus == RewardStatus.Cooling)
            {
                currentTimer = coolUnlockTimeInSeconds;
            }
        }

        public void SetProgressImage(Image _image)
        {
            progressImage = _image;
        }

        private async void StartTimer()
        {
            int hour, minute, second, totalSeconds;
            totalSeconds = rewardModel.AwardEveryMinutes * SecondsInMinute;

            rewardsPanelClosed = false;

            while (currentTimer > -One && !rewardsPanelClosed)
            {
                hour = (currentTimer % SecondsInDay) / SecondsInHour;
                minute = (currentTimer / SecondsInMinute) % SecondsInMinute;
                second = currentTimer % SecondsInMinute;

                if(currentStatus == RewardStatus.Go)
                {
                    rewardView.GoTimer.text = EmptyString + minute + SemicolonString + second;
                }
                else if(currentStatus == RewardStatus.Cooling)
                {
                    rewardView.CoolDownTimer.text = EmptyString + hour + SemicolonString + minute + SemicolonString + second;
                    progressImage.fillAmount = (float)(totalSeconds - currentTimer) / totalSeconds;
                }

                await Task.Delay(Thousand);

                currentTimer -= One;
            }

            if (currentTimer <= Zero && currentCurrency <= Zero && currentStatus == RewardStatus.Go)
            {
                ChangeStatus();
            }
        }

        private void StopTimer()
        {
            rewardView.CurrentStatus = rewardModel.Status;
            rewardsPanelClosed = true;
            InitializeRewards();
        }

        public void ChangeStatus()
        {
            switch (currentStatus)
            {
                case RewardStatus.Go:
                    rewardView.SetCurrentState(RewardStatus.Claim);
                    rewardView.SetBackGround(RewardStatus.Claim, rewardModel.CurrencyEarned);
                    rewardView.RewardButton.interactable = true;
                    currentStatus = RewardStatus.Claim;
                    rewardView.Animator.SetBool(HeartBeatAnimation, false);
                    rewardView.Animator.SetBool(BounceAnimation, true);
                    break;

                case RewardStatus.Claim:
                    rewardView.SetCurrentState(RewardStatus.Cooling);
                    rewardView.SetBackGround(RewardStatus.Cooling, rewardModel.CurrencyEarned);
                    rewardView.FadeImageTransparency(true);
                    rewardView.RewardButton.interactable = false;
                    currentTimer = coolUnlockTimeInSeconds;
                    StartTimer();
                    currentStatus = RewardStatus.Cooling;
                    rewardView.Animator.SetBool(BounceAnimation, false);
                    rewardView.Animator.SetBool(RotateAnimation, true);
                    break;

                default:
                    break;
            }
        }

        private void SubtractCoins()
        {
            currentCurrency -= CurrencySubtractValue;
            rewardView.SetCurrency(currentCurrency);

            if (currencyNotEarned)
            {
                rewardView.SetBackGround(rewardModel.Status, One);
                rewardView.Animator.SetBool(HeartBeatAnimation, true);
                currencyNotEarned = false;
            }

            if(currentCurrency <= Zero)
            {
                EventService.Instance.onSubtractCoins -= SubtractCoins;
            }

            if (currentTimer <= Zero && currentCurrency <= Zero && currentStatus == RewardStatus.Go)
            {
                ChangeStatus();
            }
        }

        public void RequestedForCollectRewards(RewardView _rewardView)
        {
            rewardService.RequestedForCollectRewards(_rewardView);
        }
    }
}